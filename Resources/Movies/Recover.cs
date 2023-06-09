using Movies_app.Resources.Movies.Objects;
using Newtonsoft.Json;
using System.Net;

namespace Movies_app.Resources.Movies
{
    public static class Recover
    {
        public static Movie.Rootobject? getMovie(int id)
        {
            string url = string.Format(Configuration.urlBase, id, Configuration.apiKey, Configuration.language);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Movie.Rootobject>(json);
            }
        }

        public static Search.Rootobject? getSearch(string query, int page)
        {
            string url = string.Format(Configuration.searchUrlBase, Configuration.apiKey, query, Configuration.language, page);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Search.Rootobject>(json);
            }
        }

        public static Popular.Rootobject? getPopular(string region, int page)
        {
            string url = string.Format(Configuration.popularUrlBase, Configuration.apiKey, Configuration.language, region, page);
            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Popular.Rootobject>(json);
            }
        }

        public static Similar.Rootobject? getSimilar(int id, int page)
        {
            string url = string.Format(Configuration.similarUrlBase, id, Configuration.apiKey, Configuration.language, page);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Similar.Rootobject>(json);
            }
        }

        public static Upcoming.Rootobject? getUpcoming(string region, int page)
        {
            string url = string.Format(Configuration.upcomingUrlBase, Configuration.apiKey, Configuration.language, region, page);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Upcoming.Rootobject>(json);
            }
        }

        public static NowPlaying.Rootobject? getNowPlaying(string region, int page)
        {
            string url = string.Format(Configuration.nowPlayingUrlBase, Configuration.apiKey, Configuration.language, region, page);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<NowPlaying.Rootobject>(json);
            }
        }

        public static Genre.Rootobject? getGenre(int page, int id)
        {
            string url = string.Format(Configuration.genreUrlBase, Configuration.apiKey, Configuration.language, page, id);

            using (WebClient web = new())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<Genre.Rootobject>(json);
            }
        }
    }
}
