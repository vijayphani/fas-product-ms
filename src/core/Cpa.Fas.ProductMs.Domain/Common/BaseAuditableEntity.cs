using Cpa.Fas.ProductMs.Domain.Common.Intefaces;

namespace Cpa.Fas.ProductMs.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void MarkAsCreated(Guid userId)
        {
            CreatedBy = UpdatedBy = userId;
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsUpdated(Guid userId)
        {
            UpdatedBy = userId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}