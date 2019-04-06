function gameList(results) {
    $("#game-list").find("tbody").empty(); 

    if (results !== "") {
	    $.each(JSON.parse(results),
		    function(key, item) {
                console.log(item);
                var gameItem = "<tr><td>" + item.ContactId +
                    "</td><td>" + item.GameStatusId + 
                    "</td><td>" + item.StartTime + 
                    "</td><td>" + item.EndTime + "</td></tr>";

                $("#game-list").find("tbody").append(gameItem);

		    });

    }


}

function errorOnAjax() {
	console.log("error");
}

/*main method, performs AJAX with dropdown list */
function main() {

    console.log("loaded!");

    $("#VenueId").on("change",
	    function(e) {
		    var venueId = this.value;

		    $.ajax({
			    type: "GET",
			    dataType: "json",
			    url: "/Game/GetGamesResult",
			    data: { "venueId": venueId },
                success: gameList,
                error: errorOnAjax
                
		    });
	    });

}

/*When the Page loads, run the main method */
$(document).ready(main);