using UnityEngine;
using UnityEngine.InputSystem;

public class MirarConMouse : MonoBehaviour
{
    public float sensibilidad = 0.15f;
    public Transform cuerpoJugador;

    private float rotacionVertical = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 movimientoMouse = Mouse.current.delta.ReadValue();

        float movimientoX = movimientoMouse.x * sensibilidad;
        float movimientoY = movimientoMouse.y * sensibilidad;

        rotacionVertical -= movimientoY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);

        cuerpoJugador.Rotate(Vector3.up * movimientoX);
    }
}