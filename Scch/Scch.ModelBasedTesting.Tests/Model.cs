using System.Collections.Generic;

namespace Scch.ModelBasedTesting.Tests
{
    internal static class Model
    {
        private static readonly IList<string> _calledActions;

        static Model()
        {
            _calledActions = new List<string>();
        }

        [Action]
        public static void Action1()
        {
            _calledActions.Add("Action1");
        }

        [DebugAction]
        public static void DebugAction()
        {
            _calledActions.Add("DebugAction");
        }

        public static bool Action1Enabled()
        {
            return !_calledActions.Contains("Action1");
        }

        [Action]
        public static void Action2()
        {
            _calledActions.Add("Action2");
        }

        public static bool Action2Enabled()
        {
            return !_calledActions.Contains("Action2");
        }

        public static object State => string.Join(",", _calledActions);

        public static void Reset()
        {
            _calledActions.Clear();
        }
    }
}
