using UnityEngine;

public class EnemigoFantasma : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Persecución")]
    public float distanciaDeteccion = 15f;
    public float distanciaAbandono = 25f;
    public float velocidad = 2f;
    public float distanciaMinimaJugador = 1.5f;

    [Header("Ataque")]
    public float distanciaAtaque = 2f;
    public int danoAtaque = 10;
    public float tiempoEntreAtaques = 1.5f;

    [Header("Flotación")]
    public float alturaFlotacion = 0.25f;
    public float velocidadFlotacion = 2f;

    private float alturaInicial;
    private bool persiguiendo = false;

    private float temporizadorAtaque = 0f;

    private EnergiaJugador energiaJugador;

    void Start()
    {
        alturaInicial = transform.position.y;

        if (jugador == null)
        {
            GameObject objetoJugador =
                GameObject.Find("CuerpoJugador");

            if (objetoJugador != null)
                jugador = objetoJugador.transform;
        }

        if (jugador != null)
        {
            energiaJugador =
                jugador.GetComponent<EnergiaJugador>();
        }
    }

    void Update()
    {
        Flotar();

        if (jugador == null)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                jugador.position
            );

        // Comienza a perseguir.
        if (!persiguiendo &&
            distancia <= distanciaDeteccion)
        {
            persiguiendo = true;
        }

        // Deja de perseguir si el jugador escapa.
        if (persiguiendo &&
            distancia >= distanciaAbandono)
        {
            persiguiendo = false;
        }

        if (!persiguiendo)
            return;

        PerseguirJugador(distancia);

        temporizadorAtaque -= Time.deltaTime;

        if (distancia <= distanciaAtaque)
        {
            AtacarJugador();
        }
    }

    void Flotar()
    {
        Vector3 posicion = transform.position;

        posicion.y =
            alturaInicial +
            Mathf.Sin(
                Time.time * velocidadFlotacion
            ) *
            alturaFlotacion;

        transform.position = posicion;
    }

    void PerseguirJugador(float distancia)
    {
        Vector3 direccion =
            jugador.position -
            transform.position;

        direccion.y = 0f;

        if (direccion.sqrMagnitude > 0.01f)
        {
            Quaternion rotacionObjetivo =
                Quaternion.LookRotation(direccion);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    rotacionObjetivo,
                    5f * Time.deltaTime
                );
        }

        if (distancia > distanciaMinimaJugador)
        {
            Vector3 movimiento =
                direccion.normalized *
                velocidad *
                Time.deltaTime;

            transform.position += movimiento;
        }
    }

    void AtacarJugador()
    {
        if (temporizadorAtaque > 0f)
            return;

        if (energiaJugador == null)
        {
            energiaJugador =
                jugador.GetComponent<EnergiaJugador>();
        }

        if (energiaJugador == null)
            return;

        energiaJugador.RecibirDanio(danoAtaque);

        temporizadorAtaque =
            tiempoEntreAtaques;

        Debug.Log(
            gameObject.name +
            " atacó al jugador. Daño: " +
            danoAtaque
        );
    }
}