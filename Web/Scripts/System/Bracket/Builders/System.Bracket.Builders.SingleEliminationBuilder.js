registerNamespace("System.Bracket.Builders.SingleEliminationBuilder");

System.Bracket.Builders.SingleEliminationBuilder = function (tournament) {
    var self = this,
        template = $('<div class="se-wrapper"></div>');

    self.tournament = tournament;

    function createGracket(tournament) {
        var result = [],
            roundLabels = [];
        for (var i = 0, il = tournament.stages.length; i < il; i++) {
            var stage = tournament.stages[i],
                stageModel = [],
                sortedGames = [];
            roundLabels.push(stage.name);

            for (var id in stage.games) {
                sortedGames.push([id, stage.games[id]]);
            }
            sortedGames.sort(function(a, b) {
                return a[1].order - b[1].order;
            });

            for (var j = 0, jl = sortedGames.length; j < jl; j++) {
                var index = j,
                    game = sortedGames[index][1],
                    gameModel = [],
                    byeModel = { name: 'BYE', id: -1, seed: 0 },
                    participant1IsBye = game.participant1 == null,
                    participant2IsBye = game.participant2 == null;

                if (participant1IsBye) {
                    gameModel.push(byeModel);
                } else {
                    gameModel.push({
                        name: game.participant1.name == '' ? '(NO NAME)' : game.participant1.name || '(NO NAME)adsf asdf das fdas fdas fdas das fdas f asd',
                        id: game.participant1.id, 
                        seed: participant2IsBye ? 1 : game.participant1Score
                    });
                }

                if (participant2IsBye) {
                    gameModel.push(byeModel);
                } else {
                    gameModel.push({
                        name: game.participant2.name == '' ? '(NO NAME)' : game.participant2.name || '(NO NAME)sdf asdfdas fdasfdas fdasfdasfdasfdasfdasfasdfdas',
                        id: game.participant2.id,
                        seed: participant1IsBye ? 1 : game.participant2Score
                    });
                }
                if (game.order > index) {
                    while (index < game.order) {
                        stageModel.push([{name: 'TBD', id: -2}, {name: 'TBD', id: -2}]);
                        index++;
                    }
                }
                stageModel.push(gameModel);
            }
            result.push(stageModel);
        }
        return {
            src: result,
            roundLabels: roundLabels,
            cornerRadius: 15,
            canvasLineGap: 15
        };
    };

    self.render = function (wrapper) {
        var tournamentModel = createGracket(self.tournament);
        if (tournamentModel.src.length == 0) {
            return;
        }

        wrapper.empty().append(template.empty());

        template.gracket(tournamentModel);
    };

    return self;
};