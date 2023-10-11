using System;
using System.Collections.Generic;
using Infrastructure.Data.Core;
using Infrastructure.Services.SaveLoadHandler.Core;
using UniRx;
using Zenject;

namespace Infrastructure.Services.SaveLoadHandler
{
    public class SaveLoadHandlerService : ISaveLoadHandlerService, IInitializable, IDisposable
    {
        private readonly List<ISaveHandler> _saveHandlers = new List<ISaveHandler>();
        private readonly List<ILoadHandler> _loadHandlers = new List<ILoadHandler>();

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        public void Save()
        {
            foreach (var saveHandler in _saveHandlers)
            {
                saveHandler.Save();
            }
        }

        public void Load()
        {
            foreach (var loadHandler in _loadHandlers)
            {
                loadHandler.Load();
            }
        }

        public void AddSaveHandler<T>(T t) where T : ISaveHandler => _saveHandlers.Add(t);

        public void RemoveSaveHandler<T>(T t) where T : ISaveHandler => _saveHandlers.Remove(t);

        public void AddLoadHandler<T>(T t) where T : ILoadHandler => _loadHandlers.Add(t);

        public void RemoveLoadHandler<T>(T t) where T : ILoadHandler => _loadHandlers.Remove(t);

        public void Add<T>(T t) where T : ISaveLoadHandler
        {
            _saveHandlers.Add(t);
            _loadHandlers.Add(t);
        }

        public void Remove<T>(T t) where T : ISaveLoadHandler
        {
            _saveHandlers.Remove(t);
            _loadHandlers.Remove(t);
        }

        public void Initialize()
        {
            StartObserving();
        }

        public void Dispose()
        {
            Save();
            StopObserving();
        }

        private void StartObserving()
        {
            StopObserving();

            Observable
                .EveryApplicationFocus()
                .Subscribe(OnApplicationFocus)
                .AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
            {
                Save();
            }
        }
    }
}
