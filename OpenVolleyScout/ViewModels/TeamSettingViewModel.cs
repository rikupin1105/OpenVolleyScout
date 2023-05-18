using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using Reactive.Bindings.Extensions;
using System.Linq;
using OpenVolleyScout.Models;

namespace OpenVolleyScout.ViewModels
{
	public class TeamSettingViewModel : BindableBase
	{
        public ReactiveProperty<string?> MatchName { get; set; }
        public ReactiveProperty<string> HomeTeamName { get; set; }
        public ReactiveProperty<string> AwayTeamName { get; set; }

        private Scouting _scouting;
        private Game _game;
        public TeamSettingViewModel(Scouting scouting , Game game)
        {
            _scouting = scouting;
            _game = game;

            HomeTeamName = _game.HomeTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
            AwayTeamName = _game.AwayTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);

            MatchName = _game.ToReactivePropertyAsSynchronized(x => x.Name.Value);
        }
	}
}
