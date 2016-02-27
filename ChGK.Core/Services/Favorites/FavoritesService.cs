using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using ChGK.Core.Models.Database;
using ChGK.Core.Services.Database;
using ChGK.Core.Services.Messenger;

namespace ChGK.Core.Services.Favorites
{
    internal class FavoritesService : IFavoritesService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMessagesService _messagesService;

        public FavoritesService(IDatabaseService databaseService,
            IMessagesService messagesService)
        {
            _databaseService = databaseService;
            _messagesService = messagesService;

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

            _messagesService.Publish(new IsFavoriteChangedMessage(this, tournament.Id));
        }

        public IEnumerable<ITournament> GetFavoriteTournaments()
        {
            return _databaseService.GetAll<FavoriteTournamentDatabaseModel>()
                .Select(favoriteTournament => new Tournament {Id = favoriteTournament.TournamentId});
        }
    }
}