using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.DomainModel.EntityFramework;
using IndexAttribute = Scch.Common.ComponentModel.DataAnnotations.IndexAttribute;

namespace Scch.DataAccess.EntityFramework
{
    public class ExtendedDbContext<TKey> : DbContext where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        const string ColumnDelimiter = ", ";
        private readonly ICollection<DbTypeMetadata> _metadata;
        private readonly bool _auditingEnabled;

        public ExtendedDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext,
                                 ICollection<DbTypeMetadata> metadata, bool auditingEnabled)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            _metadata = metadata;
            _auditingEnabled = auditingEnabled;
            objectContext.ObjectMaterialized += HandleObjectMaterialized;
        }

        private void HandleObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var entity = e.Entity as IObjectWithChangeTracker;
            if (entity != null)
            {
                bool changeTrackingEnabled = entity.ChangeTracker.ChangeTrackingEnabled;
                try
                {
                    entity.MarkAsUnchanged();
                }
                finally
                {
                    entity.ChangeTracker.ChangeTrackingEnabled = changeTrackingEnabled;
                }

                ObjectContext.StoreReferenceKeyValues(entity);
            }
        }

        public override int SaveChanges()
        {
            if (_auditingEnabled)
                WriteAuditLog();

            IncrementVersion();
            return base.SaveChanges();
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            if (_auditingEnabled)
                WriteAuditLog();

            IncrementVersion();
            ObjectContext.SaveChanges(saveOptions);
        }

        public bool DatabaseExists()
        {
            return ObjectContext.DatabaseExists();
        }

        public void CreateDatabase()
        {
            var scripts = new List<string>();
            scripts.AddRange(CreateDefaultsScripts());
            scripts.AddRange(CreateIndexScripts());
            scripts.AddRange(CreateConstraintsScripts());

            ObjectContext.CreateDatabase();

            foreach (string sql in scripts)
            {
                try
                {
                    ObjectContext.ExecuteStoreCommand(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not execute: " + sql + "-->" + ex.Message);
                    throw;
                }
            }
        }

        public void FillDatabase(string sqlScript)
        {
            var sql = new StringBuilder();
            foreach (string line in sqlScript.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None))
            {
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("--"))
                    continue;

                sql.Append(" ");
                if (!trimmedLine.EndsWith(";") && trimmedLine.ToUpper()!="GO")
                {
                    sql.Append(trimmedLine);
                    continue;
                }

                if (trimmedLine.EndsWith(";"))
                {
                    sql.Append(trimmedLine.Substring(0, trimmedLine.Length - 1));
                }

                try
                {
                    ObjectContext.ExecuteStoreCommand(sql.ToString().Trim());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not execute: " + sql + "-->" + ex.Message);
                }
                sql.Clear();
            }
        }

        public string CreateDatabaseScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(ObjectContext.CreateDatabaseScript());
            sb.AppendLine(string.Join(Environment.NewLine, CreateDefaultsScripts()));
            sb.AppendLine(string.Join(Environment.NewLine, CreateIndexScripts()));
            sb.AppendLine(string.Join(Environment.NewLine, CreateConstraintsScripts()));

            return sb.ToString();
        }

        private string[] CreateDefaultsScripts()
        {
            const string createDefaultSql = "ALTER TABLE {0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}";

            return GetDefaults().Select(info => string.Format(createDefaultSql, info.TableName, info.ColumnName,
                (info.ColumnType == typeof(string) ? "'" + info.DefaultValueExpression + "'" : info.DefaultValueExpression))).ToArray();
        }

        private string[] CreateConstraintsScripts()
        {
            const string createCheckSql = "ALTER TABLE {0} WITH CHECK ADD CONSTRAINT {1} CHECK ({2})\r\n" +
               "ALTER TABLE {0} CHECK CONSTRAINT {1}";

            return GetChecks().Select(info => string.Format(createCheckSql, info.TableName, "CK_" + info.TableName + "_" + info.ColumnName, info.CheckExpression)).ToArray();
        }

        private string[] CreateIndexScripts()
        {
            var script = new List<string>();
            var lastIndex = new IndexInfo("", 0, "", IndexDirection.Ascending, "", false);
            var columns = new StringBuilder();

            var indexes = GetIndexes().OrderBy(one => one.TableName).ThenBy(two => two.IndexName).ThenBy(three => three.OrdinalPoistion);
            int ordinalPosition = 0;
            foreach (var info in indexes)
            {
                if (lastIndex.IndexName != info.IndexName && columns.Length > 0)
                {
                    script.Add(CreateIndexSql(columns, lastIndex));
                    ordinalPosition = 0;
                }

                if (ordinalPosition != info.OrdinalPoistion)
                    throw new InvalidOperationException(string.Format("Index '{0}' for table '{1}' has the invalid ordinal position '{2}'. It should be '{3}'.", info.IndexName, info.TableName, info.OrdinalPoistion, ordinalPosition));

                columns.Append(ColumnDelimiter + (info.Direction == IndexDirection.Ascending ? info.ColumnName + " ASC " : info.ColumnName + " DESC "));
                lastIndex = info;
                ordinalPosition++;
            }

            if (columns.Length > 0)
                script.Add(CreateIndexSql(columns, lastIndex));

            return script.ToArray();
        }

        private string CreateIndexSql(StringBuilder columns, IndexInfo indexInfo)
        {
            const string createIndexSql = "CREATE NONCLUSTERED INDEX {0} ON {1} ({2})";
            const string createUniqueSql = "ALTER TABLE {1} ADD CONSTRAINT {0} UNIQUE NONCLUSTERED ({2})";

            columns.Remove(0, ColumnDelimiter.Length);
            var sql = string.Format(indexInfo.IsUnique ? createUniqueSql : createIndexSql, indexInfo.IndexName, indexInfo.TableName, columns);
            columns.Clear();
            return sql;
        }

        private void WriteAuditLog()
        {
            var entries = from entry in ChangeTracker.Entries()
                          where (entry.State == EntityState.Added || entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                          select entry;

            foreach (var entry in entries)
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                foreach (AuditLog logEntry in EntityFrameworkHelper.GetAuditRecordsForChange(entry, identity == null ? string.Empty : identity.Name))
                {
                    Set<AuditLog>().Add(logEntry);
                }
            }
        }

        private void IncrementVersion()
        {
            ChangeTracker.DetectChanges(); // Important!

            var entries = from entry in ChangeTracker.Entries()
                          where (entry.State == EntityState.Added || entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                          select entry;

            foreach (var entry in entries)
            {
                ((IEntityFrameworkEntity<TKey>)entry.Entity).IncrementVersion();
            }
        }

        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

        #region Metadata

        /// <summary>
        /// Get check constraint collection from the DbContext
        /// </summary>
        /// <returns>Collection of defaults</returns>
        public IEnumerable<CheckInfo> GetChecks()
        {
            var checks = new List<CheckInfo>();

            foreach (var type in _metadata)
            {
                foreach (var property in type.Properties)
                {
                    var attribute = (RangeAttribute)property.Attributes.FirstOrDefault(
                            a => a.GetType().IsAssignableFrom(typeof(RangeAttribute)));

                    if (attribute != null)
                    {
                        Debug.Assert(attribute != null, "attribute != null");
                        string columnName = GetColumnName(property);
                        string tableName = GetTableName(property);

                        string min = null;
                        string max = null;
                        switch (attribute.OperandType.FullName)
                        {
                            case "System.Int32":
                                if ((int)attribute.Minimum != int.MinValue)
                                    min = property.PropertyInfo.Name + " >= " + attribute.Minimum;
                                if ((int)attribute.Maximum != int.MaxValue)
                                    max = property.PropertyInfo.Name + " <= " + attribute.Maximum;
                                break;

                            case "System.Double":
                                if (Math.Abs((double)attribute.Minimum - double.MinValue) > 1E-5)
                                    min = property.PropertyInfo.Name + " >= " + attribute.Minimum;
                                if (Math.Abs((double)attribute.Maximum - double.MaxValue) > 1E-5)
                                    max = property.PropertyInfo.Name + " <= " + attribute.Maximum;
                                break;
                            default:
                                throw new InvalidOperationException("Unknown type.");
                        }

                        var sb = new StringBuilder();
                        if (min != null)
                            sb.Append("(" + min + ") ");

                        if (min != null && max != null)
                            sb.Append("AND");

                        if (max != null)
                            sb.Append(" (" + max + ")");

                        checks.Add(new CheckInfo(tableName, columnName, sb.ToString().Trim()));
                    }
                }
            }

            return checks;
        }

        /// <summary>
        /// Get indexes collection from the DbContext
        /// </summary>
        /// <returns>Collection of defaults</returns>
        public IEnumerable<IndexInfo> GetIndexes()
        {
            var indexes = new List<IndexInfo>();

            foreach (var type in _metadata)
            {
                foreach (var property in type.Properties)
                {
                    var attributes = property.Attributes.Where(attribute => attribute.GetType().IsAssignableFrom(typeof(IndexAttribute))).Cast<IndexAttribute>();

                    foreach (var attribute in attributes)
                    {
                        string columnName = GetColumnName(property);
                        var tableName = GetTableName(property);

                        string indexName = attribute.IndexName;

                        if (string.IsNullOrEmpty(indexName))
                        {
                            indexName = "IX_" + tableName + "_" + columnName;
                        }

                        indexes.Add(new IndexInfo(indexName, attribute.OrdinalPoistion, columnName, attribute.Direction, tableName, attribute.IsUnique));
                    }
                }
            }
            return indexes;
        }

        private string GetColumnName(DbPropertyMetadata property)
        {
            var columnAttribute = (ColumnAttribute)property.Attributes.FirstOrDefault(a => a.GetType().IsAssignableFrom(typeof(ColumnAttribute)));
            return columnAttribute != null ? columnAttribute.Name : property.PropertyInfo.Name;
        }

        private string GetTableName(DbPropertyMetadata property)
        {
            var tableAttribute = (TableAttribute)property.Attributes.FirstOrDefault(
                        a => a.GetType().IsAssignableFrom(typeof(TableAttribute)));

            var tableName = tableAttribute != null ? tableAttribute.Name : string.Empty;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = property.Type.ItemType.Name;
            }

            return tableName;
        }

        /// <summary>
        /// Get defaults collection from the DbContext
        /// </summary>
        /// <returns>Collection of defaults</returns>
        public IEnumerable<DefaultInfo> GetDefaults()
        {
            var defaults = new List<DefaultInfo>();

            foreach (var type in _metadata)
            {
                foreach (var property in type.Properties)
                {
                    var attribute = (DefaultAttribute)property.Attributes.FirstOrDefault(
                            a => a.GetType().IsAssignableFrom(typeof(DefaultAttribute)));

                    if (attribute != null)
                    {
                        Debug.Assert(attribute != null, "attribute != null");
                        string columnName = GetColumnName(property);
                        string tableName = GetTableName(property);

                        defaults.Add(new DefaultInfo(tableName, columnName, property.PropertyInfo.PropertyType, attribute.DefaultValueExpression));
                    }
                }
            }
            return defaults;
        }
        #endregion
    }
}
