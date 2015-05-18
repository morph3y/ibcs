registerNamespace("System.Team");

System.Team.TeamBuilder = function (data) {
    var self = this,
        rowTemplate = $('' +
            '<tr>' +
            '   <td><span class="remove-member">Remove</span></td>' +
            '   <td class="member-name"></td>' +
            '   <td class="accepted"></td>' +
            '</tr>'),
        dialogOptions = {
            autoOpen: false,
            content: $('' +
                '<div class="playerList">' +
                '   <select class="players"></select>' +
                '</div>'),
            buttons: [{ text: 'Add' }, { text: 'Cancel' }],
            onOpen: function (dialog) {
                dialog.content.find('.players option').remove();
                $.ajax({
                    method: 'POST',
                    url: System.Team.Urls.getAvailableMembers,
                    data: { teamId: self.team.id },
                    success: function (data) {
                        data = $.parseJSON(data);
                        var selectElement = dialog.content.find('.players');
                        for (var i = 0, il = data.length; i < il; i++) {
                            selectElement.append('<option value="' + data[i].id + '">' + data[i].name + '</option>');
                        }
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            },
            Add: function (dialog) {
                var selectedMember = dialog.content.find('.players option:selected').val();
                if (selectedMember != null) {
                    $.ajax({
                        method: 'POST',
                        url: System.Team.Urls.addMember,
                        data: { teamId: self.team.id, memberId: selectedMember },
                        success: function (data) {
                            data = $.parseJSON(data);
                            if (self.wrapper != null) {
                                self.wrapper.find('table tbody').append(createMemberRow(data));
                            }
                            dialog.close();
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            },
            Cancel: function (dialog) {
                dialog.close();
            }
        };

    /*
        PUBLIC PROPERTIES
    */
    self.team = data || {};
    self.members = self.team.members || [];
    self.wrapper = null;

    /*
        PRIVATE METHODS
    */
    function createMemberRow(member) {
        var newRow = rowTemplate.clone();
        newRow.find('.member-name').text(member.name);
        newRow.find('.accepted').text(member.accepted === true ? "Yes" : "No");
        newRow.find('.remove-member').click(function () {
            $.ajax({
                method: 'POST',
                url: System.Team.Urls.removeMemberUrl,
                data: { teamId: self.team.id, memberId: member.id },
                success: function (data) {
                    if (data.success === true) {
                        newRow.remove();
                    } else {
                        alert('wtf');
                    }
                },
                error: function (data) {
                    alert(data);
                }
            });
        });
        return newRow;
    }

    /*
        PUBLIC METHODS
    */
    self.render = function (wrapper) {
        // re-render with null
        self.wrapper = wrapper || self.wrapper;

        var tableTemplate = self.wrapper.find('table');
        // add button
        if (self.team.id != 0) {
            // existing team
            tableTemplate.find('.add-member').click(function () {
                new System.Dialog(dialogOptions);
            });
        }

        for (var i = 0, il = self.members.length; i < il; i++) {
            tableTemplate.find('tbody').append(createMemberRow(self.members[i]));
        }
    };

    return self;
};