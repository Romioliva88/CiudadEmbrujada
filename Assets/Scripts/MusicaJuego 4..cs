using UnityEngine;

public class MusicaJuego : MonoBehaviour
{
    [Header("Música")]
    public AudioClip musicaEmbrujada;
    public AudioClip musicaFeliz;

    private AudioSource audioSource;
    private bool musicaFinalActivada = false;

    void Start()
    {
        // Creamos automáticamente el AudioSource.
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;

        // Al comenzar el juego suena la música embrujada.
        ReproducirMusicaEmbrujada();
    }

    public void ReproducirMusicaEmbrujada()
    {
        if (musicaEmbrujada == null)
        {
            Debug.LogWarning(
                "No se asignó la música embrujada."
            );
            return;
        }

        audioSource.Stop();
        audioSource.clip = musicaEmbrujada;
        audioSource.loop = true;
        audioSource.Play();

        Debug.Log("Música embrujada iniciada.");
    }

    public void ReproducirMusicaFeliz()
    {
        if (musicaFinalActivada)
            return;

        musicaFinalActivada = true;

        if (musicaFeliz == null)
        {
            Debug.LogWarning(
                "No se asignó la música feliz."
            );
            return;
        }

        // Detiene inmediatamente la música de miedo.
        audioSource.Stop();

        // Cambia a la música feliz.
        audioSource.clip = musicaFeliz;
        audioSource.loop = true;
        audioSource.Play();

        Debug.Log(
            "¡Música feliz iniciada!"
        );
    }
}