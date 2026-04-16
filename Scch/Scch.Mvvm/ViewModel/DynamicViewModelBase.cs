using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Scch.Mvvm.ViewModel
{
    public abstract class DynamicViewModelWrapperBase<T> : DynamicObject, INotifyPropertyChanged, IDisposable where T:class 
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static bool? _isInDesignMode;

        /// <summary>
        /// Indicates if the <c>VerifyPropertyName</c> and <c>OnPropertyChanged</c>
        /// methods should raise an exception if the property name doesn't exist.
        /// </summary>
        /// <remarks>
        /// When <c>true</c>, the <c>VerifyPropertyName</c> and <c>OnPropertyChanged</c>
        /// methods will throw an exception if the supplied property name doesn't exist.
        /// If this property is <c>false</c>, then the two methods will instead
        /// print a message with <c>Debug.Fail()</c>
        /// </remarks>
        /// <value>
        /// The default value is <c>false</c>, so the <c>Debug.Fail()</c> will be
        /// used by default.
        /// </value>
        protected virtual bool ThrowOnInvalidPropertyName { get; set; }

        
        /// <summary>
        /// The underlying model instance that this view-model instance is encapsulating
        /// </summary>
        public T ModelInstance { get; private set; }


        /// <summary>
        /// Indicates if the model instance raises property changed events
        /// </summary>
        protected bool ModelRaisesPropertyChangedEvents { get; private set; }

        protected DynamicViewModelWrapperBase(T model):this(model, null)
        {
        }

        /// <summary>
        /// Creates a new view-model instance that encapsulates the given model instance.
        /// </summary>
        /// <param name="model">The non-null model instance to encapsulate</param>
        /// <param name="messenger"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Contracts", "Requires")]
        protected DynamicViewModelWrapperBase(T model, IMessenger messenger)
        {
            if (model==null)
                throw new ArgumentNullException("model");

            ModelInstance = model;

            if (model is INotifyPropertyChanged)
            {
                ModelRaisesPropertyChangedEvents = true;

                var raisesPropChangedEvents = model as INotifyPropertyChanged;
                raisesPropChangedEvents.PropertyChanged += OnModelPropertyChanged;
            }

            MessengerInstance = messenger;
            ThrowOnInvalidPropertyName = true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentOutOfRangeException("propertyName");

            if (ModelInstance == null)
                throw new InvalidOperationException("This view-model instance has already been disposed; it no longer supports 'get' opertions");
            VerifyPropertyName(propertyName);

            // Is anybody out there?
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler == null) return;

            // Somebody is listening, so raise a property changed event
            var args = new PropertyChangedEventArgs(propertyName);
            handler(this, args);
        }


        /// <summary>
        /// One of the nuances of WPF data-binding is that they will fail silently
        /// </summary>
        /// <remarks>
        /// With the dynamic proxy properties this method must check both the model and the view-model classes
        /// before deciding if a property name is spelled incorrectly.
        /// </remarks>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        protected void VerifyPropertyName(string propertyName)
        {
            // If the property exists on the model then there's nothing to do
            if (TypeDescriptor.GetProperties(ModelInstance)[propertyName] != null) 
                return;

            // Or if the property exists on the view-model there's nothing to do
            if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;

            // The property didn't exist, we're going to raise an error of some kind
            string message = "Invalid property name: " + propertyName;
            if (ThrowOnInvalidPropertyName)
                throw new InvalidOperationException(message);
            
            Debug.Fail(message);
        }


        /// <summary>
        /// Called when the underlying model instance raises the property changed event.
        /// </summary>
        /// <remarks>
        /// This method re-raises the property changed event as through it was comming from this view-model
        /// instance.  This may cause the view layer (XAML data bindings) to update.
        /// </remarks>
        /// <param name="sender">Should be the underlying model instance</param>
        /// <param name="args">Information about the property that changed</param>
        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RaisePropertyChanged(args.PropertyName);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Dynamic capabilities
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when a property is accessed that is not actually defined on this class.
        /// (Note that if the property is actually defined on the view-model class then it is called directly and no automatic
        /// delegation to the model occurs).
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation.
        /// The <c>binder.Name</c> property provides the name of the member on which the dynamic operation is performed.
        /// </param>
        /// <param name="result">
        /// The result of the get operation.
        /// In this case, this will be the result of accessing the property with the name <c>binder.Name</c> on the model instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the underlying model instance has a property named <c>binder.Name</c> that is readable;
        /// returns <c>false</c> if the underlying model instance does NOT have such a property, or it is not readable.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (ModelInstance == null)
                throw new InvalidOperationException("This view-model instance has already been disposed; it no longer supports 'get' operations");

            // A caller is trying to access a property named binder.Name on the view-model.
            // No such statically defined property exists (or it would have been called rather than this method)
            // so this method will automatically delegate the "set property" to the underlying model instance
            string propertyName = binder.Name;
            Contract.Assume(propertyName != null, "The binder property name should never be null");
            PropertyInfo property = ModelInstance.GetType().GetProperty(propertyName);
            if (property == null || property.CanRead == false)
            {
                result = null;
                return false;
            }//if - no such property on the model

            // Return the value of the underlying model property
            result = property.GetValue(ModelInstance, null);
            return true;
        }


        /// <summary>
        /// This method is called when a property is written that is not actually defined on this class.
        /// (Note that if the property is actually defined on the view-model class then it is called directly and no automatic
        /// delegation to the model occurs).
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation.
        /// The <c>binder.Name</c> property provides the name of the member on which the dynamic operation is performed.
        /// </param>
        /// <param name="value">
        /// The new property value to set.
        /// </param>
        /// <returns>
        /// <c>true</c> if the underlying model instance has a property named <c>binder.Name</c> that is writable;
        /// <c>false</c> if the underlying model instance does NOT have such a property, or it is not writable.
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (ModelInstance == null)
                throw new InvalidOperationException("This view-model instance has already been disposed; it no longer supports 'get' operations");

            // Somebody is trying to set a property named binder.Name on the view-model.
            // No such statically defined property exists (or it would have been called rather than this method)
            // so this method will automatically delegate the "set property" to the underlying model instance
            string propertyName = binder.Name;
            Contract.Assume(string.IsNullOrWhiteSpace(propertyName) == false, "The binder property name should never be ");
            PropertyInfo property = ModelInstance.GetType().GetProperty(propertyName);
            if (property == null || property.CanWrite == false)
                return false;

            // Set the value of the underlying model property
            property.SetValue(ModelInstance, value, null);

            // Execute the common setter functionality on the property
            // This includes raising events and processing other affected properties
            CommonSetterFunctionality(property);

            return true;
        }


        /// <summary>
        /// This method encapsulates all the logic that must be done when a property
        /// on the underlying model is set.
        /// </summary>
        /// <param name="propertyName">The property on the model that was set</param>
        /// <remarks>
        /// This method delegates the hard work to <see cref="CommonSetterFunctionality(PropertyInfo)"/>
        /// </remarks>
        protected void CommonSetterFunctionality(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentOutOfRangeException("propertyName");

            PropertyInfo property = ModelInstance.GetType().GetProperty(propertyName);
            Contract.Assert(property != null, "There is no such property on the underlying model");
            CommonSetterFunctionality(property);
        }


        /// <summary>
        /// This method encapsulates all the logic that must be done when a property
        /// on the underlying model is set.
        /// </summary>
        /// <param name="property">The property on the model that was set</param>
        protected virtual void CommonSetterFunctionality(PropertyInfo property)
        {
            // Raise the property changed event IF AND ONLY IF the underlying model class isn't already doing this
            // (we don't want to raise the property changed event twice for the same property change)
            if (!ModelRaisesPropertyChangedEvents)
            {
                RaisePropertyChanged(property.Name);
            }//if - model doesn't raise property changed events
        }

        #endregion




        protected virtual void Broadcast<TValue>(TValue value)
        {
            var message = new ValueChangedMessage<TValue>(value);
            if (MessengerInstance != null)
            {
                MessengerInstance.Send(message);
            }
            else
            {
                WeakReferenceMessenger.Default.Send(message);
            }
        }

        public virtual void Dispose()
        {
            if (ModelInstance != null && ModelRaisesPropertyChangedEvents)
            {
                var raisesPropChangedEvents = ModelInstance as INotifyPropertyChanged;
                Debug.Assert(raisesPropChangedEvents!=null);
                raisesPropChangedEvents.PropertyChanged -= OnModelPropertyChanged;
            }

            ModelInstance = null;

            //Messenger.Default.Unregister(this);
        }

        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                    if (!_isInDesignMode.Value && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        _isInDesignMode = true;
                    }
                }
                return _isInDesignMode.Value;
            }
        }

        protected IMessenger MessengerInstance { get; private set; }

        /// <summary>
        /// The name to display.
        /// </summary>
        public abstract string DisplayName {get;}  
        }
    }//class

