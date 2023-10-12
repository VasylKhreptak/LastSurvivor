using System;
using DG.Tweening;

namespace Plugins.Animations.Core
{
    public interface IAnimation
    {
        public float Duration { get; }

        public float Delay { get; }

        public bool IsPlaying { get; }

        public void PlayForward(Action onComplete = null);

        public void PlayBackward(Action onComplete = null);

        public void Stop();

        public void SetStartState();

        public void SetEndState();

        public Tween CreateForwardTween();

        public Tween CreateBackwardTween();
    }
}
