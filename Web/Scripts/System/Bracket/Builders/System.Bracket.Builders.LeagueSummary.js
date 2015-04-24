registerNamespace("System.Bracket.Builders.LeagueSummary");

System.Bracket.Builders.LeagueSummary = function (tournament) {
    var self = this,
        playerMap = {},
        playerInfo = {
            player: null,
            wins: 0,
            losses: 0,
            ties: 0,
            points: 0,
            played: 0,
            leftToPlay: 0
        },
        template = $('' +
            '<table class="summary">' +
            '   <thead>' +
            '       <tr>' +
            '           <th>Rank</th>' +
            '           <th class="participant-name">Name</th>' +
            '           <th>Games</th>' +
            '           <th>Wins</th>' +
            '           <th>Losses</th>' +
            '           <th>Ties</th>' +
            '           <th>Points</th>' +
            '       </tr>' +
            '   </thead>' +
            '   <tbody>' +
            '   </tbody>' +
            '</table>');

    // ctor
    for (var k = 0, kl = tournament.stages.length; k < kl; k++) {
        var stage = tournament.stages[k];
        for (var l = 0, ll = stage.games.length; l < ll; l++) {
            var game = stage.games[l];
            if (playerMap[game.participant1.id] == null) {
                playerMap[game.participant1.id] = $.extend({}, playerInfo);
                playerMap[game.participant1.id].player = game.participant1;
            }

            if (playerMap[game.participant2.id] == null) {
                playerMap[game.participant2.id] = $.extend({}, playerInfo);
                playerMap[game.participant2.id].player = game.participant2;
            }

            if (game.status != 3) {
                playerMap[game.participant2.id].leftToPlay += 1;
                playerMap[game.participant1.id].leftToPlay += 1;
                continue;
            }

            var tie = game.winner.id == null;
            if (tie) {
                playerMap[game.participant2.id].ties += 1;
                playerMap[game.participant1.id].ties += 1;

                playerMap[game.participant2.id].points += tournament.pointForTie;
                playerMap[game.participant1.id].points += tournament.pointForTie;
            } else {
                var loserId = game.participant1.id == game.winner.id ? game.participant2.id : game.participant1.id;
                playerMap[game.winner.id].wins += 1;
                playerMap[loserId].losses += 1;

                playerMap[game.winner.id].points += tournament.pointsForWin;
            }

            playerMap[game.participant2.id].played += 1;
            playerMap[game.participant1.id].played += 1;
        }
    }

    self.render = function (wrapper) {
        // sorting by points
        var tBody = template.find('tbody');

        var sorted = [];
        for (var id in playerMap) {
            sorted.push([id, playerMap[id]])
        }
        sorted.sort(function (a, b) {
            return b[1].points - a[1].points;
        })

        for (var u = 0, ul = sorted.length; u < ul; u++) {
            var rank = u + 1;
            tBody.append($('' +
                '<tr>' +
                '   <td>' + rank + '</td>' +
                '   <td class="participant-name">' + sorted[u][1].player.name + '</td>' +
                '   <td>' + sorted[u][1].played + '</td>' +
                '   <td>' + sorted[u][1].wins + '</td>' +
                '   <td>' + sorted[u][1].losses + '</td>' +
                '   <td>' + sorted[u][1].ties + '</td>' +
                '   <td>' + sorted[u][1].points + '</td>' +
                '</tr>'));
        }
        wrapper.append(template);
    };

    return self;
};