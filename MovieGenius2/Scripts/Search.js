

var apikey = "863b8b07d72a6a0fdefb3c70fb492353";

//todo move api key out of js file

//todo fix autocomplete styling


$(document).ready(function () {
    var url = "https://api.themoviedb.org/3/search/movie?api_key=863b8b07d72a6a0fdefb3c70fb492353&language=en-US&page=1&include_adult=false&query=";//https://api.themoviedb.org/3/search/multi?api_key=863b8b07d72a6a0fdefb3c70fb492353&language=en-US&page=1&include_adult=false&query=";
    var searchField = $('[id$=searchText]');
    searchField.autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url + request.term,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.results, function (movie) {
                        return {
                            label: movie.title,
                            thumb: movie.poster_path,
                            id: movie.id,
                            year: movie.release_date,
                            rating: movie.vote_average
                        }
                    }));
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        },
        minLength: 2,

        open: function () {
            $(this).autocomplete("widget").css({
                "width": 350
            });
        }
    });
    searchField.data("ui-autocomplete")._renderMenu = function (ul, items) {
        var that = this;
        

        $.each(items, function (index, item) {
            that._renderItemData(ul, item);

        });
    }

    searchField.data("ui-autocomplete")._renderItem = function (ul, item) {
        var li = $("<li>");
        var year = item.year.substr(0, 4);

        if (item.label) {
            var a = $('<a class="autocompleteList" style="width:335px">').attr('href', "/Home/Details/" + item.id );

            if (item.thumb) {
                $('<img width=\"34\" height=\"50\">').addClass("thumb").attr('src', "http://image.tmdb.org/t/p/w92" + item.thumb).appendTo(a);


            }
            if (item.rating) {
                $('<span>').addClass('rating').text(item.rating).appendTo(a);
            }

            $('<div style="clear:both">').addClass("searchcenter").appendTo(a);

            if (item.label) {
                $('<span>').addClass('searchtitle').text(item.label + " (" + year + ")").appendTo(a);
                $('<p>').appendTo(a);
            }

            $('</br>').appendTo(a);

           

            $('</div>').appendTo(a);

            a.appendTo(li);
        }
        else {
            $('<a>').html(item.label).appendTo(li);
        }
        return li.appendTo(ul);
    };

});