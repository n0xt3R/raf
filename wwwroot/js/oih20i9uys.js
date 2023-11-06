$(document).ready(function() {
    var api = "/api/TaskApi/";
    $('.addUserSelect').select2({
     placeholder: 'Select an user',
        theme: "classic"
    });



    $('.modal').modal({
        show: false,
        backdrop: 'static',
        keyboard: false,
        dismissible: false, // Modal can be dismissed by clicking outside of the modal
        opacity: .9, // Opacity of modal background
        inDuration: 300, // Transition in duration
        outDuration: 200, // Transition out duration
        startingTop: '4%', // Starting top style attribute
        endingTop: '10%', // Ending top style attribute
        ready: function (modal, trigger) { // Callback for Modal open. Modal and trigger parameters available.

        },
        complete: function () {

        } // Callback for Modal close
    }
    );
    $('body').on('click', '.delbtn', function (c) {
        var userID = $(this).attr("data-id");
        var pa = $(this).attr("data-key");
        var table = $('#UserList tbody');
        var elec = $('#electionID').val();

        if (confirm("Are you sure you want to delete this user?")) {


            $.ajax({
                url: api + "DUFP",
                type: "post",
                data: { ps: pa, elec: elec, userID: userID },
                success:
                    function (data) {
                        if (data === false) {

                            $("[data-id='" + pa + "'] #indicator").addClass("text-danger icon-close");
                            $("[data-id='" + pa + "'] #indicator").removeClass("text-success icon-check");

                        }
                        $('#modal-teams .modal-body').prepend(
                            '<div class="alert-error alert alert-danger" role="alert">' +
                            'User sucessfully deleted.' +
                            '</div>'
                        );

                        window.setTimeout(function () { $(".alert-error").fadeOut('slow'); }, 2000);
                        repopulateList(pa, elec);
                    },
                beforeSend: function (data) {
                    $('#overlay').addClass('overlay');
                },
                error:
                    function (xhr, ajaxOptions, thrownError) {
                        $('#modal-teams .modal-body').prepend(
                            '<div class="alert-error alert alert-danger" role="alert">' +
                            'Encountered an error. Contact administrator immediately.' +
                            '</div>'
                        );
                        $('#overlay').removeClass('overlay');
                        window.setTimeout(function () { $(".alert-error").fadeOut('slow'); }, 2000);

                    }
            });


        }
    
    });
    $('body').on('click', '.getMembers', function (c) {
        var pa = $(this).attr("data-id");
        $(".modal-title").text("Edit Team for "+$(this).attr("title"));
        var elec = $(this).attr("data-ele-id");
        $(".addMembers").attr("data-pa", pa);
        repopulateList(pa, elec);
  });
    
    $('body').on('click', '.addMembers', function (c) {
        var ps = $(this).attr("data-pa");
        var elec = $('#electionID').val();

        var id = $('.addUserSelect').find(':selected').attr('id');
        if ($.trim(id)) {

            $.ajax({
                url: api+"ANM",
                type: "post",
                data: {ps: ps, elec: elec,userID:id},
                success:
                    function (data) {
                        if ($.trim(data)) {
                            if (data === true) {
                                $("[data-id='" + ps + "'] #indicator").removeClass("text-danger icon-close");
                                $("[data-id='" + ps + "'] #indicator").addClass("text-success icon-check");
                            }
                        

                            $('#modal-teams .modal-body').prepend(
                              '<div id="sche-alert-error" class="alert alert-success" role="alert">' +
                              'User added successfully.' +
                              '</div>'
                             );
                            window.setTimeout(function () { $(".alert-success").fadeOut("slow"); }, 2000);
                            repopulateList(ps,elec);

                        }
                 
                   
                    },
                beforeSend: function (data) {
                    $('#overlay').addClass('overlay');
                },
                error:
                    function (xhr, ajaxOptions, thrownError) {
                        $('#modal-teams .modal-body').prepend(
                            '<div class="alert-error alert alert-danger" role="alert">' +
                            'Encountered an error. Contact administrator immediately.' +
                            '</div>'
                        );
                        $('#overlay').removeClass('overlay');
                        window.setTimeout(function () { $(".alert-error").fadeOut("slow"); }, 2000);

                    }


            });
        }
        else {

         $('#modal-teams .modal-body').prepend(
                '<div id="alert-error" class="alert-error alert alert-primary alert-dismissible" role="alert">' +
                'No user selected.' +
                '</div>'
            );
            window.setTimeout(function () { $(".alert-error").fadeOut("slow"); }, 2000);

        }
       
    });
    function repopulateList(pa,elec) { 
        var table = $('#UserList tbody');

        $.ajax({
            url: api+"GetFUL", 
            type: "post",
            data: { ps: pa, elec:elec},
            success:
                function (data) {
                    $(".addUserSelect").empty(); 
                    $('.addUserSelect').append('<option value="" disabled selected>Select a user</option>');
                    $.each(data, function (i, item) {
                        $('.addUserSelect').append($('<option>', {
                            value: item.firstName,
                            text: item.firstName + " " + item.lastName+" - "+item.roles,
                            id: item.id
                        }));
                    });

                    $('#overlay').removeClass('overlay');
                },
            beforeSend: function (data) {
                $('#overlay').addClass('overlay');
            },
            error:
                function (xhr, ajaxOptions, thrownError) {
                    $('#modal-teams .modal-body').prepend(
                        '<div class="alert-error alert alert-danger" role="alert">' +
                        'Encountered an error. Contact administrator immediately.' +
                        '</div>'
                    );
                    $('#overlay').removeClass('overlay');
                    window.setTimeout(function () { $(".alert-error").fadeOut("slow"); }, 2000);

                }
        });


        $.ajax({
            url: api+"getUL",
            type: "post",
            data: { ps: pa, elec: elec },
            success:

                function (data) {
                    table.empty();
                    if ($.trim(data)) {
                        data.forEach(function (item) {
                            var links = "<a data-id=\"" + item["id"] + "\" data-key=\"" + pa + "\" class=\"delbtn btn btn-sm btn-danger\" href=\"javascript:void(0);\">Delete</a>";
                            table.append("<tr><td>" + item["firstName"] + "</td><td>" + item["lastName"] + "</td><td>" + item["cellPhone"] + "</td><td>" + item["roles"] + "</td><td>" + links + "</td></tr>");

                        });
                    }
                    else {
                        table.append("<tr><td colspan=\"5\">No Users added yet</td></tr>");
                    }

                    $('#overlay').removeClass('overlay');
                },
            beforeSend: function (data) {
                $('#overlay').addClass('overlay');
            },
            error:
                function (xhr, ajaxOptions, thrownError) {
                    $('#modal-teams .modal-body').prepend(
                        '<div id="sche-alert-error" class="alert alert-danger" role="alert">' +
                        'Encountered an error. Contact administrator immediately.' +
                        '</div>'
                    );
                    $('#overlay').removeClass('overlay');
                    window.setTimeout(function () { $(".alert-danger").fadeOut("slow"); }, 4000);

                }
        });
    }



});