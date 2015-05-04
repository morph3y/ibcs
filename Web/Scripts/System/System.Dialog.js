registerNamespace("System.Dialog");

System.Dialog = function (options) {
    var self = this,
        container = $('<div class="jquery-dialog"></div>');

    /*
        PUBLIC PROPERTIES
    */
    self.content = options.content || container;
    self.options = $.extend({}, options);

    /*
        PUBLIC METHODS
    */
    self.close = function() {
        container.dialog('close');
    };

    /*
        CONSTRUCTOR
    */
    $('body').append(container.append(self.content));

    // build buttons
    var btns = [];
    if (options.buttons != null) {
        for (var i = 0, il = options.buttons.length; i < il; i++) {
            (function(index) {
                btns.push({
                    text: options.buttons[index].text,
                    click: function () {
                        if ($.isFunction(options[options.buttons[index].text])) {
                            options[options.buttons[index].text](self);
                        }
                    }
                });
            }(i));
        }
    }

    container.dialog({
        autoOpen: true,
        modal: true,
        open: function(e, ui) {
            if ($.isFunction(self.options.onOpen)) {
                self.options.onOpen(self);
            }
        },
        buttons: btns
    });

    return self;
};