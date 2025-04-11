namespace CTFApp.Models
{
    public class UserUpdateDto
    {
        public string Id { get; set; } = null!;

        public string? Username { get; set; }

        public int? userScore { get; set; }

        public string? ImageAva { get; set; }

        public string Role { get; set; }
    }
}
