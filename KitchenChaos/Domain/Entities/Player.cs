namespace Domain.Entities
{
    public class Player : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public ICollection<Record> Records { get; set; }
    }
}
