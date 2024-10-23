using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Core.PropertyEditors;

namespace Pixelbuilders.Umbraco.BlockStat.Composers
{
    internal class ManifestComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<ManifestFilter>();
        }

        public class ManifestFilter : IManifestFilter
        {
            private readonly IDataValueEditorFactory _dataValueEditorFactory;

            public ManifestFilter(IDataValueEditorFactory dataValueEditorFactory)
            {
                _dataValueEditorFactory = dataValueEditorFactory;
            }

            public void Filter(List<PackageManifest> manifests)
            {
                manifests.Add(new PackageManifest
                {
                    PackageName = "Pixelbuilders.Umbraco.ElementFinder",
                    AllowPackageTelemetry = true,
                    Scripts =
                    [
                        "/App_Plugins/ElementFinder/ElementFinder.controller.js",
                        "/App_Plugins/ElementFinder/ElementFinder.service.js",
                        "/App_Plugins/ElementFinder/ElementFinder-actions.js",
                    ],
                    Stylesheets =
                    [
                        "/App_Plugins/ElementFinder/ElementFinder.css"
                    ],
                    Version = "1.0.1",
                    Dashboards = [new()
                    {                        
                        Alias = "Block Stat",
                        View = "/App_Plugins/ElementFinder/ElementFinder.html",
                        Sections = ["content"],
                        Weight = 100,
                    },
                    ]
                });
            }
        }
    }
}
