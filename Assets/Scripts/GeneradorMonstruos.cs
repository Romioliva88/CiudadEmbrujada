using System.Collections.Generic;
using UnityEngine;

public class GeneradorMonstruos : MonoBehaviour
{
    [Header("Referencias")]
    public CicloDiaNoche cicloDiaNoche;
    public Transform jugador;

    [Header("Monstruos")]
    public GameObject[] prefabsMonstruos;

    [Header("Cantidad")]
    public int maximoMonstruos = 20;
    public int cantidadMunecas = 8;

    [Header("Reaparición")]
    public float tiempoReaparicion = 10f;

    [Header("Distribución")]
    public float separacionMinima = 8f;
    public float distanciaMinimaJugador = 8f;
    public int intentosParaEncontrarLugar = 150;

    private List<GameObject> monstruosActivos =
        new List<GameObject>();

    private Bounds limitesPueblo;
    private bool limitesEncontrados = false;
    private bool poblacionCreada = false;

    private float temporizadorReaparicion = 0f;

    private GameObject prefabMuneca;

    // Cuando sea true, los monstruos desaparecen
    // y no pueden volver a aparecer.
    private bool maldicionTerminada = false;

    void Start()
    {
        BuscarReferencias();
        BuscarMuneca();
        CalcularLimitesPueblo();
    }

    void Update()
    {
        if (maldicionTerminada)
            return;

        if (cicloDiaNoche == null || jugador == null)
            return;

        if (cicloDiaNoche.EsDeNoche)
        {
            if (!poblacionCreada)
            {
                CrearPoblacionInicial();
                poblacionCreada = true;
            }

            LimpiarLista();

            if (monstruosActivos.Count < maximoMonstruos)
            {
                temporizadorReaparicion += Time.deltaTime;

                if (temporizadorReaparicion >= tiempoReaparicion)
                {
                    CrearReemplazo();

                    temporizadorReaparicion = 0f;
                }
            }
            else
            {
                temporizadorReaparicion = 0f;
            }
        }
        else
        {
            if (poblacionCreada)
            {
                EliminarTodosLosMonstruos();

                poblacionCreada = false;
            }

            temporizadorReaparicion = 0f;
        }
    }

    // =====================================================
    // REFERENCIAS
    // =====================================================

    void BuscarReferencias()
    {
        if (jugador == null)
        {
            GameObject objetoJugador =
                GameObject.Find("CuerpoJugador");

            if (objetoJugador != null)
            {
                jugador =
                    objetoJugador.transform;
            }
        }

        if (cicloDiaNoche == null)
        {
            cicloDiaNoche =
                FindFirstObjectByType<CicloDiaNoche>();
        }
    }

    // =====================================================
    // BUSCAR DEMON DOLL
    // =====================================================

    void BuscarMuneca()
    {
        prefabMuneca = null;

        if (prefabsMonstruos == null)
            return;

        foreach (GameObject prefab in prefabsMonstruos)
        {
            if (prefab == null)
                continue;

            if (prefab.name.Contains("DemonDoll"))
            {
                prefabMuneca = prefab;

                Debug.Log(
                    "Muñeca encontrada: " +
                    prefab.name
                );

                return;
            }
        }

        Debug.LogWarning(
            "No se encontró SKM_DemonDoll en Prefabs Monstruos."
        );
    }

    // =====================================================
    // LÍMITES DEL PUEBLO
    // =====================================================

    void CalcularLimitesPueblo()
    {
        GameObject escenario =
            GameObject.Find("ESCENARIO");

        if (escenario == null)
        {
            Debug.LogWarning(
                "No se encontró ESCENARIO."
            );

            return;
        }

        Renderer[] renderers =
            escenario.GetComponentsInChildren<Renderer>(true);

        bool primero = true;

        foreach (Renderer renderer in renderers)
        {
            if (!renderer.enabled)
                continue;

            if (primero)
            {
                limitesPueblo =
                    renderer.bounds;

                primero = false;
            }
            else
            {
                limitesPueblo.Encapsulate(
                    renderer.bounds
                );
            }
        }

        limitesEncontrados =
            !primero;

        if (limitesEncontrados)
        {
            Debug.Log(
                "Pueblo detectado correctamente."
            );
        }
    }

    // =====================================================
    // POBLACIÓN INICIAL
    // =====================================================

    void CrearPoblacionInicial()
    {
        if (maldicionTerminada)
            return;

        if (!limitesEncontrados)
            return;

        int munecasCreadas = 0;

        if (prefabMuneca != null)
        {
            int seguridad = 0;

            while (
                munecasCreadas < cantidadMunecas &&
                seguridad < 500)
            {
                if (CrearMonstruo(prefabMuneca))
                {
                    munecasCreadas++;
                }

                seguridad++;
            }
        }

        Debug.Log(
            "Muñecas creadas: " +
            munecasCreadas
        );

        int intentos = 0;

        while (
            monstruosActivos.Count < maximoMonstruos &&
            intentos < 500)
        {
            GameObject prefab =
                ElegirCreep();

            if (prefab != null)
            {
                CrearMonstruo(prefab);
            }

            intentos++;
        }

        Debug.Log(
            "Total enemigos creados: " +
            monstruosActivos.Count
        );
    }

    // =====================================================
    // ELEGIR CREEP
    // =====================================================

    GameObject ElegirCreep()
    {
        List<GameObject> creeps =
            new List<GameObject>();

        if (prefabsMonstruos == null)
            return null;

        foreach (GameObject prefab in prefabsMonstruos)
        {
            if (prefab == null)
                continue;

            if (!prefab.name.Contains("DemonDoll"))
            {
                creeps.Add(prefab);
            }
        }

        if (creeps.Count == 0)
            return prefabMuneca;

        return creeps[
            Random.Range(
                0,
                creeps.Count
            )
        ];
    }

    // =====================================================
    // ELEGIR GUARDIÁN
    // =====================================================

    GameObject ElegirMonstruoAleatorio()
    {
        if (prefabsMonstruos == null ||
            prefabsMonstruos.Length == 0)
        {
            return null;
        }

        List<GameObject> disponibles =
            new List<GameObject>();

        foreach (GameObject prefab in prefabsMonstruos)
        {
            if (prefab != null)
            {
                disponibles.Add(prefab);
            }
        }

        if (disponibles.Count == 0)
            return null;

        return disponibles[
            Random.Range(
                0,
                disponibles.Count
            )
        ];
    }

    // =====================================================
    // REEMPLAZAR MONSTRUOS ELIMINADOS
    // =====================================================

    void CrearReemplazo()
    {
        if (maldicionTerminada)
            return;

        int munecasActuales =
            ContarMunecas();

        if (munecasActuales < cantidadMunecas &&
            prefabMuneca != null)
        {
            CrearMonstruo(
                prefabMuneca
            );
        }
        else
        {
            GameObject prefab =
                ElegirCreep();

            if (prefab != null)
            {
                CrearMonstruo(prefab);
            }
        }
    }

    int ContarMunecas()
    {
        int cantidad = 0;

        foreach (GameObject monstruo in monstruosActivos)
        {
            if (monstruo == null)
                continue;

            if (monstruo.name.Contains("DemonDoll"))
            {
                cantidad++;
            }
        }

        return cantidad;
    }

    // =====================================================
    // CREAR MONSTRUO NORMAL
    // =====================================================

    bool CrearMonstruo(GameObject prefab)
    {
        if (maldicionTerminada)
            return false;

        if (prefab == null)
            return false;

        Vector3 posicion;

        if (!BuscarPosicionDisponible(
            out posicion))
        {
            return false;
        }

        GameObject nuevoMonstruo =
            Instantiate(
                prefab,
                posicion,
                Quaternion.Euler(
                    0f,
                    Random.Range(
                        0f,
                        360f
                    ),
                    0f
                )
            );

        PrepararMonstruo(
            nuevoMonstruo
        );

        monstruosActivos.Add(
            nuevoMonstruo
        );

        return true;
    }

    // =====================================================
    // CREAR GUARDIÁN DE FRAGMENTO
    // =====================================================

    public GameObject CrearGuardian(
        Vector3 posicionFragmento)
    {
        if (maldicionTerminada)
            return null;

        GameObject prefab =
            ElegirMonstruoAleatorio();

        if (prefab == null)
        {
            Debug.LogWarning(
                "No hay prefab disponible para crear guardián."
            );

            return null;
        }

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
                2.5f,
                4f
            );

        Vector3 posicion =
            posicionFragmento +
            new Vector3(
                direccion2D.x * distancia,
                0f,
                direccion2D.y * distancia
            );

        RaycastHit golpe;

        Vector3 origen =
            posicion +
            Vector3.up * 30f;

        if (Physics.Raycast(
            origen,
            Vector3.down,
            out golpe,
            100f))
        {
            posicion =
                golpe.point;
        }

        GameObject guardian =
            Instantiate(
                prefab,
                posicion,
                Quaternion.Euler(
                    0f,
                    Random.Range(
                        0f,
                        360f
                    ),
                    0f
                )
            );

        guardian.name =
            "GuardianFragmento_" +
            prefab.name;

        PrepararMonstruo(
            guardian
        );

        monstruosActivos.Add(
            guardian
        );

        Debug.Log(
            "Guardián creado para un fragmento."
        );

        return guardian;
    }

    // =====================================================
    // TERMINAR MALDICIÓN
    // =====================================================

    public void TerminarMaldicion()
    {
        if (maldicionTerminada)
            return;

        maldicionTerminada = true;

        temporizadorReaparicion = 0f;
        poblacionCreada = false;

        EliminarTodosLosMonstruos();

        Debug.Log(
            "¡La maldición terminó! " +
            "Todos los monstruos desaparecieron."
        );
    }

    public bool MaldicionTerminada()
    {
        return maldicionTerminada;
    }

    // =====================================================
    // CONFIGURACIÓN DE TODOS LOS MONSTRUOS
    // =====================================================

    void PrepararMonstruo(
        GameObject nuevoMonstruo)
    {
        Monstruo monstruo =
            nuevoMonstruo.GetComponent<Monstruo>();

        if (monstruo == null)
        {
            monstruo =
                nuevoMonstruo.AddComponent<Monstruo>();
        }

        EnemigoFantasma enemigo =
            nuevoMonstruo.GetComponent<EnemigoFantasma>();

        if (enemigo == null)
        {
            enemigo =
                nuevoMonstruo.AddComponent<EnemigoFantasma>();
        }

        enemigo.jugador =
            jugador;

        // Detectan al jugador desde lejos.
        enemigo.distanciaDeteccion =
            22f;

        // Si logras alejarte bastante,
        // dejan de perseguirte.
        enemigo.distanciaAbandono =
            30f;

        // Suficientemente rápidos para dar miedo,
        // pero el jugador puede escapar.
        enemigo.velocidad =
            2f;

        enemigo.distanciaMinimaJugador =
            1.4f;

        // Deben acercarse bastante para atacar.
        enemigo.distanciaAtaque =
            1.7f;

        // Daño reducido.
        enemigo.danoAtaque =
            5;

        // Tiempo mayor entre ataques.
        enemigo.tiempoEntreAtaques =
            2.5f;

        CapsuleCollider capsule =
            nuevoMonstruo.GetComponent<CapsuleCollider>();

        if (capsule == null)
        {
            capsule =
                nuevoMonstruo.AddComponent<CapsuleCollider>();
        }

        capsule.center =
            new Vector3(
                0f,
                1.2f,
                0f
            );

        capsule.radius =
            0.8f;

        capsule.height =
            2.4f;

        capsule.isTrigger =
            false;

        if (nuevoMonstruo.name.Contains("DemonDoll"))
        {
            CorregirDemonDoll corregir =
                nuevoMonstruo.GetComponent<CorregirDemonDoll>();

            if (corregir == null)
            {
                nuevoMonstruo.AddComponent<CorregirDemonDoll>();
            }
        }
    }

    // =====================================================
    // BUSCAR POSICIÓN
    // =====================================================

    bool BuscarPosicionDisponible(
        out Vector3 posicionFinal)
    {
        for (
            int intento = 0;
            intento < intentosParaEncontrarLugar;
            intento++)
        {
            float x =
                Random.Range(
                    limitesPueblo.min.x,
                    limitesPueblo.max.x
                );

            float z =
                Random.Range(
                    limitesPueblo.min.z,
                    limitesPueblo.max.z
                );

            Vector3 origen =
                new Vector3(
                    x,
                    limitesPueblo.max.y + 30f,
                    z
                );

            RaycastHit golpe;

            if (!Physics.Raycast(
                origen,
                Vector3.down,
                out golpe,
                limitesPueblo.size.y + 100f))
            {
                continue;
            }

            Vector3 posicion =
                golpe.point;

            if (jugador != null)
            {
                float distanciaJugador =
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

                if (distanciaJugador <
                    distanciaMinimaJugador)
                {
                    continue;
                }
            }

            if (!LugarEstaLibre(
                posicion))
            {
                continue;
            }

            posicionFinal =
                posicion;

            return true;
        }

        posicionFinal =
            Vector3.zero;

        return false;
    }

    bool LugarEstaLibre(
        Vector3 posicion)
    {
        foreach (
            GameObject monstruo
            in monstruosActivos)
        {
            if (monstruo == null)
                continue;

            float distancia =
                Vector2.Distance(
                    new Vector2(
                        posicion.x,
                        posicion.z
                    ),
                    new Vector2(
                        monstruo.transform.position.x,
                        monstruo.transform.position.z
                    )
                );

            if (distancia <
                separacionMinima)
            {
                return false;
            }
        }

        return true;
    }

    // =====================================================
    // LIMPIEZA
    // =====================================================

    void LimpiarLista()
    {
        monstruosActivos.RemoveAll(
            monstruo =>
                monstruo == null
        );
    }

    void EliminarTodosLosMonstruos()
    {
        Monstruo[] todos =
            FindObjectsByType<Monstruo>(
                FindObjectsSortMode.None
            );

        foreach (Monstruo monstruo in todos)
        {
            if (monstruo != null)
            {
                Destroy(
                    monstruo.gameObject
                );
            }
        }

        monstruosActivos.Clear();
    }
}