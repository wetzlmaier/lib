using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Scch.DataAccess.EntityFramework
{
    public static class EntityLogger
    {
        public static void LogEntities(DbContext context)
        {
            return;
/*
            context.ChangeTracker.DetectChanges(); // Important!

            var entries = from e in context.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        Console.WriteLine("Adding a {0}", entry.Entity.GetType());
                        PrintPropertyValues(entry.CurrentValues, entry.CurrentValues.PropertyNames);
                        break;
                    case EntityState.Deleted:
                        Console.WriteLine("Deleting a {0}", entry.Entity.GetType());
                        PrintPropertyValues(entry.OriginalValues, GetKeyPropertyNames(context, entry.Entity));
                        break;
                    case EntityState.Modified:
                        Console.WriteLine("Modifying a {0}", entry.Entity.GetType());
                        DbEntityEntry entry1 = entry;
                        var modifiedPropertyNames = from n in entry.CurrentValues.PropertyNames
                                                    where entry1.Property(n).IsModified
                                                    select n;
                        PrintPropertyValues(entry.CurrentValues, GetKeyPropertyNames(context, entry.Entity).Concat(modifiedPropertyNames));
                        break;
                }
            }*/
        }

        private static void PrintPropertyValues(DbPropertyValues values, IEnumerable<string> propertiesToPrint, int indent = 1)
        {
            foreach (var propertyName in propertiesToPrint)
            {
                var value = values[propertyName];
                if (value is DbPropertyValues)
                {
                    Console.WriteLine("{0}- Complex Property: {1}", string.Empty.PadLeft(indent), propertyName);
                    var complexPropertyValues = (DbPropertyValues)value;
                    PrintPropertyValues(complexPropertyValues, complexPropertyValues.PropertyNames, indent + 1);
                }
                else
                {
                    Console.WriteLine("{0}- {1}: {2}", string.Empty.PadLeft(indent), propertyName, values[propertyName]);
                }
            }
        }

        private static IEnumerable<string> GetKeyPropertyNames(DbContext context, object entity)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.ObjectStateManager.GetObjectStateEntry(entity).EntityKey.EntityKeyValues.Select(k => k.Key);
        }
    }
}
