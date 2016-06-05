registerNamespace("System.Bracket.Builders.Group.GamesList");

System.Bracket.Builders.Group.GamesList = function(groups) {
    var self = this,
        template = $('' +
            '<div class="group-list-wrapper">' +
            '   <span class="group-list-title"></span>' +
            '   <div class="group-list-container"></div>' +
            '</div>');

    self.render = function (wrapper) {
        template.find('.group-list-title').text('Games:').click(function() {
            template.find('.group-list-container').toggle();
        });
        var groupTemplate = $('' +
                '<div class="group-list-item">' +
                '   <div class="group-list-item-header"></div>' +
                '   <div class="group-list-item-body"></div>' +
                '</div>' +
                '');

        for (var i = 0, il = groups.length; i < il; i++) {
            var newTemplate = groupTemplate.clone(),
                group = groups[i];

            for (var j = 0, jl = group.games.length; j < jl; j++) {
                var game = group.games[j],
                    gameTemplate = $('' +
                        '<div class="game-item-container">' +
                        '   <span class="player1"></span>' +
                        '   <span class="player1-score"></span>' +
                        ' : ' +
                        '   <span class="player2-score"></span>' +
                        '   <span class="player2"></span>' +
                        '</div>' +
                        '');

                if (game.winner != null) {
                    if (game.winner.id == game.participant1.id) {
                        gameTemplate.find('.player1, .player1-score').addClass('winner');
                    } else {
                        gameTemplate.find('.player2, .player2-score').addClass('winner');
                    }
                }

                gameTemplate.find('.player1').text(game.participant1.name);
                gameTemplate.find('.player1-score').text(game.participant1Score);
                gameTemplate.find('.player2').text(game.participant2.name);
                gameTemplate.find('.player2-score').text(game.participant2Score);

                newTemplate.find('.group-list-item-body').append(gameTemplate);
            }
            newTemplate.find('.group-list-item-header').text(group.name);
            template.find('.group-list-container').append(newTemplate);
        }

        wrapper.append(template);
    }
};