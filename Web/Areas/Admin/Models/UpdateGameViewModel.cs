namespace Web.Areas.Admin.Models
{
    public class UpdateGameViewModel
    {
        public int GameId { get; set; }
        public int WinnerId { get; set; }
        public int Participant1Score { get; set; }
        public int Participant2Score { get; set; }
    }
}
