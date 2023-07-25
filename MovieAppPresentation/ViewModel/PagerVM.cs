namespace MovieAppPresentation.ViewModel
{
    public class PagerVM
    {
        public List<MovieVM> Movies { get; set; }
        public MovieVM Movie { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMovies { get; set; }
        public int TotalPages { get; set; }
    }
}
