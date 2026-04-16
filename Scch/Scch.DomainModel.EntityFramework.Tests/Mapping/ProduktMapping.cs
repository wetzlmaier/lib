namespace Scch.DomainModel.EntityFramework.Tests.Mapping
{
    public class ProduktMapping : EntityWithBezeichnungMapping<Produkt>
    {
        public ProduktMapping()
        {
            Property(x => x.Grundpreis);
            Property(x => x.Nummer);

            HasRequired(x => x.Produktgruppe).WithMany();
        }
    }
}
