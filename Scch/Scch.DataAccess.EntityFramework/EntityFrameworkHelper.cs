using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Scch.DomainModel;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.EntityFramework
{
    internal static class EntityFrameworkHelper
    {
        private static string GetKeyValues(DbEntityEntry dbEntry)
        {
            var keyValues = new StringBuilder();

            foreach (var property in dbEntry.Entity.GetType().GetProperties())
            {
                if (property.GetCustomAttributes(typeof(KeyAttribute), false).Any())
                {
                    DbPropertyValues values = (dbEntry.State == EntityState.Deleted)
                                                  ? dbEntry.OriginalValues
                                                  : dbEntry.CurrentValues;
                    keyValues.Append(", " + values.GetValue<object>(property.Name));
                }
            }

            if (keyValues.Length > 1)
                keyValues.Remove(0, 2);

            return keyValues.ToString();
        }

        internal static IEnumerable<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            var result = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            DbPropertyValues values = (dbEntry.State == EntityState.Deleted)
                              ? dbEntry.OriginalValues
                              : dbEntry.CurrentValues;

            string description = (values.ToObject() is IDescribable)
                                     ? ((IDescribable) values.ToObject()).Describe()
                                     : values.ToObject().ToString();

            switch (dbEntry.State)
            {
                case EntityState.Added:
                    result.Add(new AuditLog
                    {
                        UserId = userId,
                        EventDateUtc = changeTime,
                        EventType = "A", // Added
                        TableName = tableName,
                        RecordId = GetKeyValues(dbEntry),
                        ColumnName = null,
                        NewValue = description
                    });
                    break;

                case EntityState.Deleted:
                    result.Add(new AuditLog
                    {
                        UserId = userId,
                        EventDateUtc = changeTime,
                        EventType = "D", // Deleted
                        TableName = tableName,
                        RecordId = GetKeyValues(dbEntry),
                        ColumnName = null,
                        NewValue = description
                    });
                    break;

                case EntityState.Modified:
                    result.AddRange(from propertyName in dbEntry.OriginalValues.PropertyNames
                                    where !Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName))
                                    select new AuditLog
                                    {
                                        UserId = userId,
                                        EventDateUtc = changeTime,
                                        EventType = "M", // Modified
                                        TableName = tableName,
                                        RecordId = GetKeyValues(dbEntry),
                                        ColumnName = propertyName,
                                        OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                        NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                                    });
                    break;
            }

            return result;
        }
    }
}
