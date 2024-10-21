namespace ElementFinder.Models
{
    public class SearchResults
    {
        public IEnumerable<PageDetails> Pages { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class PageDetails
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? TreePath { get; set; }
        public string? PageId { get; set; }
    }
}
