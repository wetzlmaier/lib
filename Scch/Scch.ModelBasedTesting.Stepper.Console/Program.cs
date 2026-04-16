using System;
using System.Linq;
using Scch.Common.Configuration;
using Scch.Common.Data;

namespace Scch.ModelBasedTesting.Stepper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var wrapper = new ModelWrapper(ConfigurationHelper.Current.ReadString("ModelType"), ConfigurationHelper.Current.ReadBool("Debug"));

            System.Console.WriteLine("Model Stepper");
            System.Console.WriteLine("Using model type: {0}", wrapper.ModelType.AssemblyQualifiedName);

            string action;

            do
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Current state: {0}", wrapper.State);
                System.Console.WriteLine();
                System.Console.WriteLine("Choose one of the following actions:");
                System.Console.WriteLine();

                var enabledActions = wrapper.EnabledActions.Keys.ToList();
                enabledActions.Sort();

                for (int index = 1; index <= enabledActions.Count; index++)
                {
                    System.Console.WriteLine("{0} = {1}", index, enabledActions[index - 1]);
                }

                System.Console.WriteLine();
                System.Console.Write("Enter number or name: ");
                action = System.Console.ReadLine();

                if (action == null)
                    continue;

                int selectedIndex;
                if (int.TryParse(action, out selectedIndex))
                {
                    if (selectedIndex <= 0 || selectedIndex > enabledActions.Count)
                    {
                        System.Console.WriteLine("Invalid index '{0}'.", selectedIndex);
                        continue;
                    }

                    action = enabledActions[selectedIndex - 1];
                }

                if (!enabledActions.Contains(action))
                {
                    System.Console.WriteLine("Unknown action '{0}'.", action);
                    continue;
                }

                var combinations = wrapper.Actions[action];

                int selectedCombination = 1;
                if (combinations.Length > 1)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("Choose one of the following parameter combinations: ");
                    System.Console.WriteLine();

                    var dt = wrapper.CreateDataTable(action);
                    var indexColumn = dt.Columns.Add("Index", typeof(int));
                    indexColumn.SetOrdinal(0);

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        dt.Rows[row]["Index"] = row + 1;
                    }

                    System.Console.WriteLine(DataTableHelper.GenerateText(dt));

                    System.Console.Write("Enter index: ");
                    string selection = System.Console.ReadLine();

                    if (selection == null || !int.TryParse(selection, out selectedCombination))
                    {
                        System.Console.WriteLine("Invalid number.");
                        continue;
                    }

                    if (selectedCombination < 1 || selectedCombination > combinations.Length)
                    {
                        System.Console.WriteLine("Index '{0}' out of range.", selectedCombination);
                        continue;
                    }
                }

                try
                {
                    wrapper.PerformAction(action, combinations[selectedCombination - 1]);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}.", ex.Message);
                    throw;
                }

            } while (action != null && action.Trim() != string.Empty);
        }
    }
}
