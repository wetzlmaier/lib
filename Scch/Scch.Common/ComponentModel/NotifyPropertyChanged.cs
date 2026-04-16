using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using Scch.Common.Linq.Expressions;

namespace Scch.Common.ComponentModel
{
    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                string propertyName = ExpressionHelper.GetPropertyPath(property);
#if DEBUG
                var stackTrace = new StackTrace();

                bool found = false;
                for (int i = 1; i < stackTrace.FrameCount; i++)
                {
                    if (stackTrace.GetFrame(i).GetMethod().Name != "set_" + propertyName)
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                    throw new InvalidOperationException("Invalid property name.");
#endif
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
