using UnityEngine;
using UnityEngine.UI;

public class IntroSequenceManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    public Button startButton;

    [Header("Animadores")]
    public Animator doorAnimator;
    public Animator guideAnimator;

    [Header("Audio (Opcional)")]
    public AudioSource backgroundMusic;
    public AudioSource welcomeVoice;

    void Start()
    {
        // Configuramos el botón para que ejecute la función cuando se presione
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
        }

        // Si tienes una música de fondo, puedes activarla aquí
        if (backgroundMusic != null) backgroundMusic.Play();
        
        // Si tienes una voz de bienvenida, la activamos
        if (welcomeVoice != null) welcomeVoice.Play();
    }

    void OnStartButtonPressed()
    {
        Debug.Log("¡Secuencia Iniciada!");

        // 1. Abrimos la puerta usando el Trigger 'Open' que creamos
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        // 2. Activamos el movimiento del guía usando el Trigger 'Exit'
        if (guideAnimator != null)
        {
            guideAnimator.SetTrigger("Exit");
        }

        // 3. Desactivamos el botón para que no se pueda pulsar dos veces
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }

        // 4. Detener la voz si seguía hablando
        if (welcomeVoice != null) welcomeVoice.Stop();
    }
}
