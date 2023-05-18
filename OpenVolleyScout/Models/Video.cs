using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Windows.Controls;

namespace OpenVolleyScout.Models
{
    public class Video : BindableBase
    {
        public Video()
        {
            MediaElementObj = new MediaElement();
            MediaElementObj.LoadedBehavior = MediaState.Manual;
            MediaElementObj.UnloadedBehavior = MediaState.Manual;
        }
        public void Create(string videoPath)
        {
            MediaElementObj.Source =new Uri(videoPath);
        }
        public bool IsMute { get; set; } = false;
        public MediaElement MediaElementObj { get; set; }
        public void Play()
        {
            if (MediaElementObj is null) return;
            MediaElementObj.Play();
        }
        public void Pause()
        {
            if (MediaElementObj is null) return;
            MediaElementObj.Pause();
        }
        public void MoveForward(double MillSeconds)
        {
            if (MediaElementObj is null) return;
            MediaElementObj.Position = (TimeSpan)(GetVideoTime()! + TimeSpan.FromMilliseconds(MillSeconds));
        }
        public void MoveBack(double MillSeconds)
        {
            if (MediaElementObj is null) return;
            MediaElementObj.Position = (TimeSpan)(GetVideoTime()! - TimeSpan.FromMilliseconds(MillSeconds));
        }
        public TimeSpan? GetVideoTime()
        {
            if (MediaElementObj is null) return null;
            return MediaElementObj.Position;
        }
        public void SwitchMute(bool b)
        {
            if (MediaElementObj is null) return;
            MediaElementObj.IsMuted = b;
        }
    }
}

