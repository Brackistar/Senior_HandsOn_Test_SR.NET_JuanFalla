$(document).bind("ajaxSend", function () {
    $("#loading").show();
}).bind("ajaxComplete", function () {
    $("#loading").hide();
});

function HintPanEmpty(SearchHintPan) {
    $(SearchHintPan).hide("fast");
    $(SearchHintPan).html(null);
}