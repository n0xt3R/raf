$(document).ready(function () {
    repopulation("", "", "", "", "");
    $('#data-fn').on('keyup', function () {
        repopulation($('#data-fn').val(), $('#data-ln').val(), $('#data-ds').val(), $('#data-ct').val(), $('#data-oc').val());
    });
    $('#data-ln').on('keyup', function () {
        repopulation($('#data-fn').val(), $('#data-ln').val(), $('#data-ds').val(), $('#data-ct').val(), $('#data-oc').val());
    });
    $('#data-ds').on('change', function () {
        repopulation($('#data-fn').val(), $('#data-ln').val(), $('#data-ds').val(), $('#data-ct').val(), $('#data-oc').val());
    });
    $('#data-ct').on('change', function () {
        repopulation($('#data-fn').val(), $('#data-ln').val(), $('#data-ds').val(), $('#data-ct').val(), $('#data-oc').val());
    });
    $('#data-oc').on('change', function () {
        repopulation($('#data-fn').val(), $('#data-ln').val(), $('#data-ds').val(), $('#data-ct').val(), $('#data-oc').val());
    });
});


function repopulation(firstname, lastname, district, city) {
    $.ajax({
        url: api_gs,
        type: "post",
        contentType: "application/x-www-form-urlencoded",

        data: { FirstName: firstname, LastName: lastname, District: district },
        success:
            function (data) {
                var table = $('#searchEABgrid tbody');
                table.empty();
                if (data.length === 0) {
                    table.append("<tr><td colspan=8>" + "<strong>Oops.... Nothing was found</strong></td ></tr>");
                }
                data.forEach(function (item) {

                    table.append("<tr id=\"" + item["id"] + "\"><td >" +

                        item["firstName"] + "</td><td >" +

                        item["lastName"] + "</td><td >" +

                        item["jobTitle"] + "</td><td >" +
                        item["userName"] + "</td><td >" +
                        item["district"] + "</td><td>" +
                        "<a style=\"margin-right:5px\" class=\"btn btn-success btn-small\" href='/Manage/EditUser?id=" + item["id"] + "'>Edit Info</a>" +

                        "</td></tr>");
                });

            },

        beforeSend: function (data) {
            $('#overlay').addClass('overlay');
        },
        error:
            function (xhr, ajaxOptions, thrownError) {
                onErrorRequest(xhr, ajaxOptions, thrownError);
            }
    });
}