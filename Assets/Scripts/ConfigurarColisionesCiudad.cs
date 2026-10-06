using System.Collections;
using UnityEngine;

public class ConfigurarColisionesCiudad : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ConfigurarDespuesDeCargar());
    }

    IEnumerator ConfigurarDespuesDeCargar()
    {
        // Esperamos a que la ciudad esté completamente cargada.
        yield return null;
        yield return null;

        Configurar();
    }

    void Configurar()
    {
        MeshFilter[] objetos =
            GetComponentsInChildren<MeshFilter>(true);

        int creados = 0;
        int existentes = 0;
        int ignorados = 0;

        foreach (MeshFilter mesh in objetos)
        {
            if (mesh == null || mesh.sharedMesh == null)
                continue;

            GameObject objeto = mesh.gameObject;

            string nombre =
                objeto.name.ToLower();

            // =========================================
            // OBJETOS QUE PODEMOS ATRAVESAR
            // =========================================

            bool atravesable =
                nombre.Contains("grass") ||
                nombre.Contains("soil") ||
                nombre.Contains("plant") ||
                nombre.Contains("flower") ||
                nombre.Contains("bush") ||
                nombre.Contains("hedge") ||
                nombre.Contains("nature") ||
                nombre.Contains("asphalt") ||
                nombre.Contains("sidewalk") ||
                nombre.Contains("water");

            if (atravesable)
            {
                ignorados++;
                continue;
            }

            // =========================================
            // OBJETOS SÓLIDOS
            // =========================================

            bool casa =
                nombre == "home" ||
                nombre.StartsWith("home_") ||
                nombre.StartsWith("home.") ||
                nombre == "house" ||
                nombre.StartsWith("house_") ||
                nombre.StartsWith("house.");

            bool arbol =
                nombre == "tree" ||
                nombre.StartsWith("tree_") ||
                nombre.StartsWith("tree.");

            bool poste =
                nombre.Contains("lamppost");

            bool basurero =
                nombre.Contains("trash_can") ||
                nombre.Contains("trashcan");

            bool cerca =
                nombre == "fence" ||
                nombre.StartsWith("fence_") ||
                nombre.StartsWith("fence.") ||
                nombre.Contains("garden_gate");

            bool puertaGarage =
                nombre.Contains("garage_door");

            if (!casa &&
                !arbol &&
                !poste &&
                !basurero &&
                !cerca &&
                !puertaGarage)
            {
                continue;
            }

            // =========================================
            // SI YA TIENE COLLIDER
            // =========================================

            Collider colliderExistente =
                objeto.GetComponent<Collider>();

            if (colliderExistente != null)
            {
                colliderExistente.enabled = true;
                colliderExistente.isTrigger = false;

                existentes++;
                continue;
            }

            // =========================================
            // CREAR COLLIDER
            // =========================================

            MeshCollider collider =
                objeto.AddComponent<MeshCollider>();

            collider.sharedMesh =
                mesh.sharedMesh;

            collider.convex = false;
            collider.isTrigger = false;
            collider.enabled = true;

            creados++;
        }

        Debug.Log(
            "COLISIONES CIUDAD LISTAS | " +
            "Creados: " + creados +
            " | Existentes: " + existentes +
            " | Atravesables: " + ignorados
        );
    }
}