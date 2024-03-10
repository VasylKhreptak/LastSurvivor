using System;
using Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.GameObjectUtilities
{
    public class SkinRandomizer
    {
        private readonly Animator _animator;
        private readonly Preferences _preferences;

        private readonly Transform[] _bones;

        public SkinRandomizer(Animator animator, Preferences preferences)
        {
            _animator = animator;
            _preferences = preferences;

            _bones = _preferences.SkinRoot.GetComponentInChildren<SkinnedMeshRenderer>().bones;
        }

        public void Randomize()
        {
            RemoveExistingSkin();
            CreateRandomSkin();
            _animator.Rebind();
        }

        private void RemoveExistingSkin()
        {
            Transform[] children = _preferences.SkinRoot.GetChildren();

            foreach (Transform child in children)
                Object.Destroy(child.gameObject);
        }

        private void CreateRandomSkin()
        {
            GameObject prefab = _preferences.SkinPrefabs.Random();

            GameObject skin = Object.Instantiate(prefab, _preferences.SkinRoot);
            skin.transform.localPosition = Vector3.zero;
            skin.transform.localRotation = Quaternion.identity;
            skin.transform.localScale = Vector3.one;

            SkinnedMeshRenderer skinnedMeshRenderer = skin.GetComponentInChildren<SkinnedMeshRenderer>();
            skinnedMeshRenderer.rootBone = _preferences.Root;
            skinnedMeshRenderer.bones = _bones;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private GameObject[] _skinPrefabs;
            [SerializeField] private Transform _root;
            [SerializeField] private Transform _skinRoot;

            public GameObject[] SkinPrefabs => _skinPrefabs;
            public Transform Root => _root;
            public Transform SkinRoot => _skinRoot;
        }
    }
}