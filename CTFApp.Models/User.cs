using Microsoft.AspNetCore.Identity;

namespace CTFApp.Models
{
    public class User : IdentityUser
    {
        public int userScore { get; set; }

        public string ImageAva { get; set; }
    }
}
