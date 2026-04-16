using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Scch.Common.ComponentModel;

namespace Scch.DomainModel
{
    /// <summary>
    /// Interface for entities.
    /// </summary>
    public interface IEntity<TKey> : IExtendedDataErrorInfo, INotifyPropertyChanged, IComparable<IEntity<TKey>> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// The key.
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// True, if the entity has not been saved.
        /// </summary>
        bool IsTransient { get; }
/*
        /// <summary>
        /// The method is called during deserialization of an object.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserializing]
        void OnDeserializingMethod(StreamingContext context);

        /// <summary>
        /// The method is called immediately after deserialization of the object.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context);

        /// <summary>
        /// The method is called before serialization of an object.
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        void OnSerializingMethod(StreamingContext context);

        /// <summary>
        /// The method is called after serialization of an object graph.
        /// </summary>
        /// <param name="context"></param>
        [OnSerialized]
        void OnSerializedMethod(StreamingContext context);*/
    }
}
