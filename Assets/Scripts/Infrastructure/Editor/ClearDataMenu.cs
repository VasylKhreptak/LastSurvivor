using System.IO;
using Infrastructure.Data.Persistent;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor
{
    public static class ClearDataMenu
    {
        [MenuItem("Game/🧹 Clear Data %F12")]
        public static void ClearData()
        {
            string path = Path.Combine(Application.persistentDataPath, nameof(PersistentData));

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}