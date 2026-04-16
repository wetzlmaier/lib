namespace Scch.DomainModel.EntityFramework.Tests.Mapping
{
    public class AuspraegungMapping : EntityWithBezeichnungMapping<Auspraegung>
    {
        public AuspraegungMapping()
        {
            HasRequired(x => x.Produktmerkmal).WithMany();
        }
    }
}
