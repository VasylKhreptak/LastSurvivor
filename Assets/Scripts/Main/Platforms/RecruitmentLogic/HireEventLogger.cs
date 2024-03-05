using System;
using Analytics;
using Firebase.Analytics;
using UnityEngine;
using Zenject;

namespace Main.Platforms.RecruitmentLogic
{
    public class HireEventLogger : IInitializable, IDisposable
    {
        private readonly EntityRecruiter _recruiter;
        private readonly string _entityName;

        public HireEventLogger(EntityRecruiter recruiter, string entityName)
        {
            _recruiter = recruiter;
            _entityName = entityName;
        }

        public void Initialize() => _recruiter.OnHired += OnHiredEntity;

        public void Dispose() => _recruiter.OnHired += OnHiredEntity;

        private void OnHiredEntity(GameObject entity) => LogEvent();

        private void LogEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.HiredEntity, new Parameter(AnalyticParameters.Name, _entityName));
    }
}