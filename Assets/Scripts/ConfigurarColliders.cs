using UnityEngine;

public class ConfigurarColliders : MonoBehaviour
{
    void Start()
    {
        CrearColliders();
    }

    void CrearColliders()
    {
        MeshRenderer[] objetos =
            GetComponentsInChildren<MeshRenderer>(true);

        int creados = 0;

        foreach (MeshRenderer renderer in objetos)
        {
            if (renderer == null)
                continue;

            GameObject objeto = renderer.gameObject;
            string nombre = objeto.name.ToLower();

            // ==========================================
            // SOLO ESTOS OBJETOS TENDRÁN COLISIÓN
            // ==========================================

            bool esArbol =
                nombre == "tree" ||
                nombre.StartsWith("tree_") ||
                nombre.StartsWith("tree.");

            bool esPoste =
                nombre == "lamppost" ||
                nombre.StartsWith("lamppost_") ||
                nombre.StartsWith("lamppost.");

            bool esBasurero =
                nombre == "trash_can" ||
                nombre.StartsWith("trash_can_") ||
                nombre.StartsWith("trash_can.");

            bool esCerco =
                nombre == "fence" ||
                nombre.StartsWith("fence_") ||
                nombre.StartsWith("fence.");

            bool esCasa =
                nombre == "home" ||
                nombre.StartsWith("home_") ||
                nombre.StartsWith("home.") ||
                nombre == "house" ||
                nombre.StartsWith("house_") ||
                nombre.StartsWith("house.");

            if (!esArbol &&
                !esPoste &&
                !esBasurero &&
                !esCerco &&
                !esCasa)
            {
                continue;
            }

            // Si ya tiene un collider, no hacemos otro.
            Collider existente =
                objeto.GetComponent<Collider>();

            if (existente != null)
            {
                existente.enabled = true;
                existente.isTrigger = false;
                continue;
            }

            // ==========================================
            // BOX COLLIDER SIMPLE
            // ==========================================

            BoxCollider collider =
                objeto.AddComponent<BoxCollider>();

            // Unity calcula automáticamente el tamaño
            // según la malla de ESTE objeto.
            Bounds bounds = renderer.localBounds;

            collider.center = bounds.center;
            collider.size = bounds.size;

            creados++;

            Debug.Log(
                "Collider creado en: " + objeto.name
            );
        }

        Debug.Log(
            "COLISIONES TERMINADAS. Creadas: " +
            creados
        );
    }
}