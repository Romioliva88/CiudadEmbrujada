using UnityEngine;

public class LlaveFinalBrillante : MonoBehaviour
{
    public float velocidadGiro = 70f;
    public float alturaFlotacion = 0.25f;
    public float velocidadFlotacion = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // GIRAR LA LLAVE
        transform.Rotate(
            Vector3.up,
            velocidadGiro * Time.deltaTime,
            Space.World
        );

        // HACER FLOTAR LA LLAVE
        float movimiento =
            Mathf.Sin(
                Time.time * velocidadFlotacion
            ) * alturaFlotacion;

        transform.position =
            posicionInicial +
            Vector3.up * movimiento;
    }
}