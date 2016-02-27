using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    public class IsFavoriteChangedMessage : MvxMessage
    {
        public IsFavoriteChangedMessage(object sender, string tournamentId) : base(sender)
        {
            TournamentId = tournamentId;
        }

        public string TournamentId { get; set; }
    }
}