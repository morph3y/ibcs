registerNamespace("System.Bracket.Builders.Group.GroupItem");

System.Bracket.Builders.Group.GroupItem = function (group, options) {
    var self = this,
        template = $('' +
            '<div class="group-item-wrapper">' +
            '   <div class="group-item-title"></div>' +
            '   <div class="group-item-table"></div>' +
            '</div>');

    self.options = $.extend({}, {
        pointsForWin: 3,
        pointsForLose: 0,
        pointsForTie: 1
    }, options);

    function isContestantQualified(group, contestantId) {
        for (var p = 0, pl = group.qualifiedContestants.length; p < pl; p++) {
            if (group.qualifiedContestants[p].contestant.id == contestantId) {
                return true;
            }
        }
        return false;
    }

    function getPlayerToGamesMap(group) {
        var result = {};

        for (var i = 0, il = group.contestants.length; i < il; i++) {
            result[group.contestants[i].id] = [];
            for (var j = 0, jl = group.games.length; j < jl; j++) {
                if ((group.games[j].participant1 != null && group.games[j].participant1.id == group.contestants[i].id)
                    ||
                    (group.games[j].participant2 != null && group.games[j].participant2.id == group.contestants[i].id)) {
                    result[group.contestants[i].id].push(group.games[j]);
                }
            }
        }

        return result;
    }

    function buildHeader(table, contestants) {
        table.find('thead').append('' +
            '<tr>' +
            '   <th>Player</th>' +
            '</tr>');

        for (var i = 0, il = contestants.length; i < il; i++) {
            table.find('thead tr').append('<th>' + (contestants[i].name == null ? 'N\\A' : contestants[i].name[0].toUpperCase()) + '</th>');
        }

        table.find('thead tr').append('<th>Summary (W L T)</th><th>Points</th>');
    }

    function buildBody(table, gamesMap, contestants) {
        var body = table.find('tbody'),
            generateGameCells = function(mainPlayerId) {
                var result = "";

                for (var k = 0, kl = contestants.length; k < kl; k++) {
                    var oposingPlayer = contestants[k];
                    if (oposingPlayer.id == mainPlayerId) {
                        result += "<td class='self-game'>X</td>";
                        continue;
                    }
                    var contestantGames = gamesMap[mainPlayerId];
                    var gameToRender = null;

                    for (var l = 0, ll = contestantGames.length; l < ll; l++) {
                        if (contestantGames[l].participant1 == null || contestantGames[l].participant1.id == oposingPlayer.id) {
                            result += "<td class='game-result'>" + contestantGames[l].participant2Score + " : " + contestantGames[l].participant1Score + "</td>";
                            continue;
                        }

                        if (contestantGames[l].participant2 == null || contestantGames[l].participant2.id == oposingPlayer.id) {
                            result += "<td class='game-result'>" + contestantGames[l].participant1Score + " : " + contestantGames[l].participant2Score + "</td>";
                            continue;
                        }
                    }
                }
                return result;
            };

        for (var i = 0, il = group.contestants.length; i < il; i++) {
            var wins = 0,
                losses = 0,
                ties = 0,
                contestant = group.contestants[i];
            for (var j = 0, jl = group.games.length; j < jl; j++) {
                var game = group.games[j];
                if (game.winner != null && ((game.participant1 != null && game.participant1.id == contestant.id) || (game.participant2 != null && game.participant2.id == contestant.id))) {
                    if (game.winner.id == contestant.id) {
                        wins += 1;
                    } else {
                        losses += 1;
                    }
                }
            }

            body.append($('<tr>' +
                '<td>' + 
                (isContestantQualified(group, group.contestants[i].id) 
                    ? '<b>'+group.contestants[i].name+'</b>' 
                    : group.contestants[i].name) + '</td>' +
                generateGameCells(group.contestants[i].id) +
                '<td>' + wins + ' - ' + losses + ' - ' + ties + '</td>' +
                '<td>' + wins * self.options.pointsForWin + '</td>' +
                '</tr>'));
        }
    }

    self.render = function (wrapper) {
        var self = this;
        template.find('.group-item-title').text(group.name);

        var table = $('' +
            '<table class="fancy">' +
            '   <thead></thead>' +
            '   <tbody></tbody>' +
            '   <tfoot></tfoot>' +
            '</table>');

        buildHeader(table, group.contestants);
        buildBody(table, getPlayerToGamesMap(group), group.contestants);

        template.find('.group-item-table').append(table);
        wrapper.append(template);
    };

    return self;
};