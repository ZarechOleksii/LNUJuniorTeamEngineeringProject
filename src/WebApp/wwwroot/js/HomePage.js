var page = 0;
const pageSize = 1;
var isHasMovieYet = true;
var lastKnownScrollPosition = 0;
var ticking = false;
var startLoadingCount = 3;

setIntervalX(loadMore, 1000, startLoadingCount);

$(window).scroll(function () {
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        loadMore();
    }
});

function setIntervalX(callback, delay, repetitions) {
    var x = 0;
    var intervalID = window.setInterval(function () {

        callback();

        if (++x === repetitions) {
            window.clearInterval(intervalID);
        }
    }, delay);
}

function loadMore() {
    if (isHasMovieYet) {
        $.ajax({
            type: "GET",
            url: "/Home/LoadMoreMovies",
            data: { page: page, pageSize: pageSize },
            async: true,
            dataType: "json",
            success: function (data) {
                page += 1;
                for (let i = 0; i < pageSize; i++) {
                    try {
                        var movie = document.createElement("div");
                        var movieName = document.createElement("div");
                        var movieDescription = document.createElement("div");

                        movie.setAttribute("class", "movie");
                        movieName.setAttribute("class", "movie-name");
                        movieName.innerHTML = data[i].name;
                        movieDescription.setAttribute("class", "movie-description");
                        movieDescription.innerHTML = data[i].description;


                        movie.appendChild(movieName);
                        movie.appendChild(movieDescription);

                        var movieList = document.getElementById("movie-list");
                        movieList.appendChild(movie);
                    }
                    catch (e) {
                        isHasMovieYet = false;
                    }
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    }
}