using Prism.Ioc;
using OpenVolleyScout.Views;
using Prism.Unity;
using System.Windows;
using OpenVolleyScout.Views.Pages;
using OpenVolleyScout.ViewModels;

namespace OpenVolleyScout
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();

            containerRegistry.RegisterForNavigation<Scounting, ScountingViewModel>();
            containerRegistry.RegisterForNavigation<FormB, FormBViewModel>();
            containerRegistry.RegisterForNavigation<TeamSetting, TeamSettingViewModel>();
            containerRegistry.RegisterForNavigation<Analyze, AnalyzeViewModel>();
            containerRegistry.RegisterForNavigation<Video, VideoViewModel>();

            containerRegistry.RegisterSingleton<Models.Scouting>();
            containerRegistry.RegisterSingleton<Models.Video>();
            containerRegistry.RegisterSingleton<Models.Game>();
        }
    }
}