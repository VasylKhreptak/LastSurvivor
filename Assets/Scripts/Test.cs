using Firebase.Analytics;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    }
}