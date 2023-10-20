using UnityEngine;
using Zenject;

public class Test : IInitializable
{
    public void Initialize()
    {
        Debug.Log("Initialized!");
    }
}