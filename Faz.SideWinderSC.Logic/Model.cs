using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents a model of controllers.
    /// </summary>
    public class Model
    {
        /// <summary>
        /// The associated profiles.
        /// </summary>
        private readonly ICollection<Profile> profiles;

        /// <summary>
        /// The default - and empty - mappings associated with the model.
        /// </summary>
        private readonly ICollection<Mapping> defaultMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        public Model()
        {
            this.profiles = new LinkedList<Profile>();
            this.defaultMappings = new LinkedList<Mapping>();
        }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the associated profiles.
        /// </summary>
        public virtual ICollection<Profile> Profiles
        {
            get { return this.profiles; }
        }

        /// <summary>
        /// Gets the default - and empty - mappings associated with the model.
        /// </summary>
        /// <remarks>
        /// Can be used to set up a new profile or to be sure that a profile has a valid configuration.
        /// </remarks>
        public virtual ICollection<Mapping> DefaultMappings
        {
            get { return this.defaultMappings; }
        }
    }
}
