using UnityEngine;
using UnityEngine.SceneManagement;

public class EnergiaJugador : MonoBehaviour
{
    [Header("Energía")]
    public int energiaMaxima = 100;
    public int energiaActual = 100;

    [Header("Derrota")]
    public bool reiniciarAlMorir = true;
    public float tiempoParaReiniciar = 5f;

    private bool derrotado = false;

    // =====================================================
    // INICIO
    // =====================================================

    void Start()
    {
        energiaActual = energiaMaxima;
        derrotado = false;
    }

    // =====================================================
    // RECIBIR DAÑO
    // =====================================================

    public void RecibirDanio(int cantidad)
    {
        if (derrotado)
            return;

        energiaActual -= cantidad;

        energiaActual = Mathf.Clamp(
            energiaActual,
            0,
            energiaMaxima
        );

        Debug.Log(
            "Energía del jugador: " +
            energiaActual +
            "/" +
            energiaMaxima
        );

        if (energiaActual <= 0)
        {
            Derrota();
        }
    }

    // =====================================================
    // RECUPERAR ENERGÍA
    // =====================================================

    public void RecuperarEnergia(int cantidad)
    {
        if (derrotado)
            return;

        energiaActual += cantidad;

        energiaActual = Mathf.Clamp(
            energiaActual,
            0,
            energiaMaxima
        );
    }

    // =====================================================
    // DERROTA
    // =====================================================

    void Derrota()
    {
        if (derrotado)
            return;

        derrotado = true;
        energiaActual = 0;

        Debug.Log(
            "PERDISTE - Inténtalo de nuevo. ¡No te rindas!"
        );

        // Buscar nuestra interfaz
        InterfazJugador interfaz =
            FindFirstObjectByType<InterfazJugador>();

        if (interfaz != null)
        {
            interfaz.MostrarDerrota();
        }
        else
        {
            Debug.LogWarning(
                "No se encontró InterfazJugador."
            );
        }

        // Desactivar movimiento del jugador
        MovimientoJugador movimiento =
            GetComponent<MovimientoJugador>();

        if (movimiento != null)
        {
            movimiento.enabled = false;
        }

        // Reiniciar después de unos segundos
        if (reiniciarAlMorir)
        {
            Invoke(
                nameof(ReiniciarEscena),
                tiempoParaReiniciar
            );
        }
    }

    // =====================================================
    // REINICIAR JUEGO
    // =====================================================

    void ReiniciarEscena()
    {
        Scene escenaActual =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            escenaActual.name
        );
    }

    // =====================================================
    // INFORMACIÓN PARA OTROS SCRIPTS
    // =====================================================

    public int ObtenerEnergiaActual()
    {
        return energiaActual;
    }

    public int ObtenerEnergiaMaxima()
    {
        return energiaMaxima;
    }

    public bool EstaDerrotado()
    {
        return derrotado;
    }
}