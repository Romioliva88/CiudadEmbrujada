using UnityEngine;

public class FragmentoLlave : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadGiro = 60f;
    public float alturaFlotacion = 0.15f;
    public float velocidadFlotacion = 2f;

    [Header("Recogida")]
    public float distanciaRecogida = 1.5f;

    private Transform jugador;
    private SistemaFragmentos sistemaFragmentos;

    private float alturaInicial;
    private bool recogido = false;

    void Start()
    {
        alturaInicial = transform.position.y;

        GameObject objetoJugador =
            GameObject.Find("CuerpoJugador");

        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;

            sistemaFragmentos =
                objetoJugador.GetComponent<SistemaFragmentos>();
        }
        else
        {
            Debug.LogWarning(
                "FragmentoLlave: no se encontró CuerpoJugador."
            );
        }
    }

    void Update()
    {
        Girar();
        Flotar();

        if (jugador == null || recogido)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                jugador.position
            );

        if (distancia <= distanciaRecogida)
        {
            Recoger();
        }
    }

    void Girar()
    {
        transform.Rotate(
            Vector3.up,
            velocidadGiro * Time.deltaTime,
            Space.World
        );
    }

    void Flotar()
    {
        Vector3 posicion =
            transform.position;

        posicion.y =
            alturaInicial +
            Mathf.Sin(
                Time.time * velocidadFlotacion
            ) *
            alturaFlotacion;

        transform.position = posicion;
    }

    void Recoger()
    {
        if (recogido)
            return;

        recogido = true;

        // Guardamos la posición ANTES de destruir
        // el cuarto fragmento.
        Vector3 posicionFragmento =
            transform.position;

        if (sistemaFragmentos != null)
        {
            sistemaFragmentos.RecogerFragmento(
                posicionFragmento
            );
        }
        else
        {
            Debug.LogWarning(
                "FragmentoLlave: no se encontró SistemaFragmentos."
            );
        }

        Debug.Log(
            "¡Fragmento de llave recogido!"
        );

        Destroy(gameObject);
    }
}