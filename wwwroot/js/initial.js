function renderChart(data, colors, labels,district) {
    var ctx = document.getElementById("myChart");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'This week  1',
                data: data,
                borderColor: 'rgba(75, 192, 292, 1)',
                backgroundColor: colors,
            }]
        },
        options: {
            title: {
                display: true,
                text: district + " HOURLY COUNT",
                fontWeight: "100",
                fontSize: "18",
                fontStyle: "normal",
                fontFamily:"Montserrat"
            },
            layout: {
                padding: {
                    bottom: 15,
                    left: 15,
                    right: 10,
                    top: 10,

                },
            },
            legend: { display: false },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,

                    }
                }]
            }
        },
    });
}