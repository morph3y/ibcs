using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Contracts.Business.Dal;
using Entities;

namespace Business.Ranking
{
    internal sealed class RankingService : IRankingService
    {
        public const int StartingElo = 2200;
        public const int PunishElo = 20;
        public const int AllowedMonths = 1;

        private readonly IRankingDataAdapter _rankingDataAdapter;
        private readonly IRankingProvider _rankingProvider;
        public RankingService(IRankingDataAdapter rankingDataAdapter, IRankingProvider rankingProvider = null)
        {
            _rankingDataAdapter = rankingDataAdapter;
            _rankingProvider = rankingProvider ?? new GlobalRankingProvider(rankingDataAdapter);
        }

        public void MaintainRanks()
        {
            var ranksToPunish = _rankingDataAdapter.GetRanksToPunish(AllowedMonths, StartingElo);
            foreach (var rank in ranksToPunish)
            {
                // remove elo to punish but not less than starting
                rank.Elo -= PunishElo;
                if (rank.Elo < StartingElo)
                {
                    rank.Elo = StartingElo;
                }

                Save(rank);
            }
        }

        public void Save(Rank rank)
        {
            rank.DateModified = DateTime.Now;
            _rankingDataAdapter.Save(rank);
        }

        public IEnumerable<Rank> Get<T>(int? limit = null) where T : Subject
        {
            return _rankingDataAdapter.GetRanks<T>(limit);
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            var rankedSubjects = _rankingProvider.Rank(subjects);

            // Initialize if needed
            if (rankedSubjects.Count() != subjects.Count())
            {
                var toInitialize = subjects.Where(x => !rankedSubjects.Contains(x));
                var newRanks = InitRank(toInitialize);

                var allRanks = new List<Subject>();
                allRanks.AddRange(newRanks.Select(x=>x.Subject));
                allRanks.AddRange(rankedSubjects);
                rankedSubjects = allRanks;
            }

            return rankedSubjects;
        }

        public Rank InitRank(Subject subject)
        {
            return InitRank(new List<Subject> { subject }).First();
        }

        public IEnumerable<Rank> InitRank(IEnumerable<Subject> subjects)
        {
            // TODO: WTF?
            var toReturn = new List<Rank>();
            foreach (var subject in subjects)
            {
                var newRank = new Rank
                {
                    Elo = StartingElo,
                    LastGame = null,
                    Subject = subject
                };

                toReturn.Add(newRank);
                Save(newRank);
            }

            return toReturn;
        }

        public void UpdateRank(Subject winner, Subject player2)
        {
            var currentRanks = _rankingDataAdapter.GetRanks(new List<Subject> { winner, player2 });

            var player1Rank = currentRanks.FirstOrDefault(x => x.Subject.Id == winner.Id);
            var player2Rank = currentRanks.FirstOrDefault(x => x.Subject.Id == player2.Id);

            if (player1Rank == null)
            {
                player1Rank = InitRank(winner);
            }

            if (player2Rank == null)
            {
                player2Rank = InitRank(player2);
            }

            var player1CurrentRank = (double)player1Rank.Elo;
            var player2CurrentRank = (double)player2Rank.Elo;

            var e = 30 - Math.Round(1 / (1 + Math.Pow(10, ((player2CurrentRank - player1CurrentRank) / 90))) * 30);
            player1Rank.Elo = (int)(player1CurrentRank + e);
            player2Rank.Elo = (int)(player2CurrentRank - e);

            Save(player1Rank);
            Save(player2Rank);

            //MaintainRanks();
        }
    }
}
