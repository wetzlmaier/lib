using System;

namespace Scch.DomainModel.EntityFramework
{
    public static class ObjectWithChangeTrackerExtensions
    {
        public static T MarkAsDeleted<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (Equals(trackingItem, default(T)))
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;

            if (trackingItem is IDeletable)
                ((IDeletable) trackingItem).IsDeleted = true;
            else
                trackingItem.ChangeTracker.State = ObjectState.Deleted;

            return trackingItem;
        }

        public static T MarkAsAdded<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (Equals(trackingItem, default(T)))
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Added;
            return trackingItem;
        }

        public static T MarkAsModified<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (Equals(trackingItem, default(T)))
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Modified;
            return trackingItem;
        }

        public static T MarkAsUnchanged<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (Equals(trackingItem, default(T)))
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Unchanged;
            return trackingItem;
        }

        public static void StartTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
        }

        public static void StopTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = false;
        }

        public static void AcceptChanges(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.AcceptChanges();
        }
    }
}
