
$(document).ready(function () {

    $('body').on('click', '.bz', function (c) {
        map.setView([17.54977183258917, -88.29849243164062], 10);
    });

    $('body').on('click', '.ca', function (c) {
        map.setView([17.178843023062424, -88.83338928222656], 11);
    });

    $('body').on('click', '.ow', function (c) {
        map.setView([17.877470924490204, -88.74206542968749], 11);
    });

    $('body').on('click', '.sc', function (c) {
        map.setView([16.886360164017468, -88.25386047363281], 11);
    });

    $('body').on('click', '.co', function (c) {
        map.setView([18.22282937523598, -88.35548400878906], 11);
    });

    $('body').on('click', '.to', function (c) {
        map.setView([16.247638441537, -88.9508056640625], 11);
  
    });

    $('body').on('click', '.btn-collapsed', function (c) {
        if (!$(this).hasClass("collapsed")) {
            map.setView([17.3769, -88.6130], 9);
        }
    });

    $('body').on('click', '.reset-view', function (c) {
             map.setView([17.3769, -88.6130], 9);
            $('.collapse').collapse('hide');
    });
    setInterval(function () {
   
         $.post("/api/taskapi/DC", { UMKID: $(".UMKID").val(), elecID: $(".elecID").val()}, function (data) {
                      if (data.result === true) {
                          RefreshAllDistrictsColorsInAPI();
                          repopulateDistricts();
                          repopulatePA();
                          console.log(data.umkid);
                          $(".UMKID").val(data.umkid);   
                       }
        });

    }, 5000);

    function repopulateDistricts() {
        $(".detectChanges").each(function (index) {
            var district = $(this);
            $.get("/api/taskapi/GHDCC", { district: district.data("district") }, function (data) {
             district.text(data);              
            });
        });
    }
    function repopulatePA() {
        $(".detectChangesPA").each(function (index) {
            var pa = $(this);
            var progressBar = $(this).children('.progress-bar');
            var percentage = $(this).children('.percentage').children();
            $.get("/api/taskapi/GHPAC", { pa: pa.data("pa") }, function (data) {
                progressBar.css("background-color", data.bg);
                progressBar.css("width", data.width);
                percentage.text(data.turnOutPercent);
            });
        });
    }


  
  
});