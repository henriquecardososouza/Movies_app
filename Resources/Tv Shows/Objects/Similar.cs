﻿namespace Movies_app.Resources.Tv_Shows.Objects
{
    public class Similar
    {
        public class Rootobject
        {
            public int page { get; set; }
            public Result[] results { get; set; }
            public int total_pages { get; set; }
            public int total_results { get; set; }
        }

        public class Result
        {
            public bool adult { get; set; }
            public string backdrop_path { get; set; }
            public int[] genre_ids { get; set; }
            public int id { get; set; }
            public string[] origin_country { get; set; }
            public string original_language { get; set; }
            public string original_name { get; set; }
            public string overview { get; set; }
            public float popularity { get; set; }
            public string poster_path { get; set; }
            public string first_air_date { get; set; }
            public string name { get; set; }
            public float vote_average { get; set; }
            public int vote_count { get; set; }
        }
    }
}
