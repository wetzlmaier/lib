namespace Scch.DomainModel.EntityFramework.Mapping
{
    public class AuditLogMapping : EntityFrameworkEntityMappingBase<long, AuditLog>
    {
        public AuditLogMapping()
        {
            Property(x => x.UserId);
            Property(x => x.EventDateUtc);
            Property(x => x.EventType);
            Property(x => x.TableName);
            Property(x => x.RecordId);
            Property(x => x.ColumnName);
            Property(x => x.OriginalValue);
            Property(x => x.NewValue);
        }
    }
}
