$(document).ready(function () {
    $("select").prepend("<option value=''>Select one</option>");
    $("select option[value='']").attr("selected", "selected");
});
