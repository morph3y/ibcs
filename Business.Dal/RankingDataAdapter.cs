using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business.Dal;
using Dal.Contracts;
using Entities;
using NHibernate.Criterion;

namespace Business.Dal
{
    internal sealed class RankingDataAdapter : IRankingDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public RankingDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public IEnumerable<Rank> GetRanks(IEnumerable<Subject> subjects)
        {
            // TODO: repeated ids...
            Rank rankAlias = null;
            var subjectIds = subjects.Select(x => x.Id).ToList();
            var ranksQuery = QueryOver.Of(() => rankAlias)
                .Where(Restrictions.In(Projections.Property(() => rankAlias.Subject.Id), subjectIds));

            return _dataAccessAdapter.GetCollection(ranksQuery);
        }

        public Rank GetRank(Subject subject)
        {
            return  _dataAccessAdapter.GetCollection<Rank>(x=>x.Id == subject.Id).FirstOrDefault();
        }

        public void Save(Rank rank)
        {
            _dataAccessAdapter.Save(rank);
        }

        public Rank InitRank(Subject subject)
        {
            return InitRank(new List<Subject> { subject }).First();
        }

        public IEnumerable<Rank> InitRank(IEnumerable<Subject> subjects)
        {
            // TODO: Whaaa?
            var toReturn = new List<Rank>();
            foreach (var subject in subjects)
            {
                var newRank = new Rank
                {
                    DateModified = DateTime.Now,
                    Elo = 2200,
                    LastGame = null,
                    Subject = subject
                };

                toReturn.Add(newRank);
                _dataAccessAdapter.Save(newRank);
            }

            return toReturn;
        }
    }
}
