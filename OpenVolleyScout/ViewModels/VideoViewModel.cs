using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using OpenVolleyScout.Models;

namespace OpenVolleyScout.ViewModels
{
    public class VideoViewModel : BindableBase
    {
        public ReactiveProperty<string> MediaPath { get; set; } = new();
        public ReactiveProperty<MediaElement> MediaElementObj { get; set; } = new();
        public ReactiveCommand PlayCommand { get; set; } = new();
        public ReactiveCommand PauseCommand { get; set; } = new();
        public ReactiveCommand MoveFowardCommand { get; set; } = new();
        public ReactiveCommand MoveBackCommand { get; set; } = new();
        public ReactiveCommand MediaLoadCommand { get; set; } = new();
        public ReactiveProperty<bool> IsMute { get; set; } = new(false);

        public VideoViewModel()
        {
            var video = new Video();

            MediaLoadCommand.Subscribe(_ => video.Create(MediaPath.Value));
            PlayCommand.Subscribe(_ => video.Play());
            PauseCommand.Subscribe(_ => video.Pause());
            MoveFowardCommand.Subscribe(_ => video.MoveForward(5000));
            MoveBackCommand.Subscribe(_ => video.MoveBack(5000));

            IsMute.Subscribe(x => video.SwitchMute(x));

            MediaElementObj = video.ToReactivePropertyAsSynchronized(x => x.MediaElementObj);
        }
    }
}
