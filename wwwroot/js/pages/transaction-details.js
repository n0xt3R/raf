$(document).ready(function () {
    // RE-JSON the string to format properly
    $data = $('#json')
    $data.html(JSON.stringify(JSON.parse($data.html()), undefined, 2))
})