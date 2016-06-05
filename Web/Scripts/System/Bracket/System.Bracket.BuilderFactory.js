registerNamespace("System.Bracket.BuilderFactory");

System.Bracket.BuilderFactory = (function () {
    this.get = function(tournament) {
        if (tournament.type == 1) {
            return new System.Bracket.Builders.LeagueBuilder(tournament);
        }
        else if (tournament.type == 2) {
            return new System.Bracket.Builders.SingleEliminationBuilder(tournament);
        }
        else if (tournament.type == 3) {
            return new System.Bracket.Builders.Group.Builder(tournament);
        }
        return null;
    };

    return {
        get: this.get
    };
}());