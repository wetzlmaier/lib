using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.ComponentModel;
using Scch.Common.Globalization;

namespace Scch.DomainModel.EntityFramework.Tests
{
    public static class TranslationTest
    {
        public static void TestTranslate<TEntityBase>(Assembly assembly)
        {
            var failedAssertions = new List<string>();

            foreach (var entityType in assembly.GetExportedTypes())
            {
                if (!typeof(TEntityBase).IsAssignableFrom(entityType) || entityType.IsAbstract)
                    continue;

                var translated = Translator.Translate(entityType);
                if (translated == null)
                    failedAssertions.Add("Missing translation for type " + entityType.Name);

                if (translated != null && !ValidationTest.ReplaceUml(translated).Contains(entityType.Name))
                    failedAssertions.Add("Translation incorrekt for type " + entityType.Name);

                foreach (PropertyInfo propertyInfo in entityType.GetProperties())
                {
                    var attributes = (BrowsableAttribute[])propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), true);
                    if (attributes.Length == 0 || !attributes[0].Browsable)
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(ConcurrencyCheckAttribute), true).Length > 0)
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                        continue;

                    if (propertyInfo.DeclaringType==typeof(DataErrorInfo))
                        continue;

                    translated = Translator.Translate(propertyInfo);
                    if (translated == null)
                        failedAssertions.Add("Missing translation for property " + entityType.Name + "." + propertyInfo.Name);

                    if (translated != null && !ValidationTest.ReplaceUml(translated).Contains(propertyInfo.Name))
                        failedAssertions.Add("Translation incorrekt for property " + entityType.Name + "." + propertyInfo.Name);
                }
            }

            foreach (string failedAssertion in failedAssertions)
                Console.WriteLine(failedAssertion);

            Assert.AreEqual(0, failedAssertions.Count);
        }
    }
}
