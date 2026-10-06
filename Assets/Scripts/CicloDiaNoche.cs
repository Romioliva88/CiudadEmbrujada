using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Header("Sol")]
    public Light sol;

    [Header("Maldición")]
    [Range(0f, 24f)]
    public float horaNocheEterna = 0f;

    [Header("Amanecer final")]
    [Range(0f, 24f)]
    public float horaFinalAmanecer = 8f;

    public float duracionAmanecer = 8f;

    private float horaActual;
    private bool maldicionActiva = true;
    private bool amaneciendo = false;

    private float tiempoAmanecer = 0f;
    private float horaInicioAmanecer;

    // =====================================================
    // ESTADO DE NOCHE
    // =====================================================

    public bool EsDeNoche
    {
        get
        {
            // Mientras exista la maldición,
            // SIEMPRE es de noche.
            if (maldicionActiva)
                return true;

            return horaActual >= 19f ||
                   horaActual < 6f;
        }
    }

    // =====================================================
    // INICIO
    // =====================================================

    void Start()
    {
        // La ciudad comienza atrapada
        // en una noche eterna.
        horaActual = horaNocheEterna;

        AplicarIluminacion();
    }

    // =====================================================
    // ACTUALIZACIÓN
    // =====================================================

    void Update()
    {
        // Mientras la maldición siga activa,
        // la hora NO avanza.
        if (maldicionActiva)
        {
            horaActual = horaNocheEterna;

            AplicarIluminacion();

            return;
        }

        // Cuando se rompe el hechizo,
        // comienza el amanecer.
        if (amaneciendo)
        {
            ActualizarAmanecer();
        }
    }

    // =====================================================
    // ROMPER EL HECHIZO
    // =====================================================

    public void RomperHechizo()
    {
        if (!maldicionActiva)
            return;

        maldicionActiva = false;
        amaneciendo = true;

        tiempoAmanecer = 0f;
        horaInicioAmanecer = horaActual;

        Debug.Log(
            "¡La maldición se rompió! " +
            "La noche eterna está terminando."
        );
    }

    // =====================================================
    // AMANECER
    // =====================================================

    void ActualizarAmanecer()
    {
        tiempoAmanecer += Time.deltaTime;

        float porcentaje =
            Mathf.Clamp01(
                tiempoAmanecer /
                duracionAmanecer
            );

        // Como comenzamos alrededor de medianoche
        // avanzamos lentamente hasta las 08:00.
        horaActual =
            Mathf.Lerp(
                horaInicioAmanecer,
                horaFinalAmanecer,
                porcentaje
            );

        AplicarIluminacion();

        if (porcentaje >= 1f)
        {
            amaneciendo = false;

            horaActual =
                horaFinalAmanecer;

            AplicarIluminacion();

            Debug.Log(
                "¡Ha amanecido en la ciudad!"
            );
        }
    }

    // =====================================================
    // ILUMINACIÓN
    // =====================================================

    void AplicarIluminacion()
    {
        if (sol == null)
            return;

        float rotacionSol =
            (horaActual / 24f) *
            360f -
            90f;

        sol.transform.rotation =
            Quaternion.Euler(
                rotacionSol,
                170f,
                0f
            );

        float intensidad;

        if (maldicionActiva)
        {
            // Noche oscura mientras
            // exista la maldición.
            intensidad = 0.05f;
        }
        else if (horaActual < 6f)
        {
            intensidad = 0.05f;
        }
        else if (horaActual < 8f)
        {
            // Entre las 06:00 y las 08:00
            // aumenta progresivamente la luz.
            intensidad =
                Mathf.Lerp(
                    0.05f,
                    1f,
                    Mathf.InverseLerp(
                        6f,
                        8f,
                        horaActual
                    )
                );
        }
        else
        {
            intensidad = 1f;
        }

        sol.intensity =
            intensidad;
    }

    // =====================================================
    // CONSULTAS
    // =====================================================

    public bool MaldicionActiva()
    {
        return maldicionActiva;
    }

    public bool EstaAmaneciendo()
    {
        return amaneciendo;
    }

    public float ObtenerHoraActual()
    {
        return horaActual;
    }
}