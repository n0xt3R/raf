$(document).ready(function () {
    let $pollingAreaSelect = $('#PollingAreaId')

    $('#Picture').on('change', function () {
        let selectedFile = $(this)[0].files[0]
        let reader = new FileReader()

        let $img = $('#picture_img')
        $img.attr('title', selectedFile.name)

        reader.onload = function (event) {
            $img.attr('src', reader.result)
        }

        reader.readAsDataURL(selectedFile);
    })

    function autofillDivision() {
        let $divisionInput = $('#division_display')
        $divisionInput.val($pollingAreaSelect.find(':selected').attr('data-division'))
    }

    $pollingAreaSelect.on('change', autofillDivision)

    autofillDivision()

})