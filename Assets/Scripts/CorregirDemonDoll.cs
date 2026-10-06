using UnityEngine;

public class CorregirDemonDoll : MonoBehaviour
{
    void Awake()
    {
        CorregirMateriales();
    }

    void CorregirMateriales()
    {
        Shader shaderURP = Shader.Find("Universal Render Pipeline/Lit");

        if (shaderURP == null)
        {
            Debug.LogError("No se encontró el shader URP/Lit.");
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        int corregidos = 0;

        foreach (Renderer renderer in renderers)
        {
            Material[] materiales = renderer.materials;

            for (int i = 0; i < materiales.Length; i++)
            {
                Material material = materiales[i];

                if (material == null)
                    continue;

                // Conservamos la textura principal antes de cambiar shader
                Texture texturaPrincipal = null;

                if (material.HasProperty("_MainTex"))
                    texturaPrincipal = material.GetTexture("_MainTex");
                else if (material.HasProperty("_BaseMap"))
                    texturaPrincipal = material.GetTexture("_BaseMap");

                // Cambiamos cualquier material incompatible a URP/Lit
                if (material.shader != shaderURP)
                {
                    material.shader = shaderURP;

                    if (texturaPrincipal != null &&
                        material.HasProperty("_BaseMap"))
                    {
                        material.SetTexture("_BaseMap", texturaPrincipal);
                    }

                    corregidos++;

                    Debug.Log(
                        "✓ Corregido: " +
                        renderer.gameObject.name +
                        " / " +
                        material.name
                    );
                }
            }

            renderer.materials = materiales;
        }

        Debug.Log(
            "DemonDoll corregido. Materiales cambiados: " +
            corregidos
        );
    }
}