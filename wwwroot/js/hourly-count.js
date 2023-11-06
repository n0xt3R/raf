$(document).ready(function(){

    var placeholderElement = $('#modal-placeholder');
    //if counting in more than 
    $('button[data-toggle="ajax-add-hour"]').click(function (event) { 
        var url = $(this).data('url');
        $.get(url, { id:$(this).data("ps")}).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });
    $('body').on('click', 'button[data-toggle="ajax-edit-hour"]', function (c) {

        var url = $(this).data('url');
        $.get(url, { ps: $(this).data("ps"), hdid: $(this).data("hdid")}).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });




    placeholderElement.on('click', '[data-save="add-hour"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        var ps = $(this).data("ps");
        var repoUrl = $(this).data("repourl");
        var editUrl = $(this).data("editurl");
        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            var isValid = newBody.find('[name="IsValid"]').val() === 'True';
            if (isValid) {
                repopulation(ps, repoUrl, editUrl);
                placeholderElement.find('.modal').modal('hide');
            }
            
        });
    });

    placeholderElement.on('click', '[data-save="edit-hour"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        var ps = $(this).data("ps");
        var repoUrl = $(this).data("repourl");
        var editUrl = $(this).data("editurl");

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            var isValid = newBody.find('[name="IsValid"]').val() === 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
                repopulation(ps, repoUrl, editUrl);
            }
        });
    }); 

    function repopulation(ps, repoUrl, editUrl) {
        var table = $("#addTable tbody");
        $.get(repoUrl, { ps: ps })
            .done(function (data) {
                table.empty();
                data.forEach(function (item) {

                    var links = "<button type=\"button\"   data-ps=\"" + ps + "\" data-hdid=\"" + item["id"] + "\" data-url=\"" + editUrl + "\" data-target=\"#edit-hour\" data-toggle=\"ajax-edit-hour\"  class=\"btn btn-outline-dark  btn-sm\"><i class=\"icon icon-pencil\" style=\"vertical-align:text-bottom;\"></i> Edit</button></td>";
                    table.append("<tr id=\"" + item["id"] + "\"><td><strong>" +
                        item["hour"]["timeSlot"]+   "</strong></td><td>" +                    
                        item["ballotIssuedPerHour"] + "</td><td>" +
                        item["runningTotalIssued"] + "</td><td>" +
                        item["spoilt"] + "</td><td>" +
                        item["runningTotalSpoilt"] + "</td><td>" +
                        item["totalBallotsInBox"] + "</td><td>" +

                        links + "</td><td>" +
                        "<i data-position=\"left\" data-delay=\"50\" data-key=\"" + item["id"] + "\" ></i>" + "</tr>");                

                });
            });

    }
});