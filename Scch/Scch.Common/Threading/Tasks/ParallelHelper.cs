using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scch.Common.Threading.Tasks
{
    public class ParallelHelper
    {
        public static void ExecuteAction<T>(IEnumerable<T> source, Action<T> action, ParallelOptions options = null, bool executeInParallel = true)
        {
            if (executeInParallel)
            {
                if (options == null)
                    options = new ParallelOptions();

                Parallel.ForEach(source, options, action);
            }
            else
            {
                var exceptions = new List<Exception>();

                foreach (var item in source)
                {
                    try
                    {
                        action(item);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Count > 0)
                    throw new AggregateException("One or more exceptions occurred.", exceptions);
            }
        }
    }
}
