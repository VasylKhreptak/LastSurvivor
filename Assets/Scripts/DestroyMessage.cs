using UnityEngine;

public class DestroyMessage : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private string _message = "Destroy";

    #region MonoBehaviour

    private void OnDestroy()
    {
        Debug.Log(_message);
    }

    #endregion
}