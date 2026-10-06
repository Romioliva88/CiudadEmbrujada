using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InterfazJugador : MonoBehaviour
{
    [Header("Jugador")]
    public EnergiaJugador energiaJugador;
    public SistemaFragmentos sistemaFragmentos;

    [Header("Texto Energía")]
    public TextMeshProUGUI textoEnergia;

    private TextMeshProUGUI textoFragmentos;
    private TextMeshProUGUI textoPuntos;

    private Image fondoEnergia;
    private Image barraEnergia;
    private Image fondoFragmentos;
    private Image fondoPuntos;

    // Pantalla inicial
    private GameObject panelIntroduccion;
    private bool introduccionActiva = true;

    // Pantalla final
    private GameObject panelFinal;
    private TextMeshProUGUI tituloFinal;
    private TextMeshProUGUI mensajeFinal;
    private bool finalMostrado = false;

    // Pantalla derrota
    private GameObject panelDerrota;
    private bool derrotaMostrada = false;


    // =====================================================
    // INICIO
    // =====================================================

    void Start()
    {
        BuscarReferencias();
        ConfigurarInterfaz();
        ActualizarInterfaz();

        CrearIntroduccion();

        // Pausamos el juego mientras se lee la historia.
        Time.timeScale = 0f;
    }


    void Update()
    {
        if (introduccionActiva)
        {
            RevisarInicioJuego();
            return;
        }

        ActualizarInterfaz();
    }


    // =====================================================
    // INTRODUCCIÓN
    // =====================================================

    void CrearIntroduccion()
    {
        panelIntroduccion =
            new GameObject(
                "PanelIntroduccion",
                typeof(RectTransform),
                typeof(Image)
            );

        panelIntroduccion.transform.SetParent(
            transform,
            false
        );

        panelIntroduccion.transform.SetAsLastSibling();

        RectTransform rectPanel =
            panelIntroduccion.GetComponent<RectTransform>();

        rectPanel.anchorMin = Vector2.zero;
        rectPanel.anchorMax = Vector2.one;
        rectPanel.offsetMin = Vector2.zero;
        rectPanel.offsetMax = Vector2.zero;

        Image fondo =
            panelIntroduccion.GetComponent<Image>();

        fondo.color =
            new Color(
                0.01f,
                0.01f,
                0.015f,
                0.96f
            );

        fondo.raycastTarget = false;


        // =================================================
        // TÍTULO
        // =================================================

        GameObject objetoTitulo =
            new GameObject(
                "TituloIntroduccion",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoTitulo.transform.SetParent(
            panelIntroduccion.transform,
            false
        );

        TextMeshProUGUI titulo =
            objetoTitulo.GetComponent<TextMeshProUGUI>();

        RectTransform rectTitulo =
            titulo.rectTransform;

        // Anclado arriba para que siempre sea visible.
        rectTitulo.anchorMin =
            new Vector2(0.5f, 1f);

        rectTitulo.anchorMax =
            new Vector2(0.5f, 1f);

        rectTitulo.pivot =
            new Vector2(0.5f, 1f);

        rectTitulo.anchoredPosition =
            new Vector2(0f, -30f);

        rectTitulo.sizeDelta =
            new Vector2(700f, 55f);

        titulo.text =
            "CIUDAD EMBRUJADA";

        titulo.fontSize = 25f;

        titulo.enableAutoSizing = true;
        titulo.fontSizeMin = 18f;
        titulo.fontSizeMax = 25f;

        titulo.fontStyle =
            FontStyles.Bold;

        titulo.alignment =
            TextAlignmentOptions.Center;

        titulo.color =
            new Color(
                0.95f,
                0.95f,
                1f
            );

        titulo.raycastTarget = false;


        // =================================================
        // HISTORIA
        // =================================================

        GameObject objetoHistoria =
            new GameObject(
                "HistoriaIntroduccion",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoHistoria.transform.SetParent(
            panelIntroduccion.transform,
            false
        );

        TextMeshProUGUI historia =
            objetoHistoria.GetComponent<TextMeshProUGUI>();

        RectTransform rectHistoria =
            historia.rectTransform;

        // Usamos casi toda la pantalla,
        // dejando espacio arriba y abajo.
        rectHistoria.anchorMin =
            new Vector2(0.08f, 0.18f);

        rectHistoria.anchorMax =
            new Vector2(0.92f, 0.82f);

        rectHistoria.offsetMin =
            Vector2.zero;

        rectHistoria.offsetMax =
            Vector2.zero;

        historia.text =
            "Una bruja ha condenado la ciudad a una noche eterna.\n\n" +

            "Para romper el hechizo deberás encontrar los " +
            "<b>4 fragmentos de la antigua llave</b>.\n\n" +

            "Pero encontrarlos no será suficiente...\n\n" +

            "Las criaturas de la ciudad guardan " +
            "<b>Puntos de Luz</b>.\n" +

            "Derrótalas con tu Espada de Luz y consigue " +
            "<b>1.000 Puntos de Luz</b>.\n\n" +

            "Cuando completes ambos objetivos aparecerá " +
            "<b>la antigua llave</b>.\n\n" +

            "<b>Recógela, encuentra la salida y rompe la maldición.</b>";

        historia.fontSize = 14f;

        historia.enableAutoSizing = true;
        historia.fontSizeMin = 9f;
        historia.fontSizeMax = 14f;

        historia.alignment =
            TextAlignmentOptions.Center;

        historia.color =
            new Color(
                0.84f,
                0.86f,
                0.90f
            );

        historia.raycastTarget = false;


        // =================================================
        // ENTER PARA COMENZAR
        // =================================================

        GameObject objetoContinuar =
            new GameObject(
                "TextoComenzar",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoContinuar.transform.SetParent(
            panelIntroduccion.transform,
            false
        );

        TextMeshProUGUI continuar =
            objetoContinuar.GetComponent<TextMeshProUGUI>();

        RectTransform rectContinuar =
            continuar.rectTransform;

        // Anclado abajo para que nunca quede fuera.
        rectContinuar.anchorMin =
            new Vector2(0.5f, 0f);

        rectContinuar.anchorMax =
            new Vector2(0.5f, 0f);

        rectContinuar.pivot =
            new Vector2(0.5f, 0f);

        rectContinuar.anchoredPosition =
            new Vector2(0f, 25f);

        rectContinuar.sizeDelta =
            new Vector2(650f, 45f);

        continuar.text =
            "PRESIONA ENTER PARA COMENZAR";

        continuar.fontSize = 16f;

        continuar.enableAutoSizing = true;
        continuar.fontSizeMin = 11f;
        continuar.fontSizeMax = 16f;

        continuar.fontStyle =
            FontStyles.Bold;

        continuar.alignment =
            TextAlignmentOptions.Center;

        continuar.color =
            new Color(
                1f,
                0.82f,
                0.35f
            );

        continuar.raycastTarget = false;
    }


    // =====================================================
    // COMENZAR CON ENTER
    // =====================================================

    void RevisarInicioJuego()
    {
        if (Keyboard.current == null)
            return;

        if (
            Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.numpadEnterKey.wasPressedThisFrame
        )
        {
            ComenzarJuego();
        }
    }


    void ComenzarJuego()
    {
        introduccionActiva = false;

        if (panelIntroduccion != null)
        {
            Destroy(panelIntroduccion);
        }

        Time.timeScale = 1f;

        Debug.Log(
            "La partida ha comenzado."
        );
    }


    // =====================================================
    // REFERENCIAS
    // =====================================================

    void BuscarReferencias()
    {
        if (energiaJugador == null)
        {
            energiaJugador =
                FindFirstObjectByType<EnergiaJugador>();
        }

        if (sistemaFragmentos == null)
        {
            sistemaFragmentos =
                FindFirstObjectByType<SistemaFragmentos>();
        }

        if (textoEnergia == null)
        {
            GameObject objetoTexto =
                GameObject.Find("TextoEnergia");

            if (objetoTexto != null)
            {
                textoEnergia =
                    objetoTexto.GetComponent<TextMeshProUGUI>();
            }
        }
    }


    // =====================================================
    // CONFIGURACIÓN GENERAL
    // =====================================================

    void ConfigurarInterfaz()
    {
        if (textoEnergia == null)
        {
            Debug.LogWarning(
                "No se encontró TextoEnergia."
            );

            return;
        }

        CrearFondoEnergia();
        ConfigurarTextoEnergia();
        CrearBarraEnergia();

        CrearFondoFragmentos();
        CrearTextoFragmentos();

        CrearFondoPuntos();
        CrearTextoPuntos();
    }


    // =====================================================
    // ENERGÍA
    // =====================================================

    void CrearFondoEnergia()
    {
        GameObject objetoFondo =
            GameObject.Find("FondoEnergia");

        if (objetoFondo == null)
        {
            objetoFondo =
                new GameObject(
                    "FondoEnergia",
                    typeof(RectTransform),
                    typeof(Image)
                );

            objetoFondo.transform.SetParent(
                transform,
                false
            );
        }

        objetoFondo.transform.SetAsFirstSibling();

        RectTransform rect =
            objetoFondo.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(14f, -14f);

        rect.sizeDelta =
            new Vector2(180f, 43f);

        fondoEnergia =
            objetoFondo.GetComponent<Image>();

        fondoEnergia.color =
            new Color(
                0f,
                0f,
                0f,
                0.55f
            );

        fondoEnergia.raycastTarget = false;
    }


    void ConfigurarTextoEnergia()
    {
        RectTransform rect =
            textoEnergia.rectTransform;

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(22f, -19f);

        rect.sizeDelta =
            new Vector2(165f, 20f);

        textoEnergia.fontSize = 13f;

        textoEnergia.fontStyle =
            FontStyles.Bold;

        textoEnergia.alignment =
            TextAlignmentOptions.Left;

        textoEnergia.color =
            new Color(
                0.9f,
                0.92f,
                0.95f
            );

        textoEnergia.raycastTarget = false;
    }


    void CrearBarraEnergia()
    {
        GameObject fondoBarra =
            GameObject.Find("FondoBarraEnergia");

        if (fondoBarra == null)
        {
            fondoBarra =
                new GameObject(
                    "FondoBarraEnergia",
                    typeof(RectTransform),
                    typeof(Image)
                );

            fondoBarra.transform.SetParent(
                transform,
                false
            );
        }

        RectTransform rectFondo =
            fondoBarra.GetComponent<RectTransform>();

        rectFondo.anchorMin =
            new Vector2(0f, 1f);

        rectFondo.anchorMax =
            new Vector2(0f, 1f);

        rectFondo.pivot =
            new Vector2(0f, 1f);

        rectFondo.anchoredPosition =
            new Vector2(22f, -42f);

        rectFondo.sizeDelta =
            new Vector2(145f, 4f);

        Image imagenFondo =
            fondoBarra.GetComponent<Image>();

        imagenFondo.color =
            new Color(
                0.12f,
                0.12f,
                0.14f,
                0.9f
            );

        imagenFondo.raycastTarget = false;


        GameObject objetoBarra =
            GameObject.Find("BarraEnergia");

        if (objetoBarra == null)
        {
            objetoBarra =
                new GameObject(
                    "BarraEnergia",
                    typeof(RectTransform),
                    typeof(Image)
                );

            objetoBarra.transform.SetParent(
                fondoBarra.transform,
                false
            );
        }

        RectTransform rectBarra =
            objetoBarra.GetComponent<RectTransform>();

        rectBarra.anchorMin =
            new Vector2(0f, 0f);

        rectBarra.anchorMax =
            new Vector2(1f, 1f);

        rectBarra.pivot =
            new Vector2(0f, 0.5f);

        rectBarra.offsetMin =
            Vector2.zero;

        rectBarra.offsetMax =
            Vector2.zero;

        barraEnergia =
            objetoBarra.GetComponent<Image>();

        barraEnergia.raycastTarget = false;
    }


    // =====================================================
    // FRAGMENTOS
    // =====================================================

    void CrearFondoFragmentos()
    {
        GameObject objetoFondo =
            GameObject.Find("FondoFragmentos");

        if (objetoFondo == null)
        {
            objetoFondo =
                new GameObject(
                    "FondoFragmentos",
                    typeof(RectTransform),
                    typeof(Image)
                );

            objetoFondo.transform.SetParent(
                transform,
                false
            );
        }

        RectTransform rect =
            objetoFondo.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(14f, -63f);

        rect.sizeDelta =
            new Vector2(180f, 28f);

        fondoFragmentos =
            objetoFondo.GetComponent<Image>();

        fondoFragmentos.color =
            new Color(
                0f,
                0f,
                0f,
                0.55f
            );

        fondoFragmentos.raycastTarget = false;
    }


    void CrearTextoFragmentos()
    {
        GameObject objetoTexto =
            GameObject.Find("TextoFragmentos");

        if (objetoTexto == null)
        {
            objetoTexto =
                new GameObject(
                    "TextoFragmentos",
                    typeof(RectTransform),
                    typeof(TextMeshProUGUI)
                );

            objetoTexto.transform.SetParent(
                transform,
                false
            );
        }

        textoFragmentos =
            objetoTexto.GetComponent<TextMeshProUGUI>();

        RectTransform rect =
            textoFragmentos.rectTransform;

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(22f, -68f);

        rect.sizeDelta =
            new Vector2(185f, 20f);

        textoFragmentos.fontSize = 13f;

        textoFragmentos.fontStyle =
            FontStyles.Bold;

        textoFragmentos.alignment =
            TextAlignmentOptions.Left;

        textoFragmentos.color =
            new Color(
                0.82f,
                0.84f,
                0.88f
            );

        textoFragmentos.raycastTarget = false;

        textoFragmentos.text =
            "FRAGMENTOS  0/4";
    }


    // =====================================================
    // PUNTOS DE LUZ
    // =====================================================

    void CrearFondoPuntos()
    {
        GameObject objetoFondo =
            GameObject.Find("FondoPuntosLuz");

        if (objetoFondo == null)
        {
            objetoFondo =
                new GameObject(
                    "FondoPuntosLuz",
                    typeof(RectTransform),
                    typeof(Image)
                );

            objetoFondo.transform.SetParent(
                transform,
                false
            );
        }

        RectTransform rect =
            objetoFondo.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(14f, -96f);

        rect.sizeDelta =
            new Vector2(235f, 30f);

        fondoPuntos =
            objetoFondo.GetComponent<Image>();

        fondoPuntos.color =
            new Color(
                0f,
                0f,
                0f,
                0.55f
            );

        fondoPuntos.raycastTarget = false;
    }


    void CrearTextoPuntos()
    {
        GameObject objetoTexto =
            GameObject.Find("TextoPuntosLuz");

        if (objetoTexto == null)
        {
            objetoTexto =
                new GameObject(
                    "TextoPuntosLuz",
                    typeof(RectTransform),
                    typeof(TextMeshProUGUI)
                );

            objetoTexto.transform.SetParent(
                transform,
                false
            );
        }

        textoPuntos =
            objetoTexto.GetComponent<TextMeshProUGUI>();

        RectTransform rect =
            textoPuntos.rectTransform;

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            new Vector2(22f, -101f);

        rect.sizeDelta =
            new Vector2(220f, 22f);

        textoPuntos.fontSize = 13f;

        textoPuntos.fontStyle =
            FontStyles.Bold;

        textoPuntos.alignment =
            TextAlignmentOptions.Left;

        textoPuntos.color =
            new Color(
                1f,
                0.82f,
                0.35f
            );

        textoPuntos.raycastTarget = false;

        textoPuntos.text =
            "PUNTOS DE LUZ  0/1000";
    }


    // =====================================================
    // ACTUALIZAR HUD
    // =====================================================

    void ActualizarInterfaz()
    {
        ActualizarEnergia();
        ActualizarFragmentos();
        ActualizarPuntos();
    }


    void ActualizarEnergia()
    {
        if (energiaJugador == null)
        {
            energiaJugador =
                FindFirstObjectByType<EnergiaJugador>();
        }

        if (energiaJugador == null ||
            textoEnergia == null)
        {
            return;
        }

        int actual =
            energiaJugador.ObtenerEnergiaActual();

        int maxima =
            energiaJugador.ObtenerEnergiaMaxima();

        textoEnergia.text =
            "ENERGÍA  " +
            actual +
            "/" +
            maxima;

        float porcentaje =
            (float)actual / maxima;

        if (barraEnergia == null)
            return;

        RectTransform rect =
            barraEnergia.rectTransform;

        rect.anchorMax =
            new Vector2(
                porcentaje,
                1f
            );

        if (porcentaje > 0.6f)
        {
            barraEnergia.color =
                new Color(
                    0.70f,
                    0.75f,
                    0.82f
                );
        }
        else if (porcentaje > 0.3f)
        {
            barraEnergia.color =
                new Color(
                    0.85f,
                    0.55f,
                    0.12f
                );
        }
        else
        {
            barraEnergia.color =
                new Color(
                    0.75f,
                    0.08f,
                    0.08f
                );
        }
    }


    void ActualizarFragmentos()
    {
        if (sistemaFragmentos == null)
        {
            sistemaFragmentos =
                FindFirstObjectByType<SistemaFragmentos>();
        }

        if (sistemaFragmentos == null ||
            textoFragmentos == null)
        {
            return;
        }

        int recogidos =
            sistemaFragmentos.ObtenerFragmentos();

        int total =
            sistemaFragmentos.ObtenerTotalFragmentos();

        textoFragmentos.text =
            "FRAGMENTOS  " +
            recogidos +
            "/" +
            total;

        if (recogidos >= total)
        {
            // Usamos LISTO para evitar el cuadrado
            // que estaba mostrando la fuente con ✓.
            textoFragmentos.text =
                "FRAGMENTOS  " +
                total +
                "/" +
                total +
                "  LISTO";

            textoFragmentos.color =
                new Color(
                    0.88f,
                    0.90f,
                    0.94f
                );
        }
    }


    void ActualizarPuntos()
    {
        if (textoPuntos == null)
            return;

        int puntosActuales = 0;

        if (SistemaPuntos.instancia != null)
        {
            puntosActuales =
                SistemaPuntos.instancia.ObtenerPuntos();
        }

        int puntosMostrados =
            Mathf.Min(
                puntosActuales,
                1000
            );

        textoPuntos.text =
            "PUNTOS DE LUZ  " +
            puntosMostrados +
            "/1000";

        if (puntosActuales >= 1000)
        {
            // El puntaje real sigue guardado.
            // Solo el objetivo visual queda en 1000/1000.
            textoPuntos.text =
                "PUNTOS DE LUZ  1000/1000  LISTO";

            textoPuntos.color =
                new Color(
                    1f,
                    0.90f,
                    0.40f
                );
        }
    }


    // =====================================================
    // DERROTA
    // =====================================================

    public void MostrarDerrota()
    {
        if (derrotaMostrada)
            return;

        derrotaMostrada = true;

        CrearPanelDerrota();

        Debug.Log(
            "Pantalla de derrota mostrada."
        );
    }


    void CrearPanelDerrota()
    {
        panelDerrota =
            new GameObject(
                "PanelDerrota",
                typeof(RectTransform),
                typeof(Image)
            );

        panelDerrota.transform.SetParent(
            transform,
            false
        );

        panelDerrota.transform.SetAsLastSibling();

        RectTransform rectPanel =
            panelDerrota.GetComponent<RectTransform>();

        rectPanel.anchorMin =
            Vector2.zero;

        rectPanel.anchorMax =
            Vector2.one;

        rectPanel.offsetMin =
            Vector2.zero;

        rectPanel.offsetMax =
            Vector2.zero;

        Image imagenPanel =
            panelDerrota.GetComponent<Image>();

        imagenPanel.color =
            new Color(
                0.01f,
                0.01f,
                0.015f,
                0.94f
            );

        imagenPanel.raycastTarget = false;


        GameObject objetoTitulo =
            new GameObject(
                "TituloDerrota",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoTitulo.transform.SetParent(
            panelDerrota.transform,
            false
        );

        TextMeshProUGUI titulo =
            objetoTitulo.GetComponent<TextMeshProUGUI>();

        RectTransform rectTitulo =
            titulo.rectTransform;

        rectTitulo.anchorMin =
            new Vector2(0.5f, 0.5f);

        rectTitulo.anchorMax =
            new Vector2(0.5f, 0.5f);

        rectTitulo.pivot =
            new Vector2(0.5f, 0.5f);

        rectTitulo.anchoredPosition =
            new Vector2(0f, 55f);

        rectTitulo.sizeDelta =
            new Vector2(600f, 70f);

        titulo.text =
            "PERDISTE";

        titulo.fontSize = 38f;

        titulo.fontStyle =
            FontStyles.Bold;

        titulo.alignment =
            TextAlignmentOptions.Center;

        titulo.color =
            new Color(
                0.85f,
                0.12f,
                0.12f
            );

        titulo.raycastTarget = false;


        GameObject objetoMensaje =
            new GameObject(
                "MensajeDerrota",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoMensaje.transform.SetParent(
            panelDerrota.transform,
            false
        );

        TextMeshProUGUI mensaje =
            objetoMensaje.GetComponent<TextMeshProUGUI>();

        RectTransform rectMensaje =
            mensaje.rectTransform;

        rectMensaje.anchorMin =
            new Vector2(0.5f, 0.5f);

        rectMensaje.anchorMax =
            new Vector2(0.5f, 0.5f);

        rectMensaje.pivot =
            new Vector2(0.5f, 0.5f);

        rectMensaje.anchoredPosition =
            new Vector2(0f, -25f);

        rectMensaje.sizeDelta =
            new Vector2(650f, 100f);

        mensaje.text =
            "Inténtalo de nuevo.\n¡No te rindas!";

        mensaje.fontSize = 22f;

        mensaje.fontStyle =
            FontStyles.Bold;

        mensaje.alignment =
            TextAlignmentOptions.Center;

        mensaje.color =
            new Color(
                0.92f,
                0.94f,
                0.98f
            );

        mensaje.raycastTarget = false;
    }


    // =====================================================
    // FINAL DEL JUEGO
    // =====================================================

    public void MostrarMensajeFinal()
    {
        if (finalMostrado)
            return;

        finalMostrado = true;

        CrearPanelFinal();

        Debug.Log(
            "Mensaje final mostrado."
        );
    }


    void CrearPanelFinal()
    {
        panelFinal =
            new GameObject(
                "PanelFinal",
                typeof(RectTransform),
                typeof(Image)
            );

        panelFinal.transform.SetParent(
            transform,
            false
        );

        panelFinal.transform.SetAsLastSibling();

        RectTransform rectPanel =
            panelFinal.GetComponent<RectTransform>();

        rectPanel.anchorMin =
            new Vector2(0.5f, 0.5f);

        rectPanel.anchorMax =
            new Vector2(0.5f, 0.5f);

        rectPanel.pivot =
            new Vector2(0.5f, 0.5f);

        rectPanel.anchoredPosition =
            Vector2.zero;

        rectPanel.sizeDelta =
            new Vector2(
                600f,
                260f
            );

        Image imagenPanel =
            panelFinal.GetComponent<Image>();

        imagenPanel.color =
            new Color(
                0.02f,
                0.02f,
                0.025f,
                0.92f
            );

        imagenPanel.raycastTarget = false;

        CrearTituloFinal();
        CrearTextoFinal();
    }


    void CrearTituloFinal()
    {
        GameObject objetoTitulo =
            new GameObject(
                "TituloFinal",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoTitulo.transform.SetParent(
            panelFinal.transform,
            false
        );

        tituloFinal =
            objetoTitulo.GetComponent<TextMeshProUGUI>();

        RectTransform rect =
            tituloFinal.rectTransform;

        rect.anchorMin =
            new Vector2(0.5f, 1f);

        rect.anchorMax =
            new Vector2(0.5f, 1f);

        rect.pivot =
            new Vector2(0.5f, 1f);

        rect.anchoredPosition =
            new Vector2(
                0f,
                -30f
            );

        rect.sizeDelta =
            new Vector2(
                550f,
                70f
            );

        tituloFinal.text =
            "¡FELICIDADES!\n" +
            "¡HAS ROTO LA MALDICIÓN!";

        tituloFinal.fontSize = 26f;

        tituloFinal.fontStyle =
            FontStyles.Bold;

        tituloFinal.alignment =
            TextAlignmentOptions.Center;

        tituloFinal.color =
            new Color(
                0.92f,
                0.94f,
                0.98f
            );

        tituloFinal.raycastTarget = false;
    }


    void CrearTextoFinal()
    {
        GameObject objetoMensaje =
            new GameObject(
                "MensajeFinal",
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

        objetoMensaje.transform.SetParent(
            panelFinal.transform,
            false
        );

        mensajeFinal =
            objetoMensaje.GetComponent<TextMeshProUGUI>();

        RectTransform rect =
            mensajeFinal.rectTransform;

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            new Vector2(
                0f,
                -35f
            );

        rect.sizeDelta =
            new Vector2(
                530f,
                130f
            );

        int puntosFinales = 0;

        if (SistemaPuntos.instancia != null)
        {
            puntosFinales =
                SistemaPuntos.instancia.ObtenerPuntos();
        }

        mensajeFinal.text =
            "Has conseguido la llave y liberado la ciudad.\n" +
            "La noche eterna ha terminado.\n\n" +
            "<b>PUNTOS DE LUZ: " +
            puntosFinales +
            "</b>";

        mensajeFinal.fontSize = 18f;

        mensajeFinal.alignment =
            TextAlignmentOptions.Center;

        mensajeFinal.color =
            new Color(
                0.92f,
                0.92f,
                0.92f
            );

        mensajeFinal.raycastTarget = false;
    }


    // =====================================================
    // SEGURIDAD
    // =====================================================

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}