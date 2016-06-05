registerNamespace("System.Bracket.Builders.Group.Builder");

System.Bracket.Builders.Group.Builder = function(tournament) {
    var self = this,
        template = $('' +
            '<div class="group-wrapper">' +
            '   <div class="groups-container"></div>' +
            '   <div class="groups-summary"></div>' +
            '</div>');

    self.render = function (wrapper) {
        if (tournament.stages.length != 1) {
            return;
        }

        template.find('.groups-container').empty();
        for (var i = 0, il = tournament.stages[0].groups.length; i < il; i++) {
            var groupItemBuilder = new System.Bracket.Builders.Group.GroupItem(tournament.stages[0].groups[i], {
                pointsForWin: tournament.pointsForWin,
                pointsForTie: tournament.pointsForTie
            });

            groupItemBuilder.render(template.find('.groups-container'));
        }

        var gamesListBuilder = new System.Bracket.Builders.Group.GamesList(tournament.stages[0].groups);
        gamesListBuilder.render(template.find('.groups-summary').empty())

        wrapper.append(template);
    };

    return self;
};