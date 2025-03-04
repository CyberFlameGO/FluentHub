using FluentHub.App.Helpers;
using FluentHub.App.Services;
using FluentHub.App.Models;
using FluentHub.App.ViewModels.Repositories.Releases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace FluentHub.App.Views.Repositories.Releases
{
    public sealed partial class ReleasesPage : Page
    {
        public ReleasesPage()
        {
            InitializeComponent();

            var provider = App.Current.Services;
            ViewModel = provider.GetRequiredService<ReleasesViewModel>();
            _navigation = App.Current.Services.GetRequiredService<INavigationService>();

            ViewModel.LatestReleaseDescriptionWebView2 = LatestReleaseWebView2;
        }

        public ReleasesViewModel ViewModel { get; }
        private readonly INavigationService _navigation;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as FrameNavigationArgs;
            _ = param ?? throw new ArgumentNullException("param");

            ViewModel.Login = param.Login;
            ViewModel.Name = param.Name;

            var command = ViewModel.LoadRepositoryReleasesPageCommand;
            if (command.CanExecute(null))
                command.Execute(null);
        }

        private void OnLatestReleaseWebView2Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LatestReleaseDescriptionWebView2 = LatestReleaseWebView2;
        }

        private void OnReleaseBlockButtonClick(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate<ReleasePage>(
                new FrameNavigationArgs()
                {
                    Login = ViewModel.Repository.Owner.Login,
                    Name = ViewModel.Repository.Name,
                    Parameters = new() { $"{(sender as Button).Tag as string}" }
                });
        }
    }
}
