$(document).ready(function () {
    var bgcolor;
    $('.table').on('mouseover', 'tbody tr', function () {
        bgcolor = $(this).css("background-color");
        $(this).css({ "background-color": "rgb(222,235,247)", "color": "rgb(132,151,176)", "cursor": "pointer" });
    })

    $('.table').on('mouseout', 'tbody tr', function () {
        $(this).css({ "background-color": bgcolor, "color": "#222" });
    });

});

