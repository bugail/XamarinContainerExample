namespace ContainerExample.Helpers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using ContainerExample.PageModels;

    using FormsToolkit;

    using FreshMvvm;

    using Xamarin.Forms;

    public class MasterDetailNavigation : Xamarin.Forms.MasterDetailPage, IFreshNavigationService
    {
        private List<Page> _pagesInner = new List<Page>();

        private Dictionary<string, Page> _pages = new Dictionary<string, Page>();

        private ObservableCollection<string> _pageNames = new ObservableCollection<string>();

        public MasterDetailNavigation() : this("MasterDetailNavigation")
        {
        }

        public MasterDetailNavigation(string navigationServiceName)
        {
            this.NavigationServiceName = navigationServiceName;
        }

        public Dictionary<string, Page> Pages => this._pages;

        protected ObservableCollection<string> PageNames => this._pageNames;

        public void Init(string menuTitle, string menuIcon = null)
        {
            this.CreateMenuPage(menuTitle, menuIcon);
            this.RegisterNavigation();
        }

        public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = this.NavigationServiceName;
            this._pagesInner.Add(page);

            var navigationContainer = this.CreateContainerPage(page);
            this._pages.Add(title, navigationContainer);
            this._pageNames.Add(title);

            if (this._pages.Count == 1)
            {
                this.Detail = navigationContainer;
            }
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return this.CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        protected virtual void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, this.NavigationServiceName);
        }

        protected virtual void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            var menuPage = FreshPageModelResolver.ResolvePageModel<MenuPageModel>();

            MessagingService.Current.Subscribe<string>(MessageKeys.NavigationTriggered, (x, args) =>
            {
                if (this._pages.ContainsKey((string)args))
                {
                    this.Detail = this._pages[(string)args];
                }

                this.IsPresented = false;
            });

            this.Master = menuPage;
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
            {
                return this.Navigation.PushModalAsync(this.CreateContainerPageSafe(page));
            }

            return (this.Detail as NavigationPage).PushAsync(page, animate);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
            {
                return this.Navigation.PopModalAsync(animate);
            }

            return (this.Detail as NavigationPage).PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return (this.Detail as NavigationPage).PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            if (this.Master is NavigationPage)
            {
                ((NavigationPage)this.Master).NotifyAllChildrenPopped();
            }

            foreach (var page in this.Pages.Values)
            {
                if (page is NavigationPage)
                {
                    ((NavigationPage)page).NotifyAllChildrenPopped();
                }
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = this._pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            this.Detail = this._pages.Values.ElementAt(tabIndex);

            return Task.FromResult((this.Detail as NavigationPage).CurrentPage.GetModel());
        }
    }
}