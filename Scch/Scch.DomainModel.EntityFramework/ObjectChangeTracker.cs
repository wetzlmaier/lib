using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Scch.Common.Linq.Expressions;

namespace Scch.DomainModel.EntityFramework
{
    /// <summary>
    /// Helper class that captures most of the change tracking work that needs to be done for self tracking entities.
    /// </summary>
    public class ObjectChangeTracker
    {
        #region  Fields

        private bool _isDeserializing;
        private ObjectState _objectState = ObjectState.Added;
        private bool _changeTrackingEnabled;
        private OriginalValuesDictionary _originalValues;
        private ExtendedPropertiesDictionary _extendedProperties;
        private ObjectsAddedToCollectionProperties _objectsAddedToCollections = new ObjectsAddedToCollectionProperties();
        private ObjectsRemovedFromCollectionProperties _objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties();

        #endregion

        #region Events

        public event EventHandler<ObjectStateChangingEventArgs> ObjectStateChanging;

        #endregion

        protected virtual void OnObjectStateChanging(ObjectState newState)
        {
            if (ObjectStateChanging != null)
            {
                ObjectStateChanging(this, new ObjectStateChangingEventArgs { NewState = newState });
            }
        }

        [DataMember]
        public ObjectState State
        {
            get { return _objectState; }
            set
            {
                if (_isDeserializing || _changeTrackingEnabled)
                {
                    OnObjectStateChanging(value);
                    _objectState = value;
                }
            }
        }

        public bool ChangeTrackingEnabled
        {
            get { return _changeTrackingEnabled; }
            set { _changeTrackingEnabled = value; }
        }

        // Returns the removed objects to collection valued properties that were changed.
        [DataMember]
        public ObjectsRemovedFromCollectionProperties ObjectsRemovedFromCollectionProperties
        {
            get {
                return _objectsRemovedFromCollections ??
                       (_objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties());
            }
        }

        // Returns the original values for properties that were changed.
        [DataMember]
        public OriginalValuesDictionary OriginalValues
        {
            get { return _originalValues ?? (_originalValues = new OriginalValuesDictionary()); }
        }

        // Returns the extended property values.
        // This includes key values for independent associations that are needed for the
        // concurrency model in the Entity Framework
        [DataMember]
        public ExtendedPropertiesDictionary ExtendedProperties
        {
            get { return _extendedProperties ?? (_extendedProperties = new ExtendedPropertiesDictionary()); }
        }

        // Returns the added objects to collection valued properties that were changed.
        [DataMember]
        public ObjectsAddedToCollectionProperties ObjectsAddedToCollectionProperties
        {
            get {
                return _objectsAddedToCollections ??
                       (_objectsAddedToCollections = new ObjectsAddedToCollectionProperties());
            }
        }

        #region MethodsForChangeTrackingOnClient

        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            _isDeserializing = true;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            _isDeserializing = false;
        }

        // Resets the ObjectChangeTracker to the Unchanged state and
        // clears the original values as well as the record of changes
        // to collection properties
        public void AcceptChanges()
        {
            OnObjectStateChanging(ObjectState.Unchanged);
            OriginalValues.Clear();
            ObjectsAddedToCollectionProperties.Clear();
            ObjectsRemovedFromCollectionProperties.Clear();
            ChangeTrackingEnabled = true;
            _objectState = ObjectState.Unchanged;
        }

        // Captures the original value for a property that is changing.
        public void RecordOriginalValue(string propertyName, object value)
        {
            if (_changeTrackingEnabled && _objectState != ObjectState.Added)
            {
                if (!OriginalValues.ContainsKey(propertyName))
                {
                    OriginalValues[propertyName] = value;
                }
            }
        }

        public void RecordOriginalValue<T>(Expression<Func<T>> property, object value)
        {
            RecordOriginalValue(ExpressionHelper.GetPropertyPath(property), value);
        }

        // Records an addition to collection valued properties on SelfTracking Entities.
        public void RecordAdditionToCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Add the entity back after deleting it, we should do nothing here then
                if (ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName)
                    && ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName].Remove(value);
                    if (ObjectsRemovedFromCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsRemovedFromCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsAddedToCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsAddedToCollectionProperties[propertyName] = new ObjectList {value};
                }
                else
                {
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
            }
        }

        // Records a removal to collection valued properties on SelfTracking Entities.
        public void RecordRemovalFromCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Delete the entity back after adding it, we should do nothing here then
                if (ObjectsAddedToCollectionProperties.ContainsKey(propertyName)
                    && ObjectsAddedToCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsAddedToCollectionProperties[propertyName].Remove(value);
                    if (ObjectsAddedToCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsAddedToCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName] = new ObjectList {value};
                }
                else
                {
                    if (!ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                    {
                        ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                    }
                }
            }
        }
        #endregion
    }
}
