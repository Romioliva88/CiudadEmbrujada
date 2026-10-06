using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EspadaJugador : MonoBehaviour
{
    [Header("Espada")]
    public GameObject espada;

    [Header("Posición en primera persona")]
    public Vector3 posicionGuardada =
        new Vector3(0.45f, -0.55f, 0.75f);

    public Vector3 posicionGolpe =
        new Vector3(0.05f, -0.15f, 1.05f);

    public Vector3 rotacionGuardada =
        new Vector3(20f, -25f, -25f);

    public Vector3 rotacionGolpe =
        new Vector3(-15f, 15f, 55f);

    [Header("Ataque")]
    public float distanciaAtaque = 4f;
    public float anchoAtaque = 2.5f;
    public float duracionAtaque = 0.35f;
    public float tiempoEntreAtaques = 0.35f;

    private bool atacando = false;

    void Start()
    {
        if (espada == null)
            return;

        espada.transform.localPosition =
            posicionGuardada;

        espada.transform.localRotation =
            Quaternion.Euler(rotacionGuardada);

        espada.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current == null ||
            espada == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !atacando)
        {
            StartCoroutine(Atacar());
        }
    }

    IEnumerator Atacar()
    {
        atacando = true;

        espada.SetActive(true);

        espada.transform.localPosition =
            posicionGuardada;

        espada.transform.localRotation =
            Quaternion.Euler(rotacionGuardada);

        float mitadAtaque =
            duracionAtaque / 2f;

        float tiempo = 0f;

        while (tiempo < mitadAtaque)
        {
            tiempo += Time.deltaTime;

            float porcentaje =
                Mathf.Clamp01(
                    tiempo / mitadAtaque
                );

            espada.transform.localPosition =
                Vector3.Lerp(
                    posicionGuardada,
                    posicionGolpe,
                    porcentaje
                );

            espada.transform.localRotation =
                Quaternion.Slerp(
                    Quaternion.Euler(
                        rotacionGuardada
                    ),
                    Quaternion.Euler(
                        rotacionGolpe
                    ),
                    porcentaje
                );

            yield return null;
        }

        // Momento exacto del golpe.
        DetectarGolpe();

        tiempo = 0f;

        while (tiempo < mitadAtaque)
        {
            tiempo += Time.deltaTime;

            float porcentaje =
                Mathf.Clamp01(
                    tiempo / mitadAtaque
                );

            espada.transform.localPosition =
                Vector3.Lerp(
                    posicionGolpe,
                    posicionGuardada,
                    porcentaje
                );

            espada.transform.localRotation =
                Quaternion.Slerp(
                    Quaternion.Euler(
                        rotacionGolpe
                    ),
                    Quaternion.Euler(
                        rotacionGuardada
                    ),
                    porcentaje
                );

            yield return null;
        }

        espada.transform.localPosition =
            posicionGuardada;

        espada.transform.localRotation =
            Quaternion.Euler(rotacionGuardada);

        espada.SetActive(false);

        yield return new WaitForSeconds(
            tiempoEntreAtaques
        );

        atacando = false;
    }

    void DetectarGolpe()
    {
        Monstruo[] monstruos =
            FindObjectsByType<Monstruo>(
                FindObjectsSortMode.None
            );

        List<Monstruo> golpeados =
            new List<Monstruo>();

        foreach (Monstruo monstruo in monstruos)
        {
            if (monstruo == null)
                continue;

            Vector3 haciaMonstruo =
                monstruo.transform.position -
                transform.position;

            float distancia =
                haciaMonstruo.magnitude;

            // Demasiado lejos.
            if (distancia > distanciaAtaque)
                continue;

            Vector3 direccion =
                haciaMonstruo.normalized;

            float delante =
                Vector3.Dot(
                    transform.forward,
                    direccion
                );

            // Debe estar delante del jugador.
            if (delante < 0.35f)
                continue;

            // Comprueba que no esté demasiado
            // hacia un costado.
            Vector3 puntoMasCercano =
                transform.position +
                transform.forward *
                Vector3.Dot(
                    haciaMonstruo,
                    transform.forward
                );

            float distanciaLateral =
                Vector3.Distance(
                    monstruo.transform.position,
                    puntoMasCercano
                );

            if (distanciaLateral >
                anchoAtaque)
            {
                continue;
            }

            golpeados.Add(monstruo);
        }

        foreach (Monstruo monstruo in golpeados)
        {
            if (monstruo != null)
            {
                Debug.Log(
                    "Espada golpeó a: " +
                    monstruo.gameObject.name
                );

                monstruo.Eliminar();
            }
        }

        if (golpeados.Count == 0)
        {
            Debug.Log(
                "Ataque realizado sin alcanzar monstruos."
            );
        }
    }
}