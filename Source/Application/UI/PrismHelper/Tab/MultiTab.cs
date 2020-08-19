using pdfforge.PDFCreator.Core.ServiceLocator;
using pdfforge.PDFCreator.UI.Presentation.Help;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.TabHelper;
using Prism.Events;
using Prism.Regions;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace pdfforge.PDFCreator.UI.PrismHelper.Tab
{
    public class MultiTab<T> : ISettingsTab
        where T : class, ITabViewModel
    {
        private readonly HelpTopic _helpTopic;
        private readonly string _contentRegionName;
        public IList<Type> Views { get; }

        public MultiTab(string contentRegionName, HelpTopic helpTopic, params Type[] views)
        {
            _helpTopic = helpTopic;
            _contentRegionName = contentRegionName;
            Views = views;
        }

        public MultiTab(string contentRegionName, HelpTopic helpTopic, IList<Type> views)
        {
            _helpTopic = helpTopic;
            _contentRegionName = contentRegionName;
            Views = views;
        }

        public void Register(IRegionManager regionManager, IWhitelistedServiceLocator serviceLocator, IEventAggregator eventAggregator, string regionName)
        {
            regionManager.RegisterMultiContentTab<T>(regionName, _contentRegionName, _helpTopic, serviceLocator);
            foreach (var view in Views)
            {
                regionManager.RegisterViewWithRegion(_contentRegionName, view);
            }
        }

        public static void RegisterWithoutMultiTab(IRegionManager regionManager, IEnumerable<Type> views, string RegionName)
        {
        }

        public void RegisterNavigationViews(Container container)
        {
        }
    }
}
