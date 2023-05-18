using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenVolleyScout.Models;
using OpenVolleyScout.Views;

namespace OpenVolleyScout.ViewModels
{
    public class FormBViewModel : BindableBase
    {
        public ReactiveCollection<FormBModel> FormBModelsHome { get; set; } = new();
        public ReactiveCollection<FormBModel> FormBModelsAway { get; set; } = new();
        private Scouting _scouting;
        public FormBViewModel(Scouting scouting)
        {
            _scouting = scouting;

            var players = _scouting.Players.ToReadOnlyReactiveCollection();
            players.ObserveAddChanged().Subscribe(p =>
            {
                if(p.IsHomeTeam)
                {
                    FormBModelsHome.Add(p.ConvertFormB());
                }
                else
                {
                    FormBModelsAway.Add(p.ConvertFormB());
                }
            });

            players.ObserveReplaceChanged().Subscribe(p =>
            {
                if(p.OldItem.IsHomeTeam)
                {
                    var i = FormBModelsHome.IndexOf(FormBModelsHome.First(x => x.PlayerNum==p.OldItem.PlayerNum));
                    FormBModelsHome[i] = p.NewItem.ConvertFormB();
                }
                else
                {
                    var i = FormBModelsAway.IndexOf(FormBModelsAway.First(x => x.PlayerNum==p.OldItem.PlayerNum));
                    FormBModelsAway[i] = p.NewItem.ConvertFormB();
                }
            });

            //var FormB = _scouting.FormB.ToReadOnlyReactiveCollection();
            //FormB.ObserveReplaceChanged().Subscribe(x =>
            //{
            //    if (x.OldItem.IsHomeTeam)
            //    {
            //        var s = FormBModelsHome.IndexOf(FormBModelsHome.First(p => p.PlayerNum == x.OldItem.PlayerNum));
            //        FormBModelsHome[s] = x.NewItem;
            //    }
            //    else
            //    {
            //        var s = FormBModelsAway.IndexOf(FormBModelsAway.First(p => p.PlayerNum == x.OldItem.PlayerNum));
            //        FormBModelsAway[s] = x.NewItem;
            //    }
            //});

            //FormB.ObserveAddChanged().Subscribe(x =>
            //{
            //    if (x.IsHomeTeam)
            //    {
            //        FormBModelsHome.Add(x);
            //        FormBModelsHome.OrderByDescending(x => x.PlayerNum);
            //    }
            //    else
            //        FormBModelsAway.Add(x);

            //});
        }
    }
}
