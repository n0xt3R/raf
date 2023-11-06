$(document).ready(function () {
    const $isRo = $('#isRo');
    const $regOfficerSelectDiv = $("#registerofficerdiv");
    $isRo.on('change', function () {
        if (!this.checked) {
            $regOfficerSelectDiv.show();
        }
        else {
            $regOfficerSelectDiv.hide();
        }
    });
    if ($isRo.is(':checked')) {
        $regOfficerSelectDiv.hide();
    }

   
    $("input[rel='Level1']").change(function () {
        if (this.checked) {
            $("input[rel='Level4']").prop('checked', false);
            $("input[rel='Level3']").prop('checked', false);
        }
    });

    $("input[rel='Level2']").change(function () {
        if (this.checked) {
            $("input[rel='Level4']").prop('checked', false);
            $("input[rel='Level3']").prop('checked', false);
        }
    });

    $("input[rel='Level3']").change(function () {
        if (this.checked) {
            $("input[rel='Level1']").prop('checked', false);
            $("input[rel='Level2']").prop('checked', false);
            $("input[rel='Level4']").prop('checked', false);
        }
    });
    $("input[rel='Level4']").change(function () {
        if (this.checked) {
            $("input[rel='Level1']").prop('checked', false);
            $("input[rel='Level2']").prop('checked', false);
            $("input[rel='Level3']").prop('checked', false);
        }
    });
})