using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using TMDBClassLibrary.Service;

namespace MovieGenius2.Controllers
{
    public class HomeController : Controller
    {
        MovieService movieService = new MovieService();
        public ActionResult Index()
        {
            RootObject rootObject = movieService.FindMoviesInTheaterList(1, WebConfigurationManager.AppSettings["TMDBApiKey"]);

            for (int i = 0; i < rootObject.movies.Count; i++)
            { 
                rootObject.movies[i].overview = rootObject.movies[i].overview.Split('.').First();
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

            rootObject.backdrop_path = "http://image.tmdb.org/t/p/w1280" + rootObject.backdrop_path;

            rootObject.key = "https://www.youtube.com/embed/" + rootObject.videos.results[0].key;

            rootObject.genres[0].name = string.Join(", ", rootObject.genres.Select(x => x.name));

            rootObject.credits.cast[0].name = string.Join(", ", rootObject.credits.cast.Select(x => x.name).ToList());

            rootObject.credits.crew[0].name = string.Join(", ", rootObject.credits.crew.FindAll(x => x.job.Equals("Director")).Select(x => x.name).ToList());

            return View(rootObject);
        }
    }
}