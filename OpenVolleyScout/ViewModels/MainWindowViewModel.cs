using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using OpenVolleyScout.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;

namespace OpenVolleyScout.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public ReactiveProperty<string> SSS { get; set; } = new("Hello");
        public ReactiveProperty<string> Title { get; set; } = new();
        public ReactiveCommand LoadedCommand { get; set; } = new();
        public ReactiveCommand JvimsCommand { get; set; } = new();
        public ReactiveCommand TeamSettingCommand { get; set; } = new();
        public ReactiveCommand AnalyzeCommand { get; set; } = new();
        public ReactiveCommand VideoCommand { get; set; } = new();

        private readonly Game _game;
        public MainWindowViewModel(IRegionManager regionManager , Game game)
        {
            _game = game;

            _regionManager = regionManager;
            LoadedCommand.Subscribe(_ => _regionManager.RequestNavigate("ContentRegion", source: "Scounting"));
            JvimsCommand.Subscribe(_ => _regionManager.RequestNavigate("Main","FormB"));
            TeamSettingCommand.Subscribe(_ => _regionManager.RequestNavigate("Main", "TeamSetting"));
            AnalyzeCommand.Subscribe(_ => _regionManager.RequestNavigate("Main", "Analyze"));
            VideoCommand.Subscribe(_ => _regionManager.RequestNavigate("Main", "Video"));

            _game.ToReactivePropertyAsSynchronized(x => x.Name.Value).Subscribe(x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    Title.Value = "Scouting - "+x;
                }
                else
                {
                    Title.Value = "Scouting";
                }
            });

        }
    }
}
