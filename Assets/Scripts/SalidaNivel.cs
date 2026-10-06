using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivel : MonoBehaviour
{
    [Header("Salida")]
    public float distanciaActivacion = 2.5f;

    [Header("Final")]
    public float tiempoAntesReinicio = 7f;

    private Transform jugador;
    private bool finalIniciado = false;

    void Start()
    {
        GameObject objetoJugador =
            GameObject.Find("CuerpoJugador");

        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }
    }

    void Update()
    {
        if (finalIniciado)
            return;

        if (jugador == null)
        {
            GameObject objetoJugador =
                GameObject.Find("CuerpoJugador");

            if (objetoJugador != null)
                jugador = objetoJugador.transform;

            return;
        }

        float distancia =
            Vector3.Distance(
                jugador.position,
                transform.position
            );

        if (distancia <= distanciaActivacion)
        {
            SistemaFragmentos fragmentos =
                jugador.GetComponent<SistemaFragmentos>();

            if (fragmentos != null &&
                fragmentos.TieneLlave())
            {
                StartCoroutine(
                    TerminarNivel()
                );
            }
        }
    }

    IEnumerator TerminarNivel()
    {
        if (finalIniciado)
            yield break;

        finalIniciado = true;

        Debug.Log(
            "¡HAS ENCONTRADO LA SALIDA!"
        );

        // =========================================
        // DESAPARECER MONSTRUOS
        // =========================================

        GeneradorMonstruos generador =
            FindFirstObjectByType<GeneradorMonstruos>();

        if (generador != null)
        {
            generador.TerminarMaldicion();
        }

        // =========================================
        // AMANECER
        // =========================================

        CicloDiaNoche ciclo =
            FindFirstObjectByType<CicloDiaNoche>();

        if (ciclo != null)
        {
            ciclo.RomperHechizo();
        }

        // =========================================
        // MÚSICA FELIZ
        // =========================================

        MusicaJuego musica =
            FindFirstObjectByType<MusicaJuego>();

        if (musica != null)
        {
            musica.ReproducirMusicaFeliz();
        }

        // =========================================
        // ESPERAR EL AMANECER
        // =========================================

        if (ciclo != null)
        {
            while (ciclo.EstaAmaneciendo())
            {
                yield return null;
            }
        }

        // =========================================
        // MENSAJE FINAL
        // =========================================

        InterfazJugador interfaz =
            FindFirstObjectByType<InterfazJugador>();

        if (interfaz != null)
        {
            interfaz.MostrarMensajeFinal();
        }

        Debug.Log(
            "¡FELICIDADES! HAS ROTO LA MALDICIÓN."
        );

        // =========================================
        // ESPERAR Y REINICIAR NIVEL
        // =========================================

        yield return new WaitForSeconds(
            tiempoAntesReinicio
        );

        Time.timeScale = 1f;

        Scene escenaActual =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            escenaActual.name
        );
    }
}