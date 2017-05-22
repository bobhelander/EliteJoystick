using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    internal sealed class LazyProfile : Profile
    {
        private readonly ICollection<Mapping> mappings;

        public LazyProfile(Model model)
            : base(model)
        {
            this.mappings = new LazyCollection<Mapping>(this.LoadMappings);
        }

        public override ICollection<Mapping> Mappings
        {
            get { return this.mappings; }
        }

        private ICollection<Mapping> LoadMappings()
        {
            return ModelService.RetrieveMappings(this);
        }
    }
}
