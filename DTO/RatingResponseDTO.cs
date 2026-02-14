namespace SistemaDeEventos.DTOs.Rating
{
    public class RatingCreateRequestDTO
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class RatingResponseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
