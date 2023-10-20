using UnityEngine;

public class AwakeMessage : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private string _message = "Awake";

    #region MonoBehaviour

    private void Awake()
    {
        Debug.Log(_message);
    }

    #endregion
}