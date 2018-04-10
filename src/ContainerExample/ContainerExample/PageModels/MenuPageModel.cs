namespace ContainerExample.PageModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ContainerExample.Helpers;

    using FormsToolkit;

    using FreshMvvm;

    using Xamarin.Forms;

    public class MenuPageModel : FreshBasePageModel
    {
        public ICommand NavigateCommand { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        public MenuItem SelectedItem { get; set; }

        public MenuPageModel()
        {
            this.NavigateCommand = new Command(this.NavigateToPageModel);

            this.MenuItems = new List<MenuItem>();

            this.MenuItems.Add(new MenuItem() { Value = "Page 1", Label = "Page 1", Image = "icon.png" });
            this.MenuItems.Add(new MenuItem() { Value = "Page 2", Label = "Page 2", Image = "icon.png" });
            this.MenuItems.Add(new MenuItem() { Value = "Page 3", Label = "Page 3", Image = "icon.png" });
            this.MenuItems.Add(new MenuItem() { Value = "Page 4", Label = "Page 4", Image = "icon.png" });
            this.MenuItems.Add(new MenuItem() { Value = "Page 5", Label = "Page 5", Image = "icon.png" });
        }

        void NavigateToPageModel()
        {
            MessagingService.Current.SendMessage<string>(MessageKeys.NavigationTriggered, this.SelectedItem.Value);
        }
    }
}