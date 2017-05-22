
namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Ensures that the service are provided as singletons.
    /// </summary>
    public sealed class ServiceFactory
    {
        /// <summary>
        /// The unique instance of the factory.
        /// </summary>
        private static readonly ServiceFactory instance = new ServiceFactory();

        /// <summary>
        /// The unique instance of the Model service.
        /// </summary>
        private ModelService modelService;

        /// <summary>
        /// Prevents an instance of the <see cref="ServiceFactory"/> from being created.
        /// </summary>
        private ServiceFactory()
        { }

        /// <summary>
        /// Gets the unique instance of the factory.
        /// </summary>
        public static ServiceFactory Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the unique instance of the Model service.
        /// </summary>
        public ModelService ModelService
        {
            get
            {
                if (this.modelService == null)
                {
                    this.modelService = new ModelService();
                }

                return this.modelService;
            }
        }
    }
}
