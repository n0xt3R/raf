$(document).ready(function () {

    $('body').on('blur', '.votingItem', function (c) {

        var total = 0;
        $(".votingItem").each(function (k, v) {
            if (parseInt($(this).val()) === 0) {
                $(this).val();
            }
            total = total + parseInt($(this).val());
        });
        $(".totalNumberofBallot").val(total);
    });

    $('body').on('blur', '.aAndBCount', function (c) {

        var total = 0;
        total = parseInt($(".NumberOfSpoiltBallotPapersInput").val()) + parseInt($(".TotalNumberOfBallotsCastInput").val());

        $(".NumberOfBallotPapersUsedInput").val(total);
    });
    $('body').on('blur', '.cAndBCount', function (c) {

        var total = 0;
        total = parseInt($(".NumberOfBallotPapersUsedInput").val()) + parseInt($(".NumberOfBallotPapersUnusedInput").val());

        $(".TotalBallotsPaperIssuedInput").val(total);
    });

    $('body').on('blur', '.rejAndrebCount', function (c) {

        var total = 0;
        total = parseInt($(".NumberOfRejectedBallots").val()) + parseInt($(".NumberOfRejectionBallots").val());
        $(".TotalNumberOfRejectedBallots").val(total);
    });
    $('body').on('blur', '.TCountAndTrejCount', function (c) {

        var total = 0;
        total = parseInt($(".totalNumberofBallot").val()) + parseInt($(".TotalNumberOfRejectedBallots").val());
        $(".TotalNumberOfBallotsCastInput").val(total);
        console.log("here");
    });



});