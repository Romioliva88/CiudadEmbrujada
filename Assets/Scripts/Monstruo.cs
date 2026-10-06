using UnityEngine;

public class Monstruo : MonoBehaviour
{
    [Header("Puntos")]
    public int puntosAlEliminar = 100;

    private bool eliminado = false;

    public void Eliminar()
    {
        if (eliminado)
            return;

        eliminado = true;

        // =========================================
        // SUMAR PUNTOS
        // =========================================

        if (SistemaPuntos.instancia != null)
        {
            SistemaPuntos.instancia.SumarPuntos(
                puntosAlEliminar
            );

            Debug.Log(
                "¡Monstruo eliminado! +" +
                puntosAlEliminar +
                " puntos."
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró SistemaPuntos."
            );
        }

        // =========================================
        // ELIMINAR MONSTRUO
        // =========================================

        Destroy(gameObject);
    }
}