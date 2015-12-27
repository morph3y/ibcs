using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Contracts.Business.Dal;
using Entities;

namespace Business
{
    internal sealed class RankingService : IRankingService
    {
        private readonly IRankingDataAdapter _rankingDataAdapter;
        public RankingService(IRankingDataAdapter rankingDataAdapter)
        {
            _rankingDataAdapter = rankingDataAdapter;
        }

        public IEnumerable<Rank> Get(int? limit = null)
        {
            return _rankingDataAdapter.GetRanks(limit);
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            var rankInfo = _rankingDataAdapter.GetRanks(subjects);

            // Initialize if needed
            if (rankInfo.Count() != subjects.Count())
            {
                var toInitialize = subjects.Where(x => !rankInfo.Select(y => y.Subject).Contains(x));
                var newRanks = _rankingDataAdapter.InitRank(toInitialize);

                var allRanks = new List<Rank>();
                allRanks.AddRange(newRanks);
                allRanks.AddRange(rankInfo);
                rankInfo = allRanks;
            }

            return rankInfo.OrderByDescending(x => x.Elo).Select(x => x.Subject);
        }

        public void UpdateRank(Subject winner, Subject player2)
        {
            var currentRanks = _rankingDataAdapter.GetRanks(new List<Subject> { winner, player2 });

            var player1Rank = currentRanks.FirstOrDefault(x => x.Subject.Id == winner.Id);
            var player2Rank = currentRanks.FirstOrDefault(x => x.Subject.Id == player2.Id);

            if (player1Rank == null)
            {
                player1Rank = _rankingDataAdapter.InitRank(winner);
            }

            if (player2Rank == null)
            {
                player2Rank = _rankingDataAdapter.InitRank(player2);
            }

            var player1CurrentRank = (double)player1Rank.Elo;
            var player2CurrentRank = (double)player2Rank.Elo;

            /*if (player1Score != player2Score)
            {*/
                /*if (player1Score > player2Score)
                {*/
                    var e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((player2CurrentRank - player1CurrentRank) / 90))) * 120);
                    player1Rank.Elo = (int)(player1CurrentRank + e);
                    player2Rank.Elo = (int)(player2CurrentRank - e);
                /*}
                else
                {
                    e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((player1CurrentRank - player2CurrentRank) / 90))) * 120);
                    player1Rank.Elo = (int)(player1CurrentRank - e);
                    player2Rank.Elo = (int)(player2CurrentRank + e);
                }*/
            /*}
            else
            {
                if ((int)player1CurrentRank == (int)player2CurrentRank)
                {
                    player1Rank.Elo = (int)(player1CurrentRank);
                    player2Rank.Elo = (int)(player2CurrentRank);
                }
                else
                {
                    if (player1CurrentRank > player2CurrentRank)
                    {
                        e = (120 - Math.Round(1 / (1 + Math.Pow(10, ((player1CurrentRank - player2CurrentRank) / 90))) * 120)) - (120 - Math.Round(1 / (1 + Math.Pow(10, ((player2CurrentRank - player1CurrentRank) / 90))) * 120));
                        player1Rank.Elo = (int)(player1CurrentRank - e);
                        player2Rank.Elo = (int)(player2CurrentRank + e);
                    }
                    else
                    {
                        e = (120 - Math.Round(1 / (1 + Math.Pow(10, ((player2CurrentRank - player1CurrentRank) / 90))) * 120)) - (120 - Math.Round(1 / (1 + Math.Pow(10, ((player1CurrentRank - player2CurrentRank) / 90))) * 120));
                        player1Rank.Elo = (int)(player1CurrentRank + e);
                        player2Rank.Elo = (int)(player2CurrentRank - e);
                    }
                }
            }*/

            _rankingDataAdapter.Save(player1Rank);
            _rankingDataAdapter.Save(player2Rank);
        }
    }
}
