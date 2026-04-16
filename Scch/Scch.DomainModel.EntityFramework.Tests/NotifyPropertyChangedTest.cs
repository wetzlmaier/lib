using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.ComponentModel;

namespace Scch.DomainModel.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for NotifyPropertyChangedTest
    /// </summary>
    [TestClass]
    public class NotifyPropertyChangedTest
    {
        public NotifyPropertyChangedTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        private PropertyChangedEventArgs _pargs;
        private NotifyCollectionChangedEventArgs _cargs;

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestNotifyPropertyChanged()
        {
            foreach (var entityType in typeof(Anweisungstyp).Assembly.GetExportedTypes())
            {
                if (!typeof(EntityFrameworkEntity<long>).IsAssignableFrom(entityType) || entityType.IsAbstract)
                    continue;

                var instance = (EntityFrameworkEntity<long>)Activator.CreateInstance(entityType);
                instance.PropertyChanged += instance_PropertyChanged;

                foreach (PropertyInfo propertyInfo in entityType.GetProperties())
                {
                    var attributes = (BrowsableAttribute[])propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), true);
                    if (attributes.Length == 0 || !attributes[0].Browsable)
                        continue;

                    if (propertyInfo.DeclaringType == typeof(DataErrorInfo))
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(ConcurrencyCheckAttribute), true).Length > 0)
                        continue;

                    if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                        continue;

                    _pargs = null;
                    _cargs = null;

                    if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType) && !propertyInfo.PropertyType.IsArray)
                    {
                        Type genericType = propertyInfo.PropertyType.GetGenericArguments()[0];
                        var collection = (IList)propertyInfo.GetValue(instance, null);
                        var collectionChanged = (INotifyCollectionChanged)propertyInfo.GetValue(instance, null);
                        collectionChanged.CollectionChanged += collectionChanged_CollectionChanged;
                        collection.Add(Activator.CreateInstance(genericType));
                        collectionChanged.CollectionChanged -= collectionChanged_CollectionChanged;
                        Assert.IsNotNull(_cargs);
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, _cargs.Action);
                    }
                    else
                    {
                        object value;
                        switch (propertyInfo.PropertyType.FullName)
                        {
                            case "System.DateTime":
                                value = DateTime.Now;
                                break;

                            case "System.Decimal":
                                value = 1.11m;
                                break;

                            case "System.String":
                                value = "x";
                                break;

                            case "System.Int32":
                                value = 2;
                                break;

                            case "System.Int64":
                                value = 3;
                                break;

                            case "System.Boolean":
                                value = true;
                                break;

                            case "System.Byte[]":
                                value = new byte[] {0x1};
                                break;

                            default:
                                if (typeof(EntityFrameworkEntity<long>).IsAssignableFrom(propertyInfo.PropertyType))
                                {
                                    value = Activator.CreateInstance(propertyInfo.PropertyType);
                                    break;
                                }

                                throw new InvalidOperationException(string.Format("Unknown type: '{0}'",
                                                                                  propertyInfo.PropertyType.FullName));
                        }

                        _pargs = null;
                        propertyInfo.SetValue(instance, value, null);
                        Assert.IsNotNull(_pargs, entityType.Name + "." + propertyInfo.Name);
                        Assert.AreEqual(propertyInfo.Name, _pargs.PropertyName, entityType.Name + "." + propertyInfo.Name);
                    }
                }

                instance.PropertyChanged -= instance_PropertyChanged;
            }
        }

        void collectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _cargs = e;
        }

        void instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsValid")
                _pargs = e;
        }
    }
}
