using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
    public static class EnableInstancingMenu
    {
        [MenuItem("Game/Utility/Enable GPU instancing in all material")]
        public static void OpenGameConfig()
        {
            string[] allMaterialGuids = AssetDatabase.FindAssets("t:Material");

            foreach (string materialGuid in allMaterialGuids)
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(materialGuid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

                if (material != null && material.enableInstancing == false)
                {
                    material.enableInstancing = true;
                    Debug.Log("GPU instancing enabled for material: " + material.name);
                    EditorUtility.SetDirty(material);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("GPU instancing enabled for all supported materials in the project.");
        }
    }
}