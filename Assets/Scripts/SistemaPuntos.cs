using UnityEngine;

public class SistemaPuntos : MonoBehaviour
{
    public static SistemaPuntos instancia;

    [Header("Puntos de Luz")]
    public int puntos = 0;

    // =====================================================
    // CREAR AUTOMÁTICAMENTE EL SISTEMA
    // =====================================================

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.BeforeSceneLoad
    )]
    static void CrearSistema()
    {
        if (FindFirstObjectByType<SistemaPuntos>() != null)
            return;

        GameObject objeto =
            new GameObject("SistemaPuntos");

        objeto.AddComponent<SistemaPuntos>();
    }

    // =====================================================
    // INICIO
    // =====================================================

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            puntos = 0;
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
    }

    // =====================================================
    // SUMAR PUNTOS
    // =====================================================

    public void SumarPuntos(int cantidad)
    {
        if (cantidad <= 0)
            return;

        puntos += cantidad;

        Debug.Log(
            "PUNTOS DE LUZ: " +
            puntos
        );
    }

    // =====================================================
    // CONSULTAR PUNTOS
    // =====================================================

    public int ObtenerPuntos()
    {
        return puntos;
    }
}