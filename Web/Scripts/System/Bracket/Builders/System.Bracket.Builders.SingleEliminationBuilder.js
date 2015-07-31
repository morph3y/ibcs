registerNamespace("System.Bracket.Builders.SingleEliminationBuilder");

System.Bracket.Builders.SingleEliminationBuilder = function (tournament) {
    var self = this
    template = $('<div class="se-wrapper"></div>');

    // ctor
    self.tournament = tournament;

    // private methods
    function createModel(tournament) {
        var resultModel = [],
            contestantModel = [];
        for (var i = 0, il = tournament.stages.length; i < il; i++) {
            var stage = tournament.stages[i];
            var stageModel = [];

            var sortedGames = [];
            for (var id in stage.games) {
                sortedGames.push([id, stage.games[id]]);
            }
            sortedGames.sort(function (a, b) {
                return a[1].order - b[1].order;
            });

            for (var j = 0, jl = sortedGames.length; j < jl; j++) {
                var index = j,
                    game = sortedGames[index][1],
                    gameModel = [],
                    byeModel = { name: 'BYE', id: -1 };

                if (game.participant1 == null) {
                    game.participant1 = byeModel;
                }

                if (game.participant2 == null) {
                    game.participant2 = byeModel;
                }

                if (i == 0) {
                    contestantModel.push([
                        game.participant1.name == '' ? '(NO NAME)' : game.participant1.name || '(NO NAME)',
                        game.participant2.name == '' ? '(NO NAME)' : game.participant2.name || '(NO NAME)']);
                }

                // WORKAROUND: PLUGIN DOESN'T SUPPORT WINNER BEING PERSON WITH LESS POINTS
                if (game.winner != null &&
                    ((game.participant1Score > game.participant2Score && game.winner.id != game.participant1.id)
                     ||
                     (game.participant1Score < game.participant2Score && game.winner.id != game.participant2.id)
                     || (game.participant1Score == game.participant2Score))) {
                    game.participant1Score = game.winner.id == game.participant1.id ? 1 : 0;
                    game.participant2Score = game.winner.id == game.participant2.id ? 1 : 0;
                }

                gameModel.push(game.participant1Score);
                gameModel.push(game.participant2Score);

                if (game.order > index) {
                    while (index < game.order) {
                        stageModel.push([]);
                        index++;
                    }
                }
                stageModel.push(gameModel);
            }
            resultModel.push(stageModel);
        }

        return {
            "teams": contestantModel,
            "results": resultModel
        };
    };

    self.render = function (wrapper) {
        var tournamentModel = createModel(self.tournament);
        if (tournamentModel.teams.length == 0) {
            return;
        }

        wrapper.empty().append(template.empty());

        template.bracket({
            init: tournamentModel
        });
    };

    return self;
};