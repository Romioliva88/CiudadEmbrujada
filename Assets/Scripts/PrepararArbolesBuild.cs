using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class PrepararArbolesBuild : MonoBehaviour
{
#if UNITY_EDITOR

    [ContextMenu("PREPARAR ARBOLES PARA BUILD")]
    public void PrepararArboles()
    {
        string carpeta = "Assets/MaterialesBuild";

        if (!AssetDatabase.IsValidFolder(carpeta))
        {
            AssetDatabase.CreateFolder(
                "Assets",
                "MaterialesBuild"
            );
        }

        MeshRenderer[] renderers =
            GetComponentsInChildren<MeshRenderer>(true);

        int corregidos = 0;
        int materialesCreados = 0;

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            string nombre =
                renderer.gameObject.name.ToLower();

            bool esVegetacion =
                nombre.StartsWith("tree") ||
                nombre.StartsWith("plant") ||
                nombre.StartsWith("natureplant") ||
                nombre.StartsWith("flower") ||
                nombre.Contains("plants_flowers") ||
                nombre.Contains("bush");

            if (!esVegetacion)
                continue;

            Material[] originales =
                renderer.sharedMaterials;

            Material[] nuevos =
                new Material[originales.Length];

            for (int i = 0; i < originales.Length; i++)
            {
                Material original = originales[i];

                if (original == null)
                {
                    nuevos[i] = null;
                    continue;
                }

                string rutaOriginal =
                    AssetDatabase.GetAssetPath(original);

                string nombreSeguro =
                    original.name.Replace("/", "_");

                string rutaNueva =
                    carpeta + "/" +
                    nombreSeguro +
                    "_Build.mat";

                Material materialBuild =
                    AssetDatabase.LoadAssetAtPath<Material>(
                        rutaNueva
                    );

                if (materialBuild == null)
                {
                    materialBuild =
                        new Material(original);

                    AssetDatabase.CreateAsset(
                        materialBuild,
                        AssetDatabase.GenerateUniqueAssetPath(
                            rutaNueva
                        )
                    );

                    materialesCreados++;
                }

                // ================================
                // CORRECCIÓN DE TRANSPARENCIA
                // ================================

                if (materialBuild.HasProperty("_AlphaClip"))
                    materialBuild.SetFloat("_AlphaClip", 1f);

                if (materialBuild.HasProperty("_Cutoff"))
                    materialBuild.SetFloat("_Cutoff", 0.5f);

                if (materialBuild.HasProperty("_AlphaCutoff"))
                    materialBuild.SetFloat("_AlphaCutoff", 0.5f);

                if (materialBuild.HasProperty("_Surface"))
                    materialBuild.SetFloat("_Surface", 0f);

                materialBuild.EnableKeyword(
                    "_ALPHATEST_ON"
                );

                materialBuild.renderQueue = 2450;

                EditorUtility.SetDirty(materialBuild);

                nuevos[i] = materialBuild;
            }

            renderer.sharedMaterials = nuevos;

            EditorUtility.SetDirty(renderer);

            corregidos++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.SetDirty(gameObject);

        Debug.Log(
            "ARBOLES PREPARADOS PARA BUILD | " +
            "Objetos corregidos: " + corregidos +
            " | Materiales creados: " + materialesCreados
        );
    }

#endif
}