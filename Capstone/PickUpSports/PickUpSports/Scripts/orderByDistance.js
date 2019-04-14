function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}

function showPosition(position) {
    var lat = position.coords.latitude;
    var long = position.coords.longitude;
    jQuery.ajax({
        type: "POST",
        url: "../../Venue/Index/",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ curLat: lat, curLong: long, sortBy: "Distance" }),
        success: function (data) {
            alert(data);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });


}

getLocation();
