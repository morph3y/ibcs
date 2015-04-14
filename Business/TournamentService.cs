using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Entities;

namespace Business
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly IObjectService _objectService;
        public TournamentService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public IEnumerable<Tournament> GetList()
        {
            return _objectService.Get<Tournament>(x => x.Status != TournamentStatus.Closed);
        }

        public Tournament Get(int id)
        {
            return _objectService.Get<Tournament>(x => x.Id == id).FirstOrDefault();
        }
    }
}
