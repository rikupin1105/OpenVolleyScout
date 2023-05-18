using Prism.Mvvm;
using Reactive.Bindings;

namespace OpenVolleyScout.Models
{
    public class Team : BindableBase
    {
        public ReactivePropertySlim<string> Name { get; set; } = new();
    }
}

