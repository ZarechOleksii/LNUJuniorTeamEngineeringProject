var page = 0;
const pageSize = 5;
var isHasMovieYet = true;
var lastKnownScrollPosition = 0;
var ticking = false;
var startLoadingCount = 2;
var intervalBetweenLoading = 1000;
var isloading = false;

setIntervalX(loadMore, intervalBetweenLoading, startLoadingCount);

$(window).scroll(function () {
    if (Math.ceil($(window).scrollTop()) == $(document).height() - $(window).height()) {
        loadMore();
        isloading = true;
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
    if (isHasMovieYet && !isloading) {
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
                        var movieBottom = document.createElement("div");
                        movie.setAttribute("class", "movie");
                        movie.addEventListener("click", function () {
                            document.location.href = `/Movies/Get?id=${data[i].id}`;
                        });
                        movieName.setAttribute("class", "movie-name");
                        movieName.innerHTML = data[i].name;
                        movieDescription.setAttribute("class", "movie-description");
                        movieDescription.innerHTML = data[i].description;
                        movieBottom.setAttribute("class", "movie-bottom");

                        movie.appendChild(movieName);
                        movie.appendChild(movieDescription);
                        movie.appendChild(movieBottom);

                        var movieList = document.getElementById("movie-list");
                        movieList.appendChild(movie);
                    }
                    catch (e) {
                        isHasMovieYet = false;
                    }
                }
                isloading = false;
            },
            error: function (data) {
                console.log(data);
            }
        });
    }
}