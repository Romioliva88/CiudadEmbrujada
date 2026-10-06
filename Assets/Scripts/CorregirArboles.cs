using System.Collections;
using UnityEngine;

public class CorregirArboles : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CorregirDespuesDeCargar());
    }

    IEnumerator CorregirDespuesDeCargar()
    {
        // Esperamos unos frames para asegurarnos
        // de que toda la ciudad ya esté cargada.
        yield return null;
        yield return null;

        CorregirVegetacion();
    }

    void CorregirVegetacion()
    {
        MeshRenderer[] renderers =
            FindObjectsByType<MeshRenderer>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        int corregidos = 0;

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

            Material[] materiales =
                renderer.materials;

            foreach (Material material in materiales)
            {
                if (material == null)
                    continue;

                // Activar Alpha Clipping
                if (material.HasProperty("_AlphaClip"))
                {
                    material.SetFloat(
                        "_AlphaClip",
                        1f
                    );
                }

                // Nivel de corte de transparencia
                if (material.HasProperty("_Cutoff"))
                {
                    material.SetFloat(
                        "_Cutoff",
                        0.5f
                    );
                }

                // Algunos materiales URP usan
                // _AlphaCutoff además de _Cutoff
                if (material.HasProperty("_AlphaCutoff"))
                {
                    material.SetFloat(
                        "_AlphaCutoff",
                        0.5f
                    );
                }

                material.EnableKeyword(
                    "_ALPHATEST_ON"
                );

                // Mantener superficie opaca
                if (material.HasProperty("_Surface"))
                {
                    material.SetFloat(
                        "_Surface",
                        0f
                    );
                }

                material.renderQueue = 2450;
            }

            renderer.materials = materiales;

            corregidos++;
        }

        Debug.Log(
            "VEGETACIÓN CORREGIDA PARA BUILD: " +
            corregidos
        );
    }
}