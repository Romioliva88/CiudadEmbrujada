using UnityEngine;

public class ConfigurarLlave : MonoBehaviour
{
    [Header("Texturas de la llave")]
    public Texture2D texturaColor;
    public Texture2D texturaNormal;
    public Texture2D texturaOcclusionRoughness;

    [Header("Aspecto")]
    public Color tonoLlave =
        new Color(0.75f, 0.65f, 0.50f, 1f);

    public float metalico = 0.75f;
    public float suavidad = 0.25f;

    void Start()
    {
        ConfigurarMaterial();
    }

    void ConfigurarMaterial()
    {
        Shader shaderURP =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shaderURP == null)
        {
            Debug.LogError(
                "No se encontró el shader URP/Lit."
            );

            return;
        }

        Material material =
            new Material(shaderURP);

        material.name = "MaterialLlave";

        // Color principal
        if (texturaColor != null)
        {
            material.SetTexture(
                "_BaseMap",
                texturaColor
            );
        }

        material.SetColor(
            "_BaseColor",
            tonoLlave
        );

        // Relieve
        if (texturaNormal != null)
        {
            material.SetTexture(
                "_BumpMap",
                texturaNormal
            );

            material.EnableKeyword(
                "_NORMALMAP"
            );
        }

        // Oclusión
        if (texturaOcclusionRoughness != null)
        {
            material.SetTexture(
                "_OcclusionMap",
                texturaOcclusionRoughness
            );

            material.EnableKeyword(
                "_OCCLUSIONMAP"
            );

            material.SetFloat(
                "_OcclusionStrength",
                1f
            );
        }

        // Aspecto metálico
        material.SetFloat(
            "_Metallic",
            metalico
        );

        material.SetFloat(
            "_Smoothness",
            suavidad
        );

        // Aplicar a toda la llave
        Renderer[] renderers =
            GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }

        Debug.Log(
            "Llave configurada correctamente."
        );
    }
}