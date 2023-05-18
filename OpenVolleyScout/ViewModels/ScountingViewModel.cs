using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Data;
using System.Linq;
using OpenVolleyScout.Models;

namespace OpenVolleyScout.ViewModels
{
    public class ScountingViewModel : BindableBase
    {
        public ReactiveProperty<int> NowSet { get; set; }
        public ReactiveProperty<int> LeftTeamPoint { get; set; } = new(0);
        public ReactiveProperty<int> RightTeamPoint { get; set; } = new(0);
        public ReactiveProperty<string> LeftTeamHomeorAway { get; set; } = new("AWAY");
        public ReactiveProperty<string> RightTeamHomeorAway { get; set; } = new("Home");


        public ReactiveProperty<string> LeftTeamName { get; set; }
        public ReactiveProperty<string> RightTeamName { get; set; }


        public ReactiveProperty<string> Input { get; set; }

        public ReactiveCommand SubmitCommand { get; set; } = new();
        public ReactiveCommand OpenBracketsCommand { get; set; } = new();
        public ReactiveCommand CloseBracketsCommand { get; set; } = new();
        public ReactiveCommand UndoCommand { get; set; } = new();
        public ReactiveCommand CourtChangeCommand { get; set; } = new();
        public ReactiveCommand LoadedCommand { get; set; } = new();
        public ReactiveCollection<ScoutingDataGrid> ScoutingDataGridCollection { get; set; } = new();

        private bool _isHomeTeamLeft = true;

        private readonly IRegionManager _regionManager;
        private Scouting _scouting;
        private Game _game;
        public ScountingViewModel(Scouting scouting, Game game, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _scouting = scouting;
            _game = game;


            var isHomeTeamLeft = _scouting.ToReactivePropertyAsSynchronized(x => x.isHomeTeamLeft.Value).Subscribe(b =>
            {
                if (b == true)
                {
                    //ホームチームが左側
                    LeftTeamName = _game.HomeTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
                    LeftTeamPoint = _scouting.ToReactivePropertyAsSynchronized(x => x.NowHomeTeamPoint.Value);

                    RightTeamName = _game.AwayTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
                    RightTeamPoint = _scouting.ToReactivePropertyAsSynchronized(x => x.NowAwayTeamPoint.Value);
                }
                else
                {
                    //ホームチームが右側
                    LeftTeamName = _game.AwayTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
                    LeftTeamPoint = _scouting.ToReactivePropertyAsSynchronized(x => x.NowAwayTeamPoint.Value);

                    RightTeamName = _game.HomeTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
                    RightTeamPoint = _scouting.ToReactivePropertyAsSynchronized(x => x.NowHomeTeamPoint.Value);
                }
            });



            Input = _scouting.ToReactivePropertyAsSynchronized(x => x.Input.Value);
            Input.Subscribe(x =>
            {

            });

            NowSet = _scouting.ToReactivePropertyAsSynchronized(x => x.NowSet.Value);

            var Rallies = _scouting.Rallies.ToReadOnlyReactiveCollection();
            Rallies.ObserveAddChanged().Subscribe(x =>
            {
                foreach (var item in x.Rallies)
                {
                    ScoutingDataGridCollection.Add(new ScoutingDataGrid()
                    {
                        ParsedCommand =  item.ToString(),
                        RallyId = x.RallyId,
                        Set = x.Set,
                        Zone = item.StartZone + " → " + item.EndZone+item.EndZonePlus,
                    });
                }
            });
            Rallies.ObserveRemoveChanged().Subscribe(x =>
            {
                var c = ScoutingDataGridCollection.Where(r => r.RallyId == x.RallyId).ToList();

                foreach (var item in c)
                {
                    ScoutingDataGridCollection.Remove(item);
                }
            });

            RightTeamName = _game.HomeTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);
            LeftTeamName = _game.AwayTeam.ToReactivePropertyAsSynchronized(x => x.Name.Value);

            LoadedCommand.Subscribe(_ => _regionManager.RequestNavigate("Main", "FormB"));
            UndoCommand.Subscribe(_ => _scouting.Undo());
            SubmitCommand.Subscribe(parameter => Submit(parameter as string));
            OpenBracketsCommand.Subscribe(parameter => Submit(parameter as string, true));
            CloseBracketsCommand.Subscribe(parameter => Submit(parameter as string, false));
            CourtChangeCommand.Subscribe(_ =>
            {
                _isHomeTeamLeft = !_isHomeTeamLeft;
                (LeftTeamName.Value, RightTeamName.Value) = (RightTeamName.Value, LeftTeamName.Value);
                (LeftTeamPoint.Value, RightTeamPoint.Value) = (RightTeamPoint.Value, LeftTeamPoint.Value);
                (LeftTeamHomeorAway.Value, RightTeamHomeorAway.Value) = (RightTeamHomeorAway.Value, LeftTeamHomeorAway.Value);
            });
        }
        public void OpenBrackets(string? str)
        {
            //左のチーム
            Submit(str, true);
        }
        public void CloseBrackets(string? str)
        {
            //右のチーム
            Submit(str, false);
        }
        public void Submit(string? str, bool? isLeftTeamPoint = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                if (isLeftTeamPoint is not null)
                {
                    _scouting.Update(null, isLeftTeamPoint);
                }
                return;
            }

            try
            {
                _scouting.Update(str, isLeftTeamPoint);
            }
            catch (Exception)
            {
                return;
            }

            Input.Value = string.Empty;
            Input.ForceNotify();
        }
    }
}
