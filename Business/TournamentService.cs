using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Entities;

namespace Business
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly IObjectService _objectService;
        private readonly ITournamentStageService _stageService;

        public TournamentService(IObjectService objectService, ITournamentStageService stageService)
        {
            _objectService = objectService;
            _stageService = stageService;
        }

        public IEnumerable<Tournament> GetList()
        {
            return _objectService.Get<Tournament>(x => x.Status != TournamentStatus.Closed);
        }

        public Tournament Get(int id)
        {
            return _objectService.Get<Tournament>(x => x.Id == id).FirstOrDefault();
        }

        public void Save(Tournament tournament)
        {
            _objectService.Save(tournament);
        }

        public void Create(Tournament entity)
        {
            entity.Status = TournamentStatus.Registration;
            entity.Stages.Add(_stageService.CreateStages(entity));
        }

        public void AddContestant(Subject contestant, Tournament tournament)
        {
            // if tournament is new add a stage first

            tournament.Contestants.Add(contestant);
            _stageService.GenerateGames(tournament);
        }
    }
}
