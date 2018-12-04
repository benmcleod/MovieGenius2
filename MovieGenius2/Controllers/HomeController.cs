using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;


namespace MovieGenius2
{
    public class HomeController : Controller
    {
        MovieService movieService = new MovieService();
                    

        public ActionResult Index(int index = 1)
        {

            RootObject rootObject = movieService.FindMoviesInTheaterList(index, WebConfigurationManager.AppSettings["TMDBApiKey"]);

            for (int i = 0; i < rootObject.movies.Count; i++)
            { 
                //shortens the overview so it fits one the screen
                rootObject.movies[i].overview = rootObject.movies[i].overview.Split('.').First();

                //adds the path to the poster location, or inserts a default poster
                rootObject.movies[i].poster_path = (rootObject.movies[i].poster_path == null)
                    ? "~/Images/default-movie.png"
                    : "http://image.tmdb.org/t/p/w185" + rootObject.movies[i].poster_path;
            }

            //paging
            ViewBag.index = index;
            if (index < 5)
            {
                ViewBag.startPages = 1;
                ViewBag.endPages = 10;
            }
            else if (rootObject.total_pages - index < 5)
            {
                ViewBag.startPages = rootObject.total_pages - 9;
                ViewBag.endPages = rootObject.total_pages;
            }
            else
            {
                ViewBag.startPages = (index - 5 >= 1) ? index - 5 : 1;
                ViewBag.endPages = (index + 5 <= rootObject.total_pages) ? index + 4 : rootObject.total_pages;
            }
            return View(rootObject.movies);
        }
        

        public ActionResult Menu()
        {
            RootObject rootObject = movieService.FindUpcomingMoviesList(1, WebConfigurationManager.AppSettings["TMDBApiKey"]);

            return PartialView(rootObject.movies.Take(10));
        }


        public ActionResult Details (int id)
        {
            Movie rootObject = movieService.MovieInfo(id, WebConfigurationManager.AppSettings["TMDBApiKey"]);

            // error handling for missing data

            rootObject.poster_path = (rootObject.poster_path != null) 
                ? "http://image.tmdb.org/t/p/w185" + rootObject.poster_path 
                : "~/Images/default-movie.png";

            if (rootObject.videos.results.Count() > 0)
                rootObject.key = "https://www.youtube.com/embed/" + rootObject.videos.results[0].key;

            if (rootObject.genres.Count() > 0)
                rootObject.genres[0].name = string.Join(", ", rootObject.genres.Select(x => x.name));
            else
                rootObject.genres.Add(new Genre() { name = "" });

            if (rootObject.credits.cast.Count() > 0)
                rootObject.credits.cast[0].name = string.Join(", ", rootObject.credits.cast.Select(x => x.name).Take(16).ToList());
            else
                rootObject.credits.cast.Add(new Cast() { name = "" });

            if (rootObject.credits.crew.Count() > 0)
                rootObject.credits.crew[0].name = string.Join(", ", rootObject.credits.crew.FindAll(x => x.job.Equals("Director")).Select(x => x.name).ToList());
            else
                rootObject.credits.crew.Add(new Crew() { name = "" });

            return View(rootObject);
        }


        public ActionResult Slider()
        {
            RootObject rootObject = movieService.FindTopRatedMovies(1, WebConfigurationManager.AppSettings["TMDBApiKey"]);

            return PartialView(rootObject.movies.Take(10));
        }


        // todo search page
        public ActionResult SearchBar()
        {
            return PartialView();
        }


        public ActionResult About()
        {
            return View();
        }

    }
}