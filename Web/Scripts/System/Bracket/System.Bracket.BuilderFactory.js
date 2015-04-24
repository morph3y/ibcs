registerNamespace("System.Bracket.BuilderFactory");

System.Bracket.BuilderFactory = (function () {
    this.get = function(tournament) {
        if (tournament.type == 1) {
            // league
            return new System.Bracket.Builders.LeagueBuilder(tournament);
        }
        return null;
    };

    return {
        get: this.get
    };
}());