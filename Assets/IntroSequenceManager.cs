using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; // Necesario para el NavMesh

public class IntroSequenceManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    public Button startButton;

    [Header("Animadores")]
    public Animator doorAnimator;
    public Animator guideAnimator;

    [Header("Movimiento (NavMesh)")]
    public NavMeshAgent guideAgent;    // Arrastra aquí al Personaje Guía
    public Transform exitPoint;       // Crea un objeto vacío donde quieres que camine primero
    public Transform player;          // Arrastra aquí a tu XR Origin / Player
    public float stopDistance = 2.0f; // Distancia para dejar de seguir al jugador

    [Header("Audio (Opcional)")]
    public AudioSource backgroundMusic;
    public AudioSource welcomeVoice;

    private bool hasReachedExit = false;
    private bool sequenceStarted = false;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonPressed);

        if (backgroundMusic != null) backgroundMusic.Play();
        if (welcomeVoice != null) welcomeVoice.Play();
        
        // Inicialmente el agente está detenido
        if (guideAgent != null) guideAgent.isStopped = true;
    }

    void Update()
    {
        if (!sequenceStarted || guideAgent == null) return;

        // 1. Control de la animación según la velocidad
        bool isMoving = guideAgent.velocity.magnitude > 0.1f;
        guideAnimator.SetBool("isWalking", isMoving);

        // 2. Lógica de estados
        if (!hasReachedExit)
        {
            // Verificamos si llegó al punto de salida
            if (!guideAgent.pathPending && guideAgent.remainingDistance <= 0.5f)
            {
                hasReachedExit = true;
                Debug.Log("Guía llegó a la salida. Ahora sigue al jugador.");
            }
        }
        else
        {
            // Seguir al jugador constantemente
            guideAgent.SetDestination(player.position);
            guideAgent.stoppingDistance = stopDistance;
        }
    }

    void OnStartButtonPressed()
    {
        Debug.Log("¡Secuencia Iniciada!");
        sequenceStarted = true;

        // Abrir puerta
        if (doorAnimator != null) doorAnimator.SetTrigger("Open");

        // Activar movimiento hacia la salida
        if (guideAgent != null)
        {
            guideAgent.isStopped = false;
            guideAgent.SetDestination(exitPoint.position);
        }

        // Desactivar UI
        if (startButton != null) startButton.gameObject.SetActive(false);
        if (welcomeVoice != null) welcomeVoice.Stop();
    }
}
