using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 4f;
    public float gravedad = -9.81f;

    private CharacterController controlador;
    private float velocidadVertical;

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        ColocarEnSuelo();
    }

    void ColocarEnSuelo()
    {
        GameObject suelo = GameObject.Find("SueloCasa");

        if (suelo == null)
        {
            Debug.LogError("No encontré el objeto SueloCasa.");
            return;
        }

        Collider colliderSuelo = suelo.GetComponent<Collider>();

        if (colliderSuelo == null)
        {
            Debug.LogError("SueloCasa no tiene Collider.");
            return;
        }

        // Tomamos solamente la altura de SueloCasa
        float alturaSuelo = colliderSuelo.bounds.max.y;

        controlador.enabled = false;

        Vector3 posicion = transform.position;

        // Dejamos al jugador justo encima del suelo
        posicion.y = alturaSuelo + 0.1f;

        transform.position = posicion;

        controlador.enabled = true;

        velocidadVertical = 0f;

        Debug.Log("Jugador colocado sobre SueloCasa. Y = " + posicion.y);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        // IZQUIERDA
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        // DERECHA
        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }

        // ADELANTE
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 1f;
        }

        // ATRÁS
        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -1f;
        }

        Vector3 movimiento =
            transform.right * horizontal +
            transform.forward * vertical;

        movimiento = movimiento.normalized * velocidad;

        // GRAVEDAD
        if (controlador.isGrounded && velocidadVertical < 0f)
        {
            velocidadVertical = -2f;
        }

        velocidadVertical += gravedad * Time.deltaTime;

        movimiento.y = velocidadVertical;

        controlador.Move(movimiento * Time.deltaTime);
    }
}