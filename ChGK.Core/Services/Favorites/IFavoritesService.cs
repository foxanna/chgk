using ChGK.Core.Models;

namespace ChGK.Core.Services.Favorites
{
    public interface IFavoritesService
    {
        bool IsTournamentFavorite(ITournament tournament);
        void SetTournamentFavourite(ITournament tournament, bool isFavorite);
    }
}