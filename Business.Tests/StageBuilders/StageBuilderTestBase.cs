using System.Collections.Generic;
using Entities;

namespace Business.Tests.StageBuilders
{
    internal class StageBuilderTestBase : BusinessTestBase
    {
        protected bool StageHasUniquePlayers(TournamentStage stage)
        {
            var playerHash = new HashSet<Subject>();
            foreach (var game in stage.Games)
            {
                if (game.Participant1 != null && !playerHash.Add(game.Participant1))
                {
                    return false;
                }

                if (game.Participant2 != null && !playerHash.Add(game.Participant2))
                {
                    return false;
                }
            }

            return true;
        }

        protected List<Subject> GeneratePlayers(int count)
        {
            var toReturn = new List<Subject>();
            for (var i = 0; i < count; i++)
            {
                toReturn.Add(new Player { Id = i });
            }
            return toReturn;
        }

        protected bool SubjectInGame(int subjectId, Game game)
        {
            return game.Participant1.Id == subjectId || game.Participant2.Id == subjectId;
        }
    }
}
