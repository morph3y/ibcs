registerNamespace("System.Notifications");

System.Notifications = function(data) {
    /*
        PRIVATE PROPERTIES
    */
    var self = this;

    /*
        PRIVATE METHODS
    */
    function updateRequestNumer(number) {
        if (self.wrapper != null) {
            self.wrapper.find('span.number').text(number);
        }
    }

    /*
        PUBLIC PROPERTIES
    */
    self.data = data || {};
    self.wrapper = null;

    /*
        CONSTRUCTOR
    */
    self.membershipRequests = new System.Notifications.MembershipRequests(self.data.membershipRequests, {
        onChange: function() {
            updateRequestNumer(self.membershipRequests.data.length);
        }
    });

    /*
        PUBLIC METHODS
    */
    self.render = function (wrapper) {
        self.wrapper = wrapper || self.wrapper;

        updateRequestNumer(self.data.membershipRequests == null ? 0 : self.data.membershipRequests.length);

        self.wrapper.find('span.number').click(function () {
            self.membershipRequests.render();
        });
    };
};