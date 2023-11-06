$(document).ready(function() {
    var api = "/api/TaskApi/";

    $('#LockOut').click(function () {
        var elec = $(this).attr("data-val"); 
      
        $.ajax({
            url: api + "LE",
            type: "post",
            data: { elec: elec },
            success:
                function (data) {
                   
                    if ($('#LockOut').is(':checked')) {
                        $('.card-body').prepend(
                            '<div class="alert-error alert alert-warning" role="alert">' +
                            'Election sucessfully locked' +
                            '</div>'
                        );

                    }
                    else {
                        $('.card-body').prepend(
                            '<div class="alert-error alert alert-success" role="alert">' +
                            'Election sucessfully unlocked' +
                            '</div>'
                        );
                    }
              
                    $('#overlay').removeClass('overlay');
                   

                    window.setTimeout(function () { $(".alert-error").fadeOut('slow'); }, 3000);
                },
            beforeSend: function (data) {
                $('#overlay').addClass('overlay');
            },
            error:
                function (xhr, ajaxOptions, thrownError) {
                    $('.header').append(
                        '<div class="alert-error alert alert-danger" role="alert">' +
                        'Encountered an error. Contact administrator immediately.' +
                        '</div>'
                    );
                    $('#overlay').removeClass('overlay');
                    window.setTimeout(function () { $(".alert-error").fadeOut('slow'); }, 2000);

                }
        });

    });
        
  
    });
   