namespace Scch.Mvvm.Controller
{
    /// <summary>
    /// Responsible for the module lifecycle.
    /// </summary>
    public interface IModuleController
    {
        /// <summary>
        /// Initializes the controller.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Runs the controller.
        /// </summary>
        void Run();

        /// <summary>
        /// Shut down the controller.
        /// </summary>
        void Shutdown();
    }
}
