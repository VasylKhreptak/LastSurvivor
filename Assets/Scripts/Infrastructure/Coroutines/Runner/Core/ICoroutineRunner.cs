using System.Collections;
using UnityEngine;

namespace Infrastructure.Coroutines.Runner.Core
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        
        void StopCoroutine(Coroutine routine);
    }
}
