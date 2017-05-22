using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    internal sealed class LazyModel : Model
    {
        private ICollection<Profile> profiles;

        public LazyModel()
        {
            this.profiles = new LazyCollection<Profile>(this.LoadProfiles);
        }

        public override ICollection<Profile> Profiles
        {
            get { return this.profiles; }
        }

        /// <summary>
        /// Loads the profiles associated with the model.
        /// </summary>
        /// <returns>The associated profiles.</returns>
        private ICollection<Profile> LoadProfiles()
        {
            return ModelService.RetrieveProfiles(this);
        }
    }
}
