using System;
using Audio.Players;
using Plugins.AudioService.Core;
using UnityEngine;
using Zenject;

namespace Main.Platforms.RecruitmentLogic
{
    public class HireAudioPlayer : IInitializable, IDisposable
    {
        private readonly EntityRecruiter _recruiter;
        private readonly AudioPlayer _audioPlayer;

        public HireAudioPlayer(IAudioService audioService, EntityRecruiter recruiter, AudioPlayer.Preferences preferences)
        {
            _recruiter = recruiter;
            _audioPlayer = new AudioPlayer(audioService, preferences);
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _recruiter.OnHired += OnRecruited;

        private void StopObserving() => _recruiter.OnHired -= OnRecruited;

        private void OnRecruited(GameObject entity) => _audioPlayer.Play(entity.transform.position);
    }
}