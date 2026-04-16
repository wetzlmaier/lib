namespace Scch.DomainModel.EntityFramework.Tests.Mapping
{
    public class ProduktmerkmalMapping : EntityWithBezeichnungMapping<Produktmerkmal>
    {
        public ProduktmerkmalMapping()
        {
            Property(x => x.Laenge);

            HasRequired(x => x.Merkmalsgruppe);
            HasMany(x => x.Auspraegungen).WithRequired(y => y.Produktmerkmal);
        }
    }
}
