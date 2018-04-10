namespace ContainerExample
{
    using ContainerExample.Helpers;
    using ContainerExample.PageModels;

    using FreshMvvm;

    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            this.HandlePlatformLogic();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void HandlePlatformLogic()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.HandleIOSSetup();
                    break;

                case Device.Android:
                    this.HandleAndroidSetup();
                    break;

                case Device.UWP:
                    this.HandleAndroidSetup();
                    break;

                default:
                    break;
            }
        }

        private void HandleIOSSetup()
        {
            var tabs = new FreshTabbedNavigationContainer("RunkeeperTabs");

            tabs.AddTab<Page1ViewModel>("Page 1", "icon.png");
            tabs.AddTab<Page2ViewModel>("Page 2", "icon.png");
            tabs.AddTab<Page3ViewModel>("Page 3", "icon.png");
            tabs.AddTab<Page4ViewModel>("Page 4", "icon.png");
            tabs.AddTab<Page5ViewModel>("Page 5", "icon.png");

            // Set the selected tab to the middle one.
            tabs.SwitchSelectedRootPageModel<Page3ViewModel>();

            MainPage = tabs;
        }

        private void HandleAndroidSetup()
        {
            var navContainer = new MasterDetailNavigation();

            navContainer.Init("Menu", "hamburger.png");
            navContainer.AddPage<Page1ViewModel>("Page 1");
            navContainer.AddPage<Page2ViewModel>("Page 2");
            navContainer.AddPage<Page3ViewModel>("Page 3");
            navContainer.AddPage<Page4ViewModel>("Page 4");
            navContainer.AddPage<Page5ViewModel>("Page 5");

            MainPage = navContainer;
        }
    }
}
