registerNamespace("System.Notifications.MembershipRequests");

System.Notifications.MembershipRequests = function (data, options) {
    /*
        PRIVATE PROPERTIES
    */
    var self = this,
        template = $('' +
            '<table>' +
            '   <thead>' +
            '       <tr>' +
            '           <th></th>' +
            '           <th>Team name</th>' +
            '       </tr>' +
            '   </thead>' +
            '   <tbody></tbody>' +
            '</table>'),
        dialogOptions = {
            autoOpen: false,
            buttons: [{ text: 'Cancel' }],
            Cancel: function (dialog) {
                dialog.close();
            }
        },
        defaultOptions = {
            onChange: function () {}
        };

    /*
        PUBLIC PROPERTIES
    */
    self.data = data || [];
    self.options = $.extend({}, defaultOptions, options);

    /*
        PUBLIC METHODS
    */
    self.render = function () {
        var allRequests = template.clone(),
            sendRequest = function (url, row) {
                var requestObject = row.data('request');

                $.ajax({
                    method: 'POST',
                    url: url,
                    data: { teamId: requestObject.teamId, memberId: requestObject.memberId },
                    success: function (data) {
                        row.remove();
                        self.data.splice(self.data.indexOf(requestObject), 1);
                        self.options.onChange();
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            };

        for (var i = 0, il = self.data.length; i < il; i++) {
            var rowTemplate = $('' +
                '<tr>' +
                '   <td><span class="accept-request">Accept</span> / <span class="reject-request">Reject</span></td>' +
                '   <td>' + self.data[i].teamName + '</td>' +
                '</tr>').data('request', self.data[i]);
            allRequests.find('tbody').append(rowTemplate);
        }

        allRequests.find('.accept-request').click(function() {
            sendRequest(System.Notifications.Urls.acceptRequestUrl, $(this).closest('tr'));
        });

        allRequests.find('.reject-request').click(function () {
            sendRequest(System.Notifications.Urls.rejectRequestUrl, $(this).closest('tr'));
        });

        dialogOptions.content = allRequests;
        new System.Dialog(dialogOptions);
    }
};