using System;

namespace Scch.ModelBasedTesting
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : Attribute
    {
        public ActionAttribute()
        {
        }

        public ActionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string Start { get; set; }

        public string Finish { get; set; }

        public int Weight { get; set; }
    }
}
