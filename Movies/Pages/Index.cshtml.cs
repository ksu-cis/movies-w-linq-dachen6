using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Movie> Movies;

        [BindProperty]
        public string search { get; set; }

        [BindProperty]
        public List<string> mpaa { get; set; } = new List<string>();

        [BindProperty]
        public float? minIMDB { get; set; }

        [BindProperty]
        public float? maxIMDB { get; set; }

        [BindProperty]
        public string sort { get; set; }

        public void OnGet()
        {
            Movies = MovieDatabase.All;
        }

        public void OnPost()
        {

            Movies = MovieDatabase.All;
            Movies = Movies.OrderBy(Movie => Movie.Title);

            if (search != null)
            {
                Movies = Movies.Where(Movie => Movie.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
                // Movies = MovieDatabase.Search(Movies, search);
            }

            if(mpaa.Count != 0)
            {
                Movies = Movies.Where(Movie => mpaa.Contains(Movie.MPAA_Rating));
                //Movies = MovieDatabase.FilterByMPAA(Movies, mpaa);
            }

            if(minIMDB != null)
            {
                Movies = Movies.Where(Movie => Movie.IMDB_Rating != null && Movie.IMDB_Rating >= minIMDB);
                //Movies = MovieDatabase.FilterByMinIMDB(Movies, (float)minIMDB);
            }
            if (maxIMDB != null)
            {
                Movies = Movies.Where(Movie => Movie.IMDB_Rating != null && Movie.IMDB_Rating <= maxIMDB);
                // Movies = MovieDatabase.FilterByMaxIMDB(Movies, (float)maxIMDB);
            }
            switch (sort)
            {
                case "title":
                    Movies = Movies.OrderBy(Movie => Movie.Title);
                    break;
                case "director":
                    Movies = Movies
                        .Where(Movie => Movie.Director != null)
                        .OrderBy(Movie =>
                        {
                            string[] parts = Movie.Director.Split(" ");
                            return parts[parts.Length - 1];
                        }

                        );
                    break;
                case "mapp":
                    Movies = Movies.OrderBy(Movie => Movie.MPAA_Rating);
                    break;
                case "imdb":
                    Movies = Movies.OrderBy(Movie => Movie.IMDB_Rating);
                    break;
                case "rt":
                    Movies = Movies
                        .Where(Movie => Movie.Rotten_Tomatoes_Rating != null)
                        .OrderBy(Movie => Movie.Rotten_Tomatoes_Rating);
                    break;
            }

        }
    }
}
