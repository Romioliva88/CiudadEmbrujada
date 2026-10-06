using System.Collections.Generic;
using UnityEngine;

public class GeneradorFragmentos : MonoBehaviour
{
    // =====================================================
    // CONFIGURACIÓN
    // =====================================================

    [Header("Cantidad")]
    public int cantidadFragmentos = 4;

    [Header("Distribución")]
    public float separacionMinima = 10f;
    public float distanciaMinimaJugador = 7f;

    [Header("Fragmentos")]
    public float tamanoFragmento = 0.35f;
    public float distanciaVegetacion = 2.5f;


    // =====================================================
    // REFERENCIAS
    // =====================================================

    private Transform jugador;
    private GeneradorMonstruos generadorMonstruos;

    private List<Transform> vegetacion =
        new List<Transform>();

    private List<Vector3> posicionesUsadas =
        new List<Vector3>();


    // =====================================================
    // INICIO
    // =====================================================

    void Start()
    {
        // Siempre serán exactamente cuatro.
        cantidadFragmentos = 4;

        BuscarReferencias();
        BuscarVegetacion();

        if (vegetacion.Count == 0)
        {
            Debug.LogWarning(
                "No se encontró vegetación. " +
                "Se utilizarán posiciones alternativas."
            );
        }

        CrearTodosLosFragmentos();
    }


    // =====================================================
    // REFERENCIAS
    // =====================================================

    void BuscarReferencias()
    {
        GameObject objetoJugador =
            GameObject.Find("CuerpoJugador");

        if (objetoJugador != null)
        {
            jugador =
                objetoJugador.transform;
        }
        else
        {
            Debug.LogWarning(
                "No se encontró CuerpoJugador."
            );
        }


        generadorMonstruos =
            FindFirstObjectByType<GeneradorMonstruos>();

        if (generadorMonstruos == null)
        {
            Debug.LogWarning(
                "No se encontró GeneradorMonstruos."
            );
        }
    }


    // =====================================================
    // BUSCAR VEGETACIÓN
    // =====================================================

    void BuscarVegetacion()
    {
        vegetacion.Clear();

        GameObject escenario =
            GameObject.Find("ESCENARIO");

        if (escenario == null)
        {
            Debug.LogError(
                "No se encontró ESCENARIO."
            );

            return;
        }


        Renderer[] objetos =
            escenario.GetComponentsInChildren<Renderer>(
                true
            );


        foreach (Renderer renderer in objetos)
        {
            if (renderer == null)
                continue;

            if (!renderer.enabled)
                continue;


            string nombre =
                renderer.gameObject.name.ToLower();


            bool esVegetacion =
                nombre.Contains("tree") ||
                nombre.Contains("plant") ||
                nombre.Contains("bush") ||
                nombre.Contains("flower") ||
                nombre.Contains("nature");


            if (!esVegetacion)
                continue;


            Transform objeto =
                renderer.transform;


            if (!vegetacion.Contains(objeto))
            {
                vegetacion.Add(objeto);
            }
        }


        Debug.Log(
            "Vegetación encontrada: " +
            vegetacion.Count
        );
    }


    // =====================================================
    // CREAR EXACTAMENTE LOS 4 FRAGMENTOS
    // =====================================================

    void CrearTodosLosFragmentos()
    {
        posicionesUsadas.Clear();

        MezclarVegetacion();

        int creados = 0;


        // =================================================
        // PRIMER INTENTO:
        // ESCONDERLOS CERCA DE VEGETACIÓN
        // =================================================

        foreach (Transform planta in vegetacion)
        {
            if (creados >= cantidadFragmentos)
                break;

            if (planta == null)
                continue;


            Vector3 posicion;


            // Antes eran solo 8 intentos.
            // Ahora damos muchas más posibilidades.
            for (int intento = 0; intento < 25; intento++)
            {
                if (
                    !BuscarPosicionCercaDeVegetacion(
                        planta,
                        out posicion
                    )
                )
                {
                    continue;
                }


                if (!EstaLejosDelJugador(posicion))
                    continue;


                if (!EstaSeparadoDeOtros(posicion))
                    continue;


                CrearFragmento(
                    posicion,
                    creados + 1
                );


                posicionesUsadas.Add(
                    posicion
                );


                CrearGuardian(
                    posicion
                );


                creados++;


                Debug.Log(
                    "Fragmento " +
                    creados +
                    " creado cerca de vegetación."
                );


                break;
            }
        }


        // =================================================
        // SEGUNDO INTENTO:
        // SI FALTA ALGUNO, SEGUIMOS BUSCANDO
        // =================================================

        int intentosExtra = 0;


        while (
            creados < cantidadFragmentos &&
            intentosExtra < 300
        )
        {
            intentosExtra++;


            Vector3 posicion;


            if (
                BuscarPosicionAlternativa(
                    out posicion
                )
            )
            {
                if (!EstaLejosDelJugador(posicion))
                    continue;


                if (!EstaSeparadoDeOtros(posicion))
                    continue;


                CrearFragmento(
                    posicion,
                    creados + 1
                );


                posicionesUsadas.Add(
                    posicion
                );


                CrearGuardian(
                    posicion
                );


                creados++;


                Debug.Log(
                    "Fragmento " +
                    creados +
                    " creado en posición alternativa."
                );
            }
        }


        // =================================================
        // TERCER INTENTO DE SEGURIDAD
        // =================================================
        // Si todavía falta alguno, relajamos solamente
        // la separación entre fragmentos.
        // Seguimos buscando suelo válido.

        int intentosEmergencia = 0;


        while (
            creados < cantidadFragmentos &&
            intentosEmergencia < 300
        )
        {
            intentosEmergencia++;


            Vector3 posicion;


            if (
                !BuscarPosicionAlternativa(
                    out posicion
                )
            )
            {
                continue;
            }


            if (!EstaLejosDelJugador(posicion))
                continue;


            if (
                !EstaSeparadoDeOtrosConDistancia(
                    posicion,
                    5f
                )
            )
            {
                continue;
            }


            CrearFragmento(
                posicion,
                creados + 1
            );


            posicionesUsadas.Add(
                posicion
            );


            CrearGuardian(
                posicion
            );


            creados++;


            Debug.Log(
                "Fragmento " +
                creados +
                " creado con búsqueda de emergencia."
            );
        }


        // =================================================
        // RESULTADO
        // =================================================

        Debug.Log(
            "===================================="
        );

        Debug.Log(
            "FRAGMENTOS CREADOS: " +
            creados +
            "/" +
            cantidadFragmentos
        );

        Debug.Log(
            "===================================="
        );


        if (creados < cantidadFragmentos)
        {
            Debug.LogError(
                "ERROR: solo se pudieron crear " +
                creados +
                " fragmentos."
            );
        }
    }


    // =====================================================
    // GUARDIÁN DEL FRAGMENTO
    // =====================================================

    void CrearGuardian(
        Vector3 posicion)
    {
        if (generadorMonstruos == null)
            return;


        generadorMonstruos.CrearGuardian(
            posicion
        );
    }


    // =====================================================
    // POSICIÓN CERCA DE VEGETACIÓN
    // =====================================================

    bool BuscarPosicionCercaDeVegetacion(
        Transform planta,
        out Vector3 posicionFinal)
    {
        Vector2 direccion2D =
            Random.insideUnitCircle;


        if (direccion2D.sqrMagnitude < 0.01f)
        {
            direccion2D =
                Vector2.right;
        }


        direccion2D.Normalize();


        float distancia =
            Random.Range(
                distanciaVegetacion * 0.7f,
                distanciaVegetacion
            );


        Vector3 direccion =
            new Vector3(
                direccion2D.x,
                0f,
                direccion2D.y
            );


        Vector3 punto =
            planta.position +
            direccion * distancia;


        return BuscarSueloValido(
            punto,
            out posicionFinal
        );
    }


    // =====================================================
    // POSICIÓN ALTERNATIVA
    // =====================================================

    bool BuscarPosicionAlternativa(
        out Vector3 posicionFinal)
    {
        posicionFinal =
            Vector3.zero;


        // Si tenemos vegetación, utilizamos una planta
        // aleatoria como referencia.
        if (vegetacion.Count > 0)
        {
            Transform planta =
                vegetacion[
                    Random.Range(
                        0,
                        vegetacion.Count
                    )
                ];


            if (planta != null)
            {
                Vector2 direccion2D =
                    Random.insideUnitCircle;


                if (
                    direccion2D.sqrMagnitude <
                    0.01f
                )
                {
                    direccion2D =
                        Vector2.right;
                }


                direccion2D.Normalize();


                float distancia =
                    Random.Range(
                        2f,
                        6f
                    );


                Vector3 punto =
                    planta.position +
                    new Vector3(
                        direccion2D.x,
                        0f,
                        direccion2D.y
                    ) * distancia;


                if (
                    BuscarSueloValido(
                        punto,
                        out posicionFinal
                    )
                )
                {
                    return true;
                }
            }
        }


        // =================================================
        // SI NO FUNCIONÓ:
        // BUSCAR ALREDEDOR DEL JUGADOR
        // =================================================

        if (jugador != null)
        {
            Vector2 direccion2D =
                Random.insideUnitCircle;


            if (
                direccion2D.sqrMagnitude <
                0.01f
            )
            {
                direccion2D =
                    Vector2.right;
            }


            direccion2D.Normalize();


            float distancia =
                Random.Range(
                    12f,
                    35f
                );


            Vector3 punto =
                jugador.position +
                new Vector3(
                    direccion2D.x,
                    0f,
                    direccion2D.y
                ) * distancia;


            if (
                BuscarSueloValido(
                    punto,
                    out posicionFinal
                )
            )
            {
                return true;
            }
        }


        return false;
    }


    // =====================================================
    // BUSCAR SUELO VÁLIDO
    // =====================================================

    bool BuscarSueloValido(
        Vector3 punto,
        out Vector3 posicionFinal)
    {
        posicionFinal =
            Vector3.zero;


        Vector3 origen =
            punto +
            Vector3.up * 12f;


        RaycastHit[] golpes =
            Physics.RaycastAll(
                origen,
                Vector3.down,
                25f,
                ~0,
                QueryTriggerInteraction.Ignore
            );


        if (golpes.Length == 0)
            return false;


        System.Array.Sort(
            golpes,
            (a, b) =>
                a.distance.CompareTo(
                    b.distance
                )
        );


        foreach (RaycastHit golpe in golpes)
        {
            if (golpe.collider == null)
                continue;


            // No poner fragmentos sobre monstruos.
            if (
                golpe.collider
                .GetComponentInParent<Monstruo>() !=
                null
            )
            {
                continue;
            }


            string nombre =
                golpe.collider.gameObject.name.ToLower();


            // =================================================
            // SUPERFICIES QUE NO QUEREMOS
            // =================================================

            if (
                nombre.Contains("home") ||
                nombre.Contains("house") ||
                nombre.Contains("roof") ||
                nombre.Contains("tree") ||
                nombre.Contains("lamppost") ||
                nombre.Contains("trash") ||
                nombre.Contains("fence") ||
                nombre.Contains("garage")
            )
            {
                continue;
            }


            // =================================================
            // EVITAR ALTURAS EXTRAÑAS
            // =================================================

            if (jugador != null)
            {
                float diferenciaAltura =
                    Mathf.Abs(
                        golpe.point.y -
                        jugador.position.y
                    );


                // Esto ayuda a evitar techos.
                if (diferenciaAltura > 2.5f)
                    continue;
            }


            // =================================================
            // POSICIÓN FINAL
            // =================================================

            posicionFinal =
                golpe.point +
                Vector3.up * 0.55f;


            return true;
        }


        return false;
    }


    // =====================================================
    // CREAR FRAGMENTO
    // =====================================================

    void CrearFragmento(
        Vector3 posicion,
        int numero)
    {
        GameObject fragmento =
            GameObject.CreatePrimitive(
                PrimitiveType.Capsule
            );


        fragmento.name =
            "FragmentoLlave_" +
            numero;


        fragmento.transform.position =
            posicion;


        fragmento.transform.localScale =
            new Vector3(
                tamanoFragmento * 0.35f,
                tamanoFragmento,
                tamanoFragmento * 0.35f
            );


        fragmento.transform.rotation =
            Quaternion.Euler(
                90f,
                Random.Range(
                    0f,
                    360f
                ),
                Random.Range(
                    -30f,
                    30f
                )
            );


        CrearMaterialFragmento(
            fragmento
        );


        CrearLuzFragmento(
            fragmento
        );


        FragmentoLlave comportamiento =
            fragmento.AddComponent<FragmentoLlave>();


        comportamiento.velocidadGiro =
            70f;

        comportamiento.alturaFlotacion =
            0.20f;

        comportamiento.velocidadFlotacion =
            2f;


        // Lo dejamos amplio para que sea fácil
        // recogerlo entre la vegetación.
        comportamiento.distanciaRecogida =
            3.5f;
    }


    // =====================================================
    // MATERIAL
    // =====================================================

    void CrearMaterialFragmento(
        GameObject fragmento)
    {
        Renderer renderer =
            fragmento.GetComponent<Renderer>();


        if (renderer == null)
            return;


        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit"
            );


        if (shader == null)
            return;


        Material material =
            new Material(shader);


        material.name =
            "MaterialFragmentoLlave";


        Color colorFragmento =
            new Color(
                0.65f,
                0.48f,
                0.18f
            );


        material.SetColor(
            "_BaseColor",
            colorFragmento
        );


        if (
            material.HasProperty(
                "_Metallic"
            )
        )
        {
            material.SetFloat(
                "_Metallic",
                0.85f
            );
        }


        if (
            material.HasProperty(
                "_Smoothness"
            )
        )
        {
            material.SetFloat(
                "_Smoothness",
                0.55f
            );
        }


        material.EnableKeyword(
            "_EMISSION"
        );


        Color brillo =
            new Color(
                1f,
                0.55f,
                0.10f
            ) * 3f;


        material.SetColor(
            "_EmissionColor",
            brillo
        );


        renderer.material =
            material;
    }


    // =====================================================
    // LUZ
    // =====================================================

    void CrearLuzFragmento(
        GameObject fragmento)
    {
        GameObject objetoLuz =
            new GameObject(
                "BrilloFragmento"
            );


        objetoLuz.transform.SetParent(
            fragmento.transform,
            false
        );


        objetoLuz.transform.localPosition =
            Vector3.zero;


        Light luz =
            objetoLuz.AddComponent<Light>();


        luz.type =
            LightType.Point;


        luz.color =
            new Color(
                1f,
                0.55f,
                0.15f
            );


        luz.intensity = 4f;

        luz.range = 7f;

        luz.shadows =
            LightShadows.None;
    }


    // =====================================================
    // DISTANCIA DEL JUGADOR
    // =====================================================

    bool EstaLejosDelJugador(
        Vector3 posicion)
    {
        if (jugador == null)
            return true;


        float distancia =
            Vector2.Distance(
                new Vector2(
                    posicion.x,
                    posicion.z
                ),
                new Vector2(
                    jugador.position.x,
                    jugador.position.z
                )
            );


        return
            distancia >=
            distanciaMinimaJugador;
    }


    // =====================================================
    // SEPARACIÓN NORMAL
    // =====================================================

    bool EstaSeparadoDeOtros(
        Vector3 posicion)
    {
        return
            EstaSeparadoDeOtrosConDistancia(
                posicion,
                separacionMinima
            );
    }


    // =====================================================
    // SEPARACIÓN PERSONALIZADA
    // =====================================================

    bool EstaSeparadoDeOtrosConDistancia(
        Vector3 posicion,
        float distanciaMinima)
    {
        foreach (
            Vector3 otraPosicion
            in posicionesUsadas
        )
        {
            float distancia =
                Vector2.Distance(
                    new Vector2(
                        posicion.x,
                        posicion.z
                    ),
                    new Vector2(
                        otraPosicion.x,
                        otraPosicion.z
                    )
                );


            if (
                distancia <
                distanciaMinima
            )
            {
                return false;
            }
        }


        return true;
    }


    // =====================================================
    // MEZCLAR VEGETACIÓN
    // =====================================================

    void MezclarVegetacion()
    {
        for (
            int i = 0;
            i < vegetacion.Count;
            i++
        )
        {
            int otro =
                Random.Range(
                    i,
                    vegetacion.Count
                );


            Transform temporal =
                vegetacion[i];


            vegetacion[i] =
                vegetacion[otro];


            vegetacion[otro] =
                temporal;
        }
    }
}