using System;
using System.Collections;
using System.Diagnostics;
using Scch.Common.Reflecton;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Performance"/> catrgory.
    /// </summary>
    public class PerformanceLogEntry : LogEntryBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="PerformanceLogEntry"/>.
        /// </summary>
        /// <param name="start">The start time of the action.</param>
        /// <param name="end">The end time of the action.</param>
        /// <param name="result">The result of the action.</param>
        public PerformanceLogEntry(DateTime start, DateTime end, object result)
            : base("PerformanceLogEntry", string.Format("{0}", (end - start).TotalSeconds), Priorities.Normal, TraceEventType.Information)
        {
            SetFields(result);
        }

        /// <summary>
        /// Creates a new instance of <see cref="PerformanceLogEntry"/>.
        /// </summary>
        /// <param name="start">The start time of the action.</param>
        /// <param name="end">The end time of the action.</param>
        public PerformanceLogEntry(DateTime start, DateTime end)
            : base("PerformanceLogEntry", string.Format("{0}", (end - start).TotalSeconds), Priorities.Normal, TraceEventType.Information)
        {
            SetFields(null);
        }

        void SetFields(object result)
        {
            ManagedThreadName = DebugHelper.GetFullMethodName(3);
            Categories.Add(Category.Performance);

            if (result is ICollection)
                ExtendedProperties.Add("Count", ((ICollection)result).Count);
        }
    }
}
