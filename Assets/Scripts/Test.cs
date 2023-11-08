using Infrastructure.Services.PersistentData.Core;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject] private readonly IPersistentDataService _persistentDataService;

    private void Awake() { }
}