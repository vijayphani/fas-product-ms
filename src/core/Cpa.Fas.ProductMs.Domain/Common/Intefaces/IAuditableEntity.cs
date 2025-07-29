namespace Cpa.Fas.ProductMs.Domain.Common.Intefaces
{
    public interface IAuditableEntity : IEntity
    {
        Guid CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime UpdatedAt { get; set; }

        void MarkAsCreated(Guid userId);

        void MarkAsUpdated(Guid userId);
    }
}