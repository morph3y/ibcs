registerNamespace("System.Bracket.Builders.LeagueBuilder");

System.Bracket.Builders.LeagueBuilder = function(tournament) {
    var self = this,
        template = $('' +
            '<div class="league-wrapper">' +
            '   <div class="summary-wrapper">' +
            '   </div>' +
            '   <div class="games-list">' +
            '   </div>' +
            '</div>'),
        stageTemplate = $('' +
            '<div class="stage-container">' +
            '   <div class="header">' +
            '       <span></span>' +
            '   </div>' +
            '   <div class="games">' +
            '   </div>' +
            '</div>'),
        gamesList = template.find('.games-list'),
        summaryWrapper = template.find('.summary-wrapper');

    // ctor
    self.leagueSummary = new System.Bracket.Builders.LeagueSummary(tournament);

    self.render = function(wrapper) {
        self.leagueSummary.render(summaryWrapper);

        // render all games
        for (var i = 0, il = tournament.stages.length; i < il; i++) {
            var templateForStage = stageTemplate.clone();

            var stageBuilder = new System.Bracket.Builders.StageBuilder(tournament.stages[i]);
            stageBuilder.render(templateForStage.find('.games'));

            templateForStage.find('.header span').text(tournament.stages[i].name);
            gamesList.append(templateForStage);
        }

        wrapper.append(template);
    };

    return self;
};