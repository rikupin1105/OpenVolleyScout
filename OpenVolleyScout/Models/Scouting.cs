using OpenVolleyScout.Parser;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVolleyScout.Views.Pages;

namespace OpenVolleyScout.Models
{

    public class Scouting : BindableBase
    {
        public ReactivePropertySlim<int> NowSet { get; set; } = new(1);
        public ReactivePropertySlim<int> NowHomeTeamPoint { get; set; } = new(0);
        public ReactivePropertySlim<int> NowAwayTeamPoint { get; set; } = new(0);
        public ReactivePropertySlim<int> NowHomeTeamSet { get; set; } = new(0);
        public ReactivePropertySlim<int> NowAwayTeamSet { get; set; } = new(0);
        public ReactivePropertySlim<bool> isHomeTeamLeft { get; set; } = new(true);
        public ObservableCollection<Player> Players { get; set; } = new();

        public ReactivePropertySlim<string> Input { get; set; } = new();
        //public ObservableCollection<FormBModel> FormB { get; set; } = new();
        public ObservableCollection<Ralies> Rallies { get; set; } = new();
        private void PointAdd(bool isHomeTeam)
        {
            if (isHomeTeam)
            {
                NowHomeTeamPoint.Value++;

                if (NowHomeTeamPoint.Value >= 25)
                {
                    if (NowHomeTeamPoint.Value - NowAwayTeamPoint.Value >= 2)
                    {
                        //セット終了 - ホームチームの勝ち

                        if (NowHomeTeamSet.Value == 3)
                        {
                            //ゲーム終了 - ホームチームの勝ち
                        }
                        else
                        {
                            NowHomeTeamSet.Value++;
                            NowHomeTeamPoint.Value = 0;
                            NowAwayTeamPoint.Value = 0;
                            NowSet.Value++;
                        }
                    }
                }
            }
            else
            {
                NowAwayTeamPoint.Value++;
                if (NowAwayTeamPoint.Value >= 25)
                {
                    if (NowAwayTeamPoint.Value - NowHomeTeamPoint.Value >= 2)
                    {
                        //セット終了 - アウェイチームの勝ち

                        if (NowAwayTeamSet.Value == 3)
                        {
                            //ゲーム終了 - アウェイチームの勝ち
                        }
                        else
                        {
                            NowAwayTeamSet.Value++;
                            NowHomeTeamPoint.Value = 0;
                            NowAwayTeamPoint.Value = 0;
                            NowSet.Value++;
                        }
                    }
                }
            }


        }
        public void Update(string? input, bool? isHomeTeamPoint)
        {
            var rallyId = Guid.NewGuid().ToString();

            if (input is null && isHomeTeamPoint is null)
            {
                return;
            }
            else if (input is null && isHomeTeamPoint is not null)
            {
                //得点のみの入力
                PointAdd((bool)isHomeTeamPoint);
                return;
            }
            else if (input is not null && isHomeTeamPoint is not null)
            {
                PointAdd((bool)isHomeTeamPoint);

                var p = new OpenVolleyScout.Parser.DataVolleyParser();

                var parsed = p.Parse(input!);
                var rally = new Ralies(input, parsed, rallyId)
                {
                    Set = NowSet.Value,
                    HomePoint = NowHomeTeamPoint.Value,
                    AwayPoint = NowAwayTeamPoint.Value
                };

                AddPlayer(parsed);

                //AddFormB(parsed, rallyId);
                Rallies.Add(rally);
            }
            else
            {
                var p = new OpenVolleyScout.Parser.DataVolleyParser();
                var parsed = p.Parse(input!);
                var rally = new Ralies(input, parsed, rallyId)
                {
                    Set = NowSet.Value
                };

                AddPlayer(parsed);

                //AddFormB(parsed, rallyId);
                Rallies.Add(rally);
            }
        }
        public void Undo()
        {
            if (Rallies.Count == 0) return;

            var rallyId = Rallies[Rallies.Count-1].RallyId;
            //RemoveFormB(rallyId);
            UndoPlayer(rallyId);

            Input.Value = Rallies[Rallies.Count-1].Input ??= string.Empty;
            Rallies.RemoveAt(Rallies.Count - 1);
        }
        //private void RemoveFormB(string rallyId)
        //{
        //    var r = Rallies.FirstOrDefault(x => x.RallyId == rallyId);

        //    if (r is not null)
        //    {
        //        foreach (var item in r.Rallies)
        //        {
        //            var player = FormB.Where(x => x.IsHomeTeam == item.IsHomeTeam)
        //                .First(x => x.PlayerNum == item.PlayerNumber);

        //            player.Delete(item);

        //            var i = FormB.IndexOf(player);
        //            FormB[i] = new FormBModel(player);
        //        }
        //    }
        //}

        private void AddPlayer(List<Skill> skills)
        {
            foreach (var skill in skills)
            {
                var player = Players.FirstOrDefault(x => x.PlayerNum == skill.PlayerNumber && x.IsHomeTeam==skill.IsHomeTeam);
                if (player is not null)
                {
                    var i = Players.IndexOf(player);
                    player.AddSkill(skill);

                    Players[i] = player;
                }
                else
                {
                    var p = new Player(skill.IsHomeTeam, skill.PlayerNumber);
                    p.AddSkill(skill);
                    Players.Add(p);
                }
            }
        }
        private void UndoPlayer(string rallyId)
        {
            var r = Rallies.FirstOrDefault(x => x.RallyId == rallyId);

            if (r is null) return;
            if (r.Rallies is null) return;

            foreach (var skill in r.Rallies)
            {
                var player = Players.FirstOrDefault(x => x.PlayerNum == skill.PlayerNumber && x.IsHomeTeam==skill.IsHomeTeam);
                if (player is not null)
                {
                    var i = Players.IndexOf(player);
                    player.RemoveSkill(skill);

                    Players[i] = player;
                }
            }
        }
    }
}

