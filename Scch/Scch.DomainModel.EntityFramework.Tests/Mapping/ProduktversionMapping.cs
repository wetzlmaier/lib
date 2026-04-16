using Scch.DomainModel.EntityFramework.Mapping;

namespace Scch.DomainModel.EntityFramework.Tests.Mapping
{
    public class ProduktversionMapping : EntityFrameworkEntityMappingBase<long, Produktversion>
    {
        public ProduktversionMapping()
        {
            Property(x => x.Nummer);
            Property(x => x.Kommentar);

            HasRequired(x => x.Produkt);
            HasMany(x => x.Auspraegungen).WithMany();
        }
    }
}
