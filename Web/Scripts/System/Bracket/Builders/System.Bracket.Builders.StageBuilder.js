registerNamespace("System.Bracket.Builders.StageBuilder");

System.Bracket.Builders.StageBuilder = function (stage) {
    var self = this,
        template = $('<div class="games-wrapper">'),
        playerInGameTemplate = $('' +
            '<span class="player-tag"></span>' +
            '<span class="player-score"></span>'),
        gameTemplate = $('' +
            '<div class="game-wrapper">' +
            '   <div class="player1-wrapper">' +
            '   </div>' +
            '   <div class="player2-wrapper">' +
            '   </div>' +
            '</div>');

    // ctor
    gameTemplate.find('.player1-wrapper').append(playerInGameTemplate.clone());
    gameTemplate.find('.player2-wrapper').append(playerInGameTemplate.clone());

    self.render = function (wrapper) {
        for (var j = 0, jl = stage.games.length; j < jl; j++) {
            var game = stage.games[j],
                currentTemplate = gameTemplate.clone();

            currentTemplate.find('.player1-wrapper .player-tag').text(game.participant1.name);
            currentTemplate.find('.player1-wrapper .player-score').text(game.participant1Score);

            currentTemplate.find('.player2-wrapper .player-tag').text(game.participant2.name);
            currentTemplate.find('.player2-wrapper .player-score').text(game.participant2Score);
            template.append(currentTemplate);
        }
        wrapper.append(template);
    };

    return self;
};