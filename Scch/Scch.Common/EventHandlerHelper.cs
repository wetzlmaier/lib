using System;
using System.Linq;
using System.Reflection;

namespace Scch.Common
{
    /// <summary>
    /// Helper class for event handlers.
    /// </summary>
    public static class EventHandlerHelper
    {
        /// <summary>
        /// Returns true, if a delegate is already attached to the static event handler for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="eventName">The name of the event handler.</param>
        /// <param name="handler">The delegate to check if it is already attached.</param>
        /// <returns>True, if a delegate is already attached to the event handler for the specified target.</returns>
        public static bool IsEventHandlerRegistered(Type type, string eventName, Delegate handler)
        {
            return IsEventHandlerRegistered(null, type, eventName, handler, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
        }

        /// <summary>
        /// Returns true, if a delegate is already attached to the event handler for the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="eventName">The name of the event handler.</param>
        /// <param name="handler">The delegate to check if it is already attached.</param>
        /// <returns>True, if a delegate is already attached to the event handler for the specified target.</returns>
        public static bool IsEventHandlerRegistered(object target, string eventName, Delegate handler)
        {
            return IsEventHandlerRegistered(target, target.GetType(), eventName, handler, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        }

        private static bool IsEventHandlerRegistered(object target, Type type, string eventName, Delegate handler, BindingFlags bindingFlags)
        {
            var e = type.GetEvent(eventName, bindingFlags);

            FieldInfo f = null;
            while (type != null && type != typeof(object))
            {
                f = type.GetField(eventName, bindingFlags);
                if (f != null)
                    break;

                type = type.BaseType;
            }

            if (e != null && f != null)
            {
                var d = (Delegate)f.GetValue(target);
                return d != null && d.GetInvocationList().Any(h => h.Method == handler.Method && h.Target == handler.Target);
            }

            return false;
        }
    }
}
