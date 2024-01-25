using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject.Infrastructure.Toggleable.Core;
using Object = UnityEngine.Object;

namespace Utilities.PhysicsUtilities
{
    public class Ragdoll : IEnableable, IDisableable
    {
        private readonly Preferences _preferences;

        public Ragdoll(Preferences preferences)
        {
            _preferences = preferences;
        }

        public void Enable()
        {
            _preferences.Colliders.ForEach(c => c.enabled = true);
            _preferences.Rigidbodies.ForEach(r => r.isKinematic = false);
        }

        public void Disable()
        {
            _preferences.Colliders.ForEach(c => c.enabled = false);
            _preferences.Rigidbodies.ForEach(r => r.isKinematic = true);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _root;
            [SerializeField] private List<Collider> _colliders;
            [SerializeField] private List<Rigidbody> _rigidbodies;
            [SerializeField] private List<Joint> _joints;

            public IReadOnlyList<Collider> Colliders => _colliders;
            public IReadOnlyList<Rigidbody> Rigidbodies => _rigidbodies;

            [Button]
            private void FindComponents()
            {
                if (_root == null)
                {
                    Debug.Log("Root is null");
                    return;
                }

                _colliders = _root.GetComponentsInChildren<Collider>().ToList();
                _rigidbodies = _root.GetComponentsInChildren<Rigidbody>().ToList();
                _joints = _root.GetComponentsInChildren<Joint>().ToList();
            }

            [Button]
            private void Enable()
            {
                _colliders.ForEach(c => c.enabled = true);
                _rigidbodies.ForEach(r => r.isKinematic = false);
            }

            [Button]
            private void Disable()
            {
                _colliders.ForEach(c => c.enabled = false);
                _rigidbodies.ForEach(r => r.isKinematic = true);
            }

            [Button]
            private void Destroy()
            {
                _colliders.ForEach(Object.DestroyImmediate);
                _rigidbodies.ForEach(Object.DestroyImmediate);
                _joints.ForEach(joint =>
                {
                    joint.connectedBody = null;
                    joint.connectedArticulationBody = null;
                    Object.DestroyImmediate(joint);
                });
            }
        }
    }
}