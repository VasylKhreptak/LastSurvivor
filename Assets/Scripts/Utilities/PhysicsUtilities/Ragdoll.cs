using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entities
{
    public class Ragdoll
    {
        private readonly Preferences _preferences;

        public Ragdoll(Preferences preferences)
        {
            _preferences = preferences;

            _preferences.Colliders.ForEach(c => c.enabled = false);
            _preferences.Rigidbodies.ForEach(r => r.isKinematic = true);
        }

        public void Activate()
        {
            _preferences.Colliders.ForEach(c => c.enabled = true);
            _preferences.Rigidbodies.ForEach(r => r.isKinematic = false);
            _preferences.ScriptsToDisable.ForEach(s => s.enabled = false);
            _preferences.ScriptsToDestroy.ForEach(s => Object.Destroy(s));
            _preferences.CollidersToDisable.ForEach(c => c.enabled = false);
            _preferences.CollidersToDestroy.ForEach(c => Object.Destroy(c));
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _root;
            [SerializeField] private List<Collider> _colliders;
            [SerializeField] private List<Rigidbody> _rigidbodies;
            [SerializeField] private List<Behaviour> _scriptsToDisable;
            [SerializeField] private List<Behaviour> _scriptsToDestroy;
            [SerializeField] private List<Collider> _collidersToDisable;
            [SerializeField] private List<Collider> _collidersToDestroy;

            public IReadOnlyList<Collider> Colliders => _colliders;
            public IReadOnlyList<Rigidbody> Rigidbodies => _rigidbodies;
            public IReadOnlyList<Behaviour> ScriptsToDisable => _scriptsToDisable;
            public IReadOnlyList<Behaviour> ScriptsToDestroy => _scriptsToDestroy;
            public IReadOnlyList<Collider> CollidersToDisable => _collidersToDisable;
            public IReadOnlyList<Collider> CollidersToDestroy => _collidersToDestroy;

            [Button]
            private void FindComponents()
            {
                if (_root == null)
                {
                    Debug.Log("Root is null");
                    return;
                }

                _colliders = new List<Collider>(_root.GetComponentsInChildren<Collider>());
                _rigidbodies = new List<Rigidbody>(_root.GetComponentsInChildren<Rigidbody>());
            }

            [Button]
            private void EnableComponents()
            {
                _colliders.ForEach(c => c.enabled = true);
                _rigidbodies.ForEach(r => r.isKinematic = false);
            }

            [Button]
            private void DisableComponents()
            {
                _colliders.ForEach(c => c.enabled = false);
                _rigidbodies.ForEach(r => r.isKinematic = true);
            }
        }
    }
}