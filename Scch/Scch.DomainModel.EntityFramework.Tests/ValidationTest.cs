using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.ComponentModel;

namespace Scch.DomainModel.EntityFramework.Tests
{
    public static class ValidationTest
    {
        public static void TestValidation<TEntityBase>(Assembly assembly)
        {
            var failedAssertions = new List<string>();
            foreach (var entityType in assembly.GetExportedTypes())
            {
                if (typeof(TEntityBase).IsAssignableFrom(entityType) && !entityType.IsAbstract)
                    TestIfResourceNameExists(entityType, failedAssertions);
            }

            foreach (string failedAssertion in failedAssertions)
                Console.WriteLine(failedAssertion);

            Assert.AreEqual(0, failedAssertions.Count);
        }

        private static void TestIfResourceNameExists(Type entityType, List<string> failedAssertions)
        {
            var resourceManager = new ResourceManager(entityType.Assembly.GetName().Name + ".Properties.Resources", entityType.Assembly);

            foreach (var propertyInfo in entityType.GetProperties())
            {
                if (propertyInfo.DeclaringType==typeof(DataErrorInfo))
                    continue;

                foreach (var attribute in propertyInfo.GetCustomAttributes(true))
                {
                    string resourceName = propertyInfo.DeclaringType.Name + propertyInfo.Name;

                    switch (attribute.GetType().Name)
                    {
                        case "RequiredAttribute":
                            resourceName += "_Required";
                            if (resourceName != ((RequiredAttribute)attribute).ErrorMessageResourceName)
                                failedAssertions.Add(string.Format("Wrong resource name '{0}' should be '{1}'",
                                    ((RequiredAttribute)attribute).ErrorMessageResourceName, resourceName));
                            break;

                        case "RangeAttribute":
                            resourceName += "_Range";
                            if (resourceName != ((RangeAttribute)attribute).ErrorMessageResourceName)
                                failedAssertions.Add(string.Format("Wrong resource name '{0}' should be '{1}'",
                                    ((RangeAttribute)attribute).ErrorMessageResourceName, resourceName));
                            break;

                        case "StringLengthAttribute":
                            resourceName += "_Length";
                            if (resourceName != ((StringLengthAttribute)attribute).ErrorMessageResourceName)
                                failedAssertions.Add(string.Format("Wrong resource name '{0}' should be '{1}'",
                                    ((StringLengthAttribute)attribute).ErrorMessageResourceName, resourceName));
                            break;

                        case "LocalizedDisplayNameAttribute":
                            resourceName += "_DisplayName";
                            if (resourceName != ((LocalizedDisplayNameAttribute)attribute).DisplayNameResourceName)
                                failedAssertions.Add(string.Format("Wrong resource name '{0}' should be '{1}'",
                                    ((LocalizedDisplayNameAttribute)attribute).DisplayNameResourceName, resourceName));
                            break;
                        default:
                            continue;
                    }

                    var value = resourceManager.GetString(resourceName);
                    if (value == null)
                        failedAssertions.Add(resourceName + " is missing.");
                }
            }
        }

        public static string ReplaceUml(string str)
        {
            return str.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ä", "Ae").Replace("Ö", "Oe").Replace(
                "Ü", "Ue");
        }
    }
}
