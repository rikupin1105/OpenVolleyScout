using Prism.Mvvm;
using Reactive.Bindings;
using System;

namespace OpenVolleyScout.Models
{
    public class Game : BindableBase
    {
        /// <summary>
        /// 試合ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 試合名
        /// </summary>
        public ReactivePropertySlim<string?> Name { get; set; } = new(null);
        public DateTime? DateTime { get; set; }

        public Team HomeTeam { get; set; } = new();
        public Team AwayTeam { get; set; } = new();
    }
}

