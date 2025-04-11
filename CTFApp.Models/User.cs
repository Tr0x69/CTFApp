
using System.ComponentModel.DataAnnotations;

namespace CTFApp.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public int userScore { get; set; }

        public string? ImageAva { get; set; }

        public string Role { get; set; }
    }
}
