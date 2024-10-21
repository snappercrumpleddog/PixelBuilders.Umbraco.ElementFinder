using ElementFinder.Models;
using MyProject.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace ElementFinder.Controllers
{
    [PluginController("ElementFinder")]
    public class DataController : UmbracoApiController
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;
        private readonly IScopeProvider _scopeProvider;
        private readonly IAppPolicyCache _runtimeCache;

        private readonly IPublishedContentQuery _publishedContentQuery;
        public DataController(IContentTypeService contentTypeService, IContentService contentService, IPublishedContentQuery publishedContentQuery, IScopeProvider scopeProvider, AppCaches appCaches)
        {
            _contentTypeService = contentTypeService;
            _contentService = contentService;
            _publishedContentQuery = publishedContentQuery;
            _scopeProvider = scopeProvider;

            _runtimeCache = appCaches.RuntimeCache;
        }

        public IEnumerable<Element> GetElements()
        {
            var documentTypes = _contentTypeService.GetAll();

            return documentTypes.Where(x => x.IsElement)
                .OrderBy(o => o.Name)
                .Select(s => new Element()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Alias = s.Alias,
                    Key = s.Key,
                });

        }

        public SearchResults GetLocationsOfElement(string elementGuid, int currentPage, int totalPages)
        {
            var searchResults = new SearchResults();

            var elements = _runtimeCache.GetCacheItem(
            $"GetLocation_{elementGuid}",
            () => GetLocations(elementGuid, currentPage, totalPages),
            TimeSpan.FromMinutes(1));

            if (elements != null && elements.Pages.Any())
            {
                searchResults.Pages = elements.Pages.Skip(totalPages * (currentPage - 1)).Take(totalPages);
                searchResults.PageCount = elements.PageCount;
                searchResults.TotalCount = elements.TotalCount;
            }

            return searchResults;
        }

        private SearchResults GetLocations(string elementGuid, int currentPage, int totalPages)
        {
            var roots = _publishedContentQuery.ContentAtRoot();

            var searchResults = new SearchResults();

            // need to cache this result really.
            var pagesWithElement = new List<PageDetails>();

            foreach (var root in roots)
            {
                CheckElementExistsOnPage(root, elementGuid, ref pagesWithElement);
            }

            searchResults.TotalCount = pagesWithElement.Count;
            searchResults.Pages = pagesWithElement;
            searchResults.PageCount = pagesWithElement.Count / totalPages + 1;

            return searchResults;
        }


        // RECURSIVE FUNCTION ADD PAGE NAME TO LIST IF ELEMENT EXISTS IN A PROPERTY.
        private void CheckElementExistsOnPage(IPublishedContent content, string elementGuid, ref List<PageDetails> pagesWithElement)
        {
            bool foundOnPage = false;
            foreach (var property in content.Properties)
            {
                var value = property.GetSourceValue()?.ToString();
                if (value != null && !string.IsNullOrEmpty(value) && value.Contains(elementGuid))
                {
                    var pageDetails = CreatePageDetailsFromIPublishedContent(content);

                    if (pageDetails != null && !pagesWithElement.Where(e => e.Name.Equals(pageDetails.Name) && e.TreePath.Equals(pageDetails.TreePath) && e.Url.Equals(pageDetails.Url)).Any())
                    {
                        foundOnPage = true;
                        pagesWithElement.Add(pageDetails);
                        break;
                    }
                }
            }
            foreach (var child in content.Descendants())
            {
                CheckElementExistsOnPage(child, elementGuid, ref pagesWithElement);
            }
        }


        private PageDetails? CreatePageDetailsFromIPublishedContent(IPublishedContent content)
        {
            if(content == null) return null;

            return new PageDetails() {
                Name = content.Name ?? "unkown page",
                Url = content.Url(),
                TreePath = CreateTreePath(content),
                PageId = content.Id.ToString()
            };
        }

        private string CreateTreePath(IPublishedContent content)
        {
            List<string> treePath = new List<string>();
            treePath.Add(content.Name ?? "unknown page");
            var parent = content.Parent;
            while (parent != null)
            {
                treePath.Add(parent.Name ?? "unkown page");
                parent = parent.Parent;
            }
            treePath.Reverse();

            return string.Join("/", treePath);
        }
    }   
}
