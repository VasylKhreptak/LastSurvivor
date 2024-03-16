using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    private async void Awake()
    {
        // await DoSomething(this.GetCancellationTokenOnDestroy());
    }

    private async UniTask DoSomething(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(10f), cancellationToken: cancellationToken);
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(10f), cancellationToken: cancellationToken);
        gameObject.SetActive(true);
    }
}