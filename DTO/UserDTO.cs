namespace SistemaDeEventos.DTOs.User
{
    public class UserCreateRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UserUpdateRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
