namespace CTFApp.ViewModels
{
    public class GameViewModel
    {
        public IEnumerable<UserScoreViewModel> Users { get; set; }
        public UserScoreViewModel CurrentUser { get; set; }
    }

    public class UserScoreViewModel
    {
        public string Username { get; set; }
        public int userScore { get; set; }

    }
}