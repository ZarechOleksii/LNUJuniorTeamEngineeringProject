function add_to_favorite(movie_id) {
    $.ajax({
        type: "POST",
        url: "/Movies/AddToFavourite",
        data: { id_movie: movie_id },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: added to favorite");
            var b = document.getElementsByClassName("favorite-button add")[0];
            b.setAttribute("onclick", "delete_from_favorite(this.value)");
            b.innerHTML = "Delete from Favorites";
            b.setAttribute("class", "favorite-button delete");
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
            var b = document.getElementsByClassName("favorite-button delete")[0];
            b.setAttribute("onclick", "add_to_favorite(this.value)");
            b.innerHTML = "Add to Favorites";
            b.setAttribute("class", "favorite-button add");
        },
        error: function (req, status, error) {
            console.log("BAD: NOT deleted from favorite");
        }
    });
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

function deleteMovieAdmin(movieId) {
    $.ajax({
        type: "DELETE",
        url: "/Movies/DeleteMovieAdmin",
        data: { movieId: movieId },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: deleted from favorite");
            window.location.href = "/";
        },
        error: function (req, status, error) {
            console.log("BAD: NOT deleted from favorite");
        }
    });
}

function addRate(movieId) {
    var slider = document.getElementById("myRange");
    $.ajax({
        type: "POST",
        url: "/Movies/AddRate",
        data: { movieId: movieId, rate: slider.value },
        async: true,
        dataType: "text",
        success: function (msg) {
            console.log("OK: rate is added");
            getRate(movieId);
            slider.style.backgroundSize = (slider.value - slider.min) * 100 / (slider.max - slider.min) + '%';
        },
        error: function (req, status, error) {
            console.log("BAD: rate is NOT added");
        }
    });
}

function getRate(movieId) {
    $.ajax({
        type: "GET",
        url: "/Movies/GetRate",
        data: { movieId: movieId },
        async: true,
        dataType: "json",
        success: function (data) {
            console.log("OK: rate is get");
            if (data.rateValue != 0) {
                document.getElementById("rate-mark").innerHTML = "Average rate: " + data.rateValue;
            }
        },
        error: function (req, status, error) {
            console.log("BAD: rate is NOT get");
        }
    });
}

function addComment(movieId) {
    var commentContent = document.getElementById("comment-content");
    $.ajax({
        type: "POST",
        url: "/Movies/AddComment",
        data: { movieId: movieId, content: commentContent.value },
        async: true,
        dataType: "text",
        success: function (data) {
            console.log("OK: comment is added");
            location.reload();
        },
        error: function (req, status, error) {
            console.log("BAD: comment is NOT added");
        }
    });
}

