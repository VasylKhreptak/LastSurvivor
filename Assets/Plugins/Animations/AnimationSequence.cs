using System;
using DG.Tweening;
using Plugins.Animations.Core;

namespace Plugins.Animations
{
    public class AnimationSequence : IAnimation
    {
        private readonly IAnimation[] _animations;

        public AnimationSequence(params IAnimation[] animations)
        {
            _animations = animations;
        }

        private Tween _tween;

        public float Duration => CreateForwardTween().Duration();

        public float Delay { get; private set; }

        public bool IsPlaying => _tween != null && _tween.IsPlaying();

        public void PlayForward(Action onComplete = null)
        {
            Stop();
            _tween = CreateForwardTween().OnComplete(() => onComplete?.Invoke()).Play();
        }

        public void PlayBackward(Action onComplete = null)
        {
            Stop();
            _tween = CreateBackwardTween().OnComplete(() => onComplete?.Invoke()).Play();
        }

        public void Stop() => _tween.Kill();

        public void SetStartState()
        {
            Stop();

            foreach (IAnimation animation in _animations)
            {
                animation.SetStartState();
            }
        }

        public void SetEndState()
        {
            Stop();

            foreach (IAnimation animation in _animations)
            {
                animation.SetEndState();
            }
        }

        public Tween CreateForwardTween()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(Delay);

            foreach (IAnimation animation in _animations)
            {
                sequence.Append(animation.CreateForwardTween());
            }

            return sequence;
        }

        public Tween CreateBackwardTween()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(Delay);

            for (int i = 0; i < _animations.Length; i++)
            {
                if (i == 0)
                    sequence.Append(_animations[i].CreateBackwardTween());
                else
                    sequence.Join(_animations[i].CreateBackwardTween());
            }

            return sequence;
        }

        public IAnimation SetDelay(float delay)
        {
            Delay = delay;

            return this;
        }
    }
}