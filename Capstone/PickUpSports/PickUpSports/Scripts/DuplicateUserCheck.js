$(document).ready(function () {

    var user_data = $("#Username").val();

    var source = "/Contacts/CheckDuplicateUser/" + user_data;

    $.ajax({
        type: "POST",
        dataType: "json",
        data: { "username": user_data },
        url: source,
        success: showStatus,
        error: errorOnAjax

    });

    function showStatus(data) {
        if (data === 0) {
            $("#Status").html('<font color="Green">Available !. you can take it.</font>');
            $("#Username").css("border-color", "Green");

        }
        else {
            $("#Status").html('<font color="Red">That name is taken.Try Another.</font>');
            $("#Username").css("border-color", "Red");
        }

    }

    function errorOnAjax()
    {
        console.log("error");
    }

});