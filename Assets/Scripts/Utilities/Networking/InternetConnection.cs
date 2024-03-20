using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Utilities.Networking
{
    public static class InternetConnection
    {
        private const string Host = "https://www.google.com";
        private const int Timeout = 2;

        public static async Task<bool> CheckAsync()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return false;

            UnityWebRequest request = UnityWebRequest.Head(Host);

            request.timeout = Timeout;

            try
            {
                await request.SendWebRequest();
            }
            catch (Exception)
            {
                // ignored
            }

            return request.result == UnityWebRequest.Result.Success;
        }
    }
}