using System.ComponentModel.DataAnnotations;

namespace CTFApp.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string userName { get; set; }
        public string userScore { get; set; }

    }
}
