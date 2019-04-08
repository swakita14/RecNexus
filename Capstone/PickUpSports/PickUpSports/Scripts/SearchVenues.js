/*getting results and manipulating the DOM */
function gameList(results) {
    $("#game-list").find("tbody").empty(); 

    if (results !== "") {
	    $.each(JSON.parse(results),
		    function(key, item) {
                console.log(item);
                var gameItem = "<tr><td><a href=mailto:" + item.ContactPerson.Email + ">" + item.ContactPerson.Username +
                    "</a></td><td>" + item.Status + 
                    "</td><td>" + item.StartTime + 
                    "</td><td>" + item.EndTime +
                    "</td><td><a href=../Game/Details/" + item.GameId + ">Game Details</a></td></tr>";

                $("#game-list").find("tbody").append(gameItem);

		    });

    }


}

/* error on AJAX in case something goes wrong*/
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