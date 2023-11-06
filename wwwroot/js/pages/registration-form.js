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

    $('#CountryId').on('change', function () {
        let selectedName = $(this).find(':selected').html().trim().toUpperCase()
        // Autofill place of birth and nationality
        if (selectedName === 'BELIZE') {
            //let $placeOfBirthInput = $('#PlaceOfBirth')
            let $nationalityInput = $('#Nationality')

            /*if ($placeOfBirthInput.val().trim() === '') {
                $placeOfBirthInput.val('Belize')
                $placeOfBirthInput.addClass('border-primary')
                $placeOfBirthInput.css('transition', 'border 0.2s')
                setTimeout(function () { $placeOfBirthInput.removeClass('border-primary') }, 1000);
            }*/
            
            if ($nationalityInput.val().trim() === '') {
                $nationalityInput.val('Belizean')
                $nationalityInput.addClass('border-primary')
                $nationalityInput.css('transition', 'border 0.2s')
                setTimeout(function () { $nationalityInput.removeClass('border-primary') }, 1000);
            }
        }
    })

    function autofillDivision() {
        let $divisionInput = $('#division_display')
        $divisionInput.val($pollingAreaSelect.find(':selected').attr('data-division'))
    }

    $pollingAreaSelect.on('change', autofillDivision)

    autofillDivision()
})