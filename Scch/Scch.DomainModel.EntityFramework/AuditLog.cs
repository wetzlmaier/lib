using System;
using System.ComponentModel.DataAnnotations;

namespace Scch.DomainModel.EntityFramework
{
    public class AuditLog : EntityFrameworkEntity<long> 
    {
        public override long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        public DateTime EventDateUtc { get; set; }

        [Required]
        [MaxLength(1)]
        public string EventType { get; set; }

        [Required]
        [MaxLength(50)]
        public string TableName { get; set; }

        [Required]
        [MaxLength(100)]
        public string RecordId { get; set; }

        [MaxLength(50)]
        public string ColumnName { get; set; }

        [MaxLength(100)]
        public string OriginalValue { get; set; }

        [MaxLength(100)]
        public string NewValue { get; set; }
    }
}
