registerNamespace("System.Bracket");

System.Bracket = function (model) {
    var self = this;

    self.model = model;

    self.init = function (wrapper) {
        var builder = System.Bracket.BuilderFactory.get(self.model);
        builder.render(wrapper);
    };

    return self;
};