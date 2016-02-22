using System.Linq;
using ChGK.Core.Models;
using ChGK.Core.Models.Database;
using ChGK.Core.Services.Database;

namespace ChGK.Core.Services.Favorites
{
    internal class FavoritesService : IFavoritesService
    {
        private readonly IDatabaseService _databaseService;

        public FavoritesService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            _databaseService.CreateTable<FavoriteTournamentDatabaseModel>();
        }

        public bool IsTournamentFavorite(ITournament tournament)
        {
            var isFavorite = _databaseService.GetAll<FavoriteTournamentDatabaseModel>()
                .Any(trnmnt => trnmnt.TournamentId.Equals(tournament.Id));

            return isFavorite;
        }

        public void SetTournamentFavourite(ITournament tournament, bool isFavorite)
        {
            foreach (var favoriteTournament in _databaseService.GetAll<FavoriteTournamentDatabaseModel>()
                .Where(trnmnt => trnmnt.TournamentId.Equals(tournament.Id)))
                _databaseService.Delete(favoriteTournament);

            if (isFavorite)
                _databaseService.Insert(new FavoriteTournamentDatabaseModel {TournamentId = tournament.Id});
        }
    }
}