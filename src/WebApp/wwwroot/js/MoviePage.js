function add_to_favorite(movie_id) {
    $.ajax({
        type: "POST",
        url: "/Movies/AddToFavourite",
        data: { id_movie: movie_id },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: added to favorite");
            var b = document.getElementsByClassName("favorite-button")[0];
            b.setAttribute("onclick", "delete_from_favorite(this.value)");
            b.innerHTML = "Delete from Favorites";
            b.setAttribute("class", "favorite-button-delete");
        },
        error: function (req, status, error) {
            console.log("BAD: NOT added to favorite");
        }
    });
}
function delete_from_favorite(movie_id) {
    $.ajax({
        type: "DELETE",
        url: "/Movies/DeleteFromFavourite",
        data: { id_movie: movie_id },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: deleted from favorite");
            var b = document.getElementsByClassName("favorite-button-delete")[0];
            b.setAttribute("onclick", "add_to_favorite(this.value)");
            b.innerHTML = "Add to Favorites";
            b.setAttribute("class", "favorite-button");
        },
        error: function (req, status, error) {
            console.log("BAD: NOT deleted from favorite");
        }
    });
}

function formSubmit(event) {
    var url = $(this).closest('form').attr('action');
    var request = new XMLHttpRequest();
    request.open('POST', url, true);
    request.onload = function () { 
        document.getElementById("comment").reset();
    };

    request.onerror = function () {
        console.error("fail")
    };

    request.send(new FormData(event.target)); 
    event.preventDefault();
}

function DeleteComment(comment_id) {
    $.ajax({
        type: "DELETE",
        url: "/Movies/DeleteComment",
        data: { id: comment_id },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: deleted comment");
            location.reload(true);
        },
        error: function (req, status, error) {
            console.log("BAD: NOT deleted comment");
        }
    });
}

document.getElementById("comment").addEventListener("submit", formSubmit);
