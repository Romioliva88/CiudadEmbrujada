using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrepararColisionesBuild : MonoBehaviour
{
#if UNITY_EDITOR

    [ContextMenu("CREAR COLLIDERS PERMANENTES")]
    public void CrearCollidersPermanentes()
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
            string nombre = objeto.name.ToLower();

            // =========================================
            // OBJETOS QUE SE PUEDEN ATRAVESAR
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
            // OBJETOS QUE DEBEN TENER COLISIÓN
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

            Collider existente =
                objeto.GetComponent<Collider>();

            if (existente != null)
            {
                existente.enabled = true;
                existente.isTrigger = false;

                EditorUtility.SetDirty(existente);

                existentes++;
                continue;
            }

            // =========================================
            // CREAR COLLIDER PERMANENTE
            // =========================================

            MeshCollider collider =
                Undo.AddComponent<MeshCollider>(objeto);

            collider.sharedMesh = mesh.sharedMesh;
            collider.convex = false;
            collider.isTrigger = false;
            collider.enabled = true;

            EditorUtility.SetDirty(objeto);

            creados++;
        }

        EditorUtility.SetDirty(gameObject);

        Debug.Log(
            "COLLIDERS PERMANENTES TERMINADOS | " +
            "Creados: " + creados +
            " | Existentes: " + existentes +
            " | Ignorados: " + ignorados
        );
    }

#endif
}