using System.Collections;
using UnityEngine;
using Zenject;

namespace Plugins.ObjectPoolSystem.Test
{
    public class ObjectPoolsTest : MonoBehaviour
    {
        private ObjectPools<MainPool> _mainPool;

        [Inject]
        private void Constructor(ObjectPools<MainPool> mainPool)
        {
            _mainPool = mainPool;
        }

        private IEnumerator Start()
        {
            while (true)
            {
                GameObject pooledObject = _mainPool.GetPool(MainPool.Sphere).Get();

                pooledObject.transform.position = Random.insideUnitSphere * 100f;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
