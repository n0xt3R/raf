$(document).ready(function () {
    var ctx = $('#registrations_chart')

    var registrationsChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["You", "Others", "Pending"],
            datasets: [{
                data: [registrations_chart_my_count, registrations_chart_other_count, registrations_chart_rem_count],
                backgroundColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                ],
                borderColor: 'rgba(255, 255, 255, 1)',
                hoverBorderColor: 'rgba(200, 200, 200, 1)',
                hoverBorderWidth: 1,
                borderWidth: 2
            }]
        },
        options: {
            cutoutPercentage: 70
        }
    })
})