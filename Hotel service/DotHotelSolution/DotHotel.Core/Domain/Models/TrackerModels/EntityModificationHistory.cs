namespace DotHotel.Core.Domain.Models.TrackerModels
{
    public class EntityModificationHistory
    {
        public Guid Id { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Action { get; set; }
        public string? ChangedByUserId { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? ChangesJson { get; set; }
    }
}
