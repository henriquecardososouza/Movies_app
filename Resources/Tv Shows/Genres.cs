namespace Movies_app.Resources.Tv_Shows
{
    public class Genres
    {
        public class Rootobject
        {
            public Genre[] genres { get; set; }
        }

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

    }
}
