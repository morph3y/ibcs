using System;
using System.Collections.Generic;
using Entities;

namespace Web.Models.Dto
{
    public static class DtoBuilder
    {
        public static TournamentDto BuildTournament(Tournament tournament)
        {
            var resultDto = new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Status = tournament.Status,
                TournamentType = tournament.TournamentType,
                Stages = new List<TournamentStageDto>()
            };

            foreach (var stage in tournament.Stages)
            {
                resultDto.Stages.Add(BuildStage(stage));
            }

            return resultDto;
        }

        private static TournamentStageDto BuildStage(TournamentStage stage)
        {
            var resultDto = new TournamentStageDto
            {
                Id = stage.Id,
                Name = stage.Name,
                Order = stage.Order,
                Games = new List<GameDto>()
            };

            foreach (var game in stage.Games)
            {
                resultDto.Games.Add(BuildGame(game));
            }

            return resultDto;
        }

        private static GameDto BuildGame(Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Participant1 = BuildSubject(game.Participant1),
                Participant1Score = game.Participant1Score,
                Participant2 = BuildSubject(game.Participant2),
                Participant2Score = game.Participant2Score,
                Status = game.Status,
                Winner = BuildSubject(game.Winner)
            };
        }

        private static SubjectDto BuildSubject(Subject subject)
        {
            SubjectDto resultDto;
            if (subject == null)
            {
                return null;
            }

            if (subject.Self.GetType() == typeof(Player))
            {
                resultDto = BuildPlayer((Player)subject.Self);
            }
            else if (subject.Self.GetType() == typeof (Team))
            {
                resultDto = BuildTeam((Team)subject.Self);
            }
            else
            {
                throw new NotSupportedException("Type of subject is not supported");
            }
            resultDto.Id = subject.Id;
            resultDto.Deleted = subject.Deleted;
            resultDto.Name = subject.Name;

            return resultDto;
        }

        private static TeamDto BuildTeam(Team team)
        {
            var resultDto = new TeamDto
            {
                Captain = (PlayerDto)BuildSubject(team.Captain),
                Members = new List<PlayerDto>()
            };

            foreach (var member in team.Members)
            {
                resultDto.Members.Add((PlayerDto)BuildSubject(member));
            }
            return resultDto;
        }

        private static PlayerDto BuildPlayer(Player player)
        {
            return new PlayerDto
            {
                UserName = player.UserName,
                FirstName = player.FirstName,
                IsAdmin = player.IsAdmin,
                LastName = player.LastName
            };
        }
    }
}