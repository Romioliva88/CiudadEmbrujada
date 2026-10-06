using UnityEngine;

public class SistemaFragmentos : MonoBehaviour
{
    // =====================================================
    // OBJETIVOS
    // =====================================================

    [Header("Objetivos")]
    public int fragmentosNecesarios = 4;
    public int puntosNecesarios = 1000;


    // =====================================================
    // LLAVE FINAL
    // =====================================================

    [Header("Llave final")]
    public float distanciaLlave = 4f;
    public float alturaLlave = 1.4f;
    public float distanciaRecogerLlave = 1.8f;


    // =====================================================
    // ESTADO
    // =====================================================

    private int fragmentosRecogidos = 0;

    private bool llaveCreada = false;
    private bool llaveRecogida = false;
    private bool salidaCreada = false;

    private GameObject llaveFinal;
    private GameObject salidaFinal;

    // Aquí guardaremos EXACTAMENTE dónde estaba
    // el cuarto fragmento.
    private Vector3 posicionUltimoFragmento;
    private bool posicionUltimoFragmentoGuardada = false;


    // =====================================================
    // INICIO
    // =====================================================

    void Start()
    {
        fragmentosNecesarios = 4;
        puntosNecesarios = 1000;

        fragmentosRecogidos = 0;

        llaveCreada = false;
        llaveRecogida = false;
        salidaCreada = false;

        posicionUltimoFragmentoGuardada = false;

        Debug.Log(
            "OBJETIVO: consigue 4 fragmentos y 1000 Puntos de Luz."
        );
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        RevisarObjetivos();

        if (
            llaveCreada &&
            !llaveRecogida &&
            llaveFinal != null
        )
        {
            RevisarRecogidaLlave();
        }
    }


    // =====================================================
    // RECOGER FRAGMENTO
    // =====================================================

    public void RecogerFragmento(
        Vector3 posicionFragmento
    )
    {
        if (fragmentosRecogidos >= fragmentosNecesarios)
            return;

        fragmentosRecogidos++;

        fragmentosRecogidos =
            Mathf.Clamp(
                fragmentosRecogidos,
                0,
                fragmentosNecesarios
            );

        Debug.Log(
            "FRAGMENTOS: " +
            fragmentosRecogidos +
            "/" +
            fragmentosNecesarios
        );


        // =================================================
        // ESTE ES EL CUARTO FRAGMENTO
        // =================================================

        if (
            fragmentosRecogidos >=
            fragmentosNecesarios
        )
        {
            posicionUltimoFragmento =
                posicionFragmento;

            posicionUltimoFragmentoGuardada =
                true;

            Debug.Log(
                "POSICIÓN DEL ÚLTIMO FRAGMENTO GUARDADA: " +
                posicionUltimoFragmento
            );
        }


        RevisarObjetivos();
    }


    // =====================================================
    // COMPATIBILIDAD
    // =====================================================
    // Lo dejamos por seguridad por si algún otro objeto
    // todavía llama RecogerFragmento() sin posición.

    public void RecogerFragmento()
    {
        RecogerFragmento(
            transform.position
        );
    }


    // =====================================================
    // REVISAR OBJETIVOS
    // =====================================================

    void RevisarObjetivos()
    {
        if (llaveCreada || llaveRecogida)
            return;

        if (
            fragmentosRecogidos <
            fragmentosNecesarios
        )
        {
            return;
        }

        int puntosActuales =
            ObtenerPuntosActuales();

        if (puntosActuales < puntosNecesarios)
            return;


        // Ya tenemos:
        // 4 fragmentos
        // +
        // 1000 puntos

        CrearLlaveFinal();
    }


    int ObtenerPuntosActuales()
    {
        if (SistemaPuntos.instancia == null)
            return 0;

        return
            SistemaPuntos.instancia.ObtenerPuntos();
    }


    // =====================================================
    // CREAR OLDKEY
    // =====================================================

    void CrearLlaveFinal()
    {
        if (llaveCreada)
            return;


        GameObject prefabLlave =
            Resources.Load<GameObject>(
                "Llave/OldKey"
            );


        if (prefabLlave == null)
        {
            Debug.LogError(
                "ERROR: NO SE ENCONTRÓ OLDKEY.\n" +
                "Debe estar dentro de:\n" +
                "Assets/Resources/Llave/OldKey"
            );

            return;
        }


        // =================================================
        // POSICIÓN DELANTE DEL JUGADOR
        // =================================================

        Vector3 direccion =
            transform.forward;

        direccion.y = 0f;

        if (direccion.sqrMagnitude < 0.01f)
        {
            direccion =
                Vector3.forward;
        }

        direccion.Normalize();


        Vector3 posicion =
            transform.position +
            direccion * distanciaLlave;

        posicion.y =
            transform.position.y +
            alturaLlave;


        // =================================================
        // INSTANCIAR
        // =================================================

        llaveFinal =
            Instantiate(
                prefabLlave,
                posicion,
                Quaternion.identity
            );

        llaveFinal.name =
            "OLDKEY_FINAL";


        // =================================================
        // HACERLA DE UN TAMAÑO VISIBLE
        // =================================================

        AjustarTamanoLlave();


        // =================================================
        // MOVIMIENTO
        // =================================================

        LlaveFinalBrillante brillo =
            llaveFinal.GetComponent<LlaveFinalBrillante>();

        if (brillo == null)
        {
            brillo =
                llaveFinal.AddComponent<LlaveFinalBrillante>();
        }

        brillo.velocidadGiro = 70f;
        brillo.alturaFlotacion = 0.20f;
        brillo.velocidadFlotacion = 2f;


        // =================================================
        // LUZ DORADA
        // =================================================

        CrearLuzLlave();


        llaveCreada = true;


        Debug.Log(
            "===================================="
        );

        Debug.Log(
            "¡OLDKEY HA APARECIDO!"
        );

        Debug.Log(
            "Acércate y recoge la antigua llave."
        );

        Debug.Log(
            "===================================="
        );
    }


    // =====================================================
    // AJUSTAR TAMAÑO REAL DEL MODELO
    // =====================================================

    void AjustarTamanoLlave()
    {
        if (llaveFinal == null)
            return;


        Renderer[] renderers =
            llaveFinal.GetComponentsInChildren<Renderer>(
                true
            );


        if (renderers.Length == 0)
        {
            // Si por alguna razón no encontramos renderer,
            // aumentamos igualmente la escala.
            llaveFinal.transform.localScale *= 3f;
            return;
        }


        Bounds bounds =
            renderers[0].bounds;


        for (
            int i = 1;
            i < renderers.Length;
            i++
        )
        {
            bounds.Encapsulate(
                renderers[i].bounds
            );
        }


        float tamanoActual =
            Mathf.Max(
                bounds.size.x,
                bounds.size.y,
                bounds.size.z
            );


        if (tamanoActual > 0.001f)
        {
            // Queremos que la llave mida aproximadamente
            // 1 metro para que sea muy fácil verla.
            float factor =
                1.0f / tamanoActual;

            llaveFinal.transform.localScale *=
                factor;
        }
    }


    // =====================================================
    // LUZ DE LA LLAVE
    // =====================================================

    void CrearLuzLlave()
    {
        if (llaveFinal == null)
            return;


        GameObject objetoLuz =
            new GameObject(
                "LUZ_OLDKEY"
            );

        objetoLuz.transform.SetParent(
            llaveFinal.transform,
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
                0.60f,
                0.10f
            );

        luz.intensity = 8f;
        luz.range = 10f;
        luz.shadows = LightShadows.None;
    }


    // =====================================================
    // RECOGER OLDKEY
    // =====================================================

    void RevisarRecogidaLlave()
    {
        if (llaveFinal == null)
            return;


        float distancia =
            Vector3.Distance(
                transform.position,
                llaveFinal.transform.position
            );


        if (
            distancia <=
            distanciaRecogerLlave
        )
        {
            RecogerLlaveFinal();
        }
    }


    void RecogerLlaveFinal()
    {
        if (llaveRecogida)
            return;


        llaveRecogida = true;


        Debug.Log(
            "¡OLDKEY RECOGIDA!"
        );


        if (llaveFinal != null)
        {
            Destroy(llaveFinal);
        }


        llaveFinal = null;


        // AHORA aparece el portal
        // DONDE ESTABA EL CUARTO FRAGMENTO.
        CrearSalida();
    }


    // =====================================================
    // CREAR SALIDA
    // =====================================================

    void CrearSalida()
    {
        if (salidaCreada)
            return;


        Vector3 posicion;


        if (posicionUltimoFragmentoGuardada)
        {
            posicion =
                posicionUltimoFragmento;
        }
        else
        {
            // Solo por seguridad.
            posicion =
                transform.position +
                transform.forward * 5f;
        }


        // Bajamos la posición hasta el suelo.
        // IMPORTANTE:
        // El rayo empieza cerca de la posición,
        // no desde el cielo, para evitar techos.
        RaycastHit hit;

        Vector3 origen =
            posicion +
            Vector3.up * 2f;


        if (
            Physics.Raycast(
                origen,
                Vector3.down,
                out hit,
                5f,
                ~0,
                QueryTriggerInteraction.Ignore
            )
        )
        {
            string nombre =
                hit.collider.gameObject.name.ToLower();


            // Si encuentra el propio suelo de la ciudad
            // utilizamos esa altura.
            if (
                !nombre.Contains("roof") &&
                !nombre.Contains("home") &&
                !nombre.Contains("house")
            )
            {
                posicion.y =
                    hit.point.y;
            }
        }


        // =================================================
        // OBJETO PRINCIPAL
        // =================================================

        salidaFinal =
            new GameObject(
                "PORTAL_SALIDA"
            );

        salidaFinal.transform.position =
            posicion;


        // Hacemos que mire aproximadamente
        // hacia el jugador.
        Vector3 haciaJugador =
            transform.position -
            posicion;

        haciaJugador.y = 0f;


        if (haciaJugador.sqrMagnitude > 0.01f)
        {
            salidaFinal.transform.rotation =
                Quaternion.LookRotation(
                    haciaJugador.normalized
                );
        }


        // =================================================
        // PILARES
        // =================================================

        CrearPartePortal(
            "PilarIzquierdo",
            new Vector3(
                -1.5f,
                1.8f,
                0f
            ),
            new Vector3(
                0.25f,
                3.6f,
                0.25f
            )
        );


        CrearPartePortal(
            "PilarDerecho",
            new Vector3(
                1.5f,
                1.8f,
                0f
            ),
            new Vector3(
                0.25f,
                3.6f,
                0.25f
            )
        );


        CrearPartePortal(
            "ParteSuperior",
            new Vector3(
                0f,
                3.6f,
                0f
            ),
            new Vector3(
                3.25f,
                0.25f,
                0.25f
            )
        );


        // =================================================
        // CENTRO DEL PORTAL
        // =================================================

        CrearCentroPortal();


        // =================================================
        // LUZ
        // =================================================

        GameObject objetoLuz =
            new GameObject(
                "LuzPortal"
            );

        objetoLuz.transform.SetParent(
            salidaFinal.transform,
            false
        );

        objetoLuz.transform.localPosition =
            new Vector3(
                0f,
                1.8f,
                0f
            );


        Light luz =
            objetoLuz.AddComponent<Light>();

        luz.type =
            LightType.Point;

        luz.color =
            new Color(
                0.20f,
                0.70f,
                1f
            );

        luz.intensity = 8f;
        luz.range = 14f;
        luz.shadows = LightShadows.None;


        // =================================================
        // SALIDA NIVEL
        // =================================================

        SalidaNivel salida =
            salidaFinal.AddComponent<SalidaNivel>();

        salida.distanciaActivacion =
            2.2f;


        salidaCreada = true;


        Debug.Log(
            "===================================="
        );

        Debug.Log(
            "¡PORTAL DESBLOQUEADO!"
        );

        Debug.Log(
            "El portal apareció donde encontraste " +
            "el cuarto fragmento."
        );

        Debug.Log(
            "===================================="
        );
    }


    // =====================================================
    // PIEZAS DEL PORTAL
    // =====================================================

    void CrearPartePortal(
        string nombre,
        Vector3 posicionLocal,
        Vector3 escala)
    {
        GameObject parte =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        parte.name =
            "Portal_" + nombre;

        parte.transform.SetParent(
            salidaFinal.transform,
            false
        );

        parte.transform.localPosition =
            posicionLocal;

        parte.transform.localScale =
            escala;


        AplicarMaterialPortal(
            parte,
            false
        );


        Collider collider =
            parte.GetComponent<Collider>();

        if (collider != null)
        {
            Destroy(collider);
        }
    }


    void CrearCentroPortal()
    {
        GameObject centro =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        centro.name =
            "CentroPortal";

        centro.transform.SetParent(
            salidaFinal.transform,
            false
        );

        centro.transform.localPosition =
            new Vector3(
                0f,
                1.8f,
                0f
            );

        centro.transform.localScale =
            new Vector3(
                2.7f,
                3.2f,
                0.06f
            );


        AplicarMaterialPortal(
            centro,
            true
        );


        Collider collider =
            centro.GetComponent<Collider>();

        if (collider != null)
        {
            Destroy(collider);
        }
    }


    // =====================================================
    // MATERIAL PORTAL
    // =====================================================

    void AplicarMaterialPortal(
        GameObject objeto,
        bool centro)
    {
        Renderer renderer =
            objeto.GetComponent<Renderer>();

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


        Color color;

        if (centro)
        {
            color =
                new Color(
                    0.25f,
                    0.85f,
                    1f
                );
        }
        else
        {
            color =
                new Color(
                    0.10f,
                    0.45f,
                    1f
                );
        }


        material.SetColor(
            "_BaseColor",
            color
        );


        material.EnableKeyword(
            "_EMISSION"
        );


        material.SetColor(
            "_EmissionColor",
            color * (centro ? 7f : 4f)
        );


        if (
            material.HasProperty(
                "_Metallic"
            )
        )
        {
            material.SetFloat(
                "_Metallic",
                centro ? 0.1f : 0.7f
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
                0.8f
            );
        }


        renderer.material =
            material;
    }


    // =====================================================
    // MÉTODOS PARA INTERFAZ
    // =====================================================

    public int ObtenerFragmentos()
    {
        return fragmentosRecogidos;
    }


    public int ObtenerTotalFragmentos()
    {
        return fragmentosNecesarios;
    }


    public int ObtenerPuntosNecesarios()
    {
        return puntosNecesarios;
    }


    public bool TieneFragmentosCompletos()
    {
        return
            fragmentosRecogidos >=
            fragmentosNecesarios;
    }


    public bool TieneLlaveCompleta()
    {
        return
            fragmentosRecogidos >=
            fragmentosNecesarios;
    }


    public bool LlaveHaAparecido()
    {
        return
            llaveCreada &&
            !llaveRecogida;
    }


    public bool TieneLlave()
    {
        return llaveRecogida;
    }


    public bool SalidaEstaCreada()
    {
        return salidaCreada;
    }
}