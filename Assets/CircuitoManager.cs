using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class CircuitoManager : MonoBehaviour
{
    // --- Referencias Asignables en el Inspector ---
    // (Asegúrate de arrastrar los objetos aquí)
    [Header("Sockets del Circuito")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor ledSocket;         // Socket donde DEBE ir el LE
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor resistenciaSocket; // Socket donde DEBE ir la Resistencia
    
    [Header("Activador")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable botonActivador; // Componente de tu botón físico

    [Header("Efectos")]
    public GameObject led; 
    public Material ledEncendidoMaterial; 
    public GameObject explosionPrefab; 
    
    // --- Variables Internas ---
    private Renderer ledRenderer;
    private Material ledApagadoMaterial;

    void Start()
    {
        // ... (Código de inicialización del LED y la explosión)
        if (led != null)
        {
            ledRenderer = led.GetComponent<Renderer>();
            ledApagadoMaterial = ledRenderer.material;
            if (explosionPrefab != null) explosionPrefab.SetActive(false);
        }
        
        // **Clave:** Suscribir el evento de presión del botón
        if (botonActivador != null)
        {
            botonActivador.selectEntered.AddListener(OnBotonPressed);
        }
    }
    
    // Método llamado cuando el usuario presiona el botón
    private void OnBotonPressed(SelectEnterEventArgs args)
    {
        CheckFinalConnection();
    }

  private void CheckFinalConnection()
    {
        // 1. Verificar si ambos sockets están ocupados (usando hasSelection)
        if (!ledSocket.hasSelection || !resistenciaSocket.hasSelection)
        {
            Debug.Log("Faltan componentes en el circuito para probar la conexión.");
            return;
        }

        // 2. Determinar QUÉ objeto está en CADA socket.
        // Usamos la lista 'interactablesSelected' y tomamos el primer elemento [0].
        string objectInLedSocketName = ledSocket.interactablesSelected[0].transform.gameObject.name;
        string objectInResistenciaSocketName = resistenciaSocket.interactablesSelected[0].transform.gameObject.name;

        Debug.Log("Objeto detectado en Socket LED: " + objectInLedSocketName);
        Debug.Log("Objeto detectado en Socket Resistencia: " + objectInResistenciaSocketName);

        // 3. Lógica de VALIDACIÓN: 
        bool isCorrectlyWired = 
            objectInLedSocketName.Contains("LED") && objectInResistenciaSocketName.Contains("Resistencia");

        Debug.Log("Resultado de la validación isCorrectlyWired: " + isCorrectlyWired);
        // --- Lógica de Resultado ---
        if (isCorrectlyWired)
        {
            // ... (Lógica de Encendido)
            Debug.Log("¡Conexión correcta! LED encendido.");
            if (ledRenderer != null && ledEncendidoMaterial != null)
            {
                ledRenderer.material = ledEncendidoMaterial;
            }
        }
        else 
        {
            // ... (Lógica de Explosión)
            Debug.LogError("¡Conexión incorrecta! El circuito explotó.");
            if (explosionPrefab != null)
            {
                // explosionPrefab.transform.position = transform.position; 
                // explosionPrefab.SetActive(true);
                explosionPrefab.transform.position = transform.position; 
                explosionPrefab.SetActive(true);
                
             
                ParticleSystem ps = explosionPrefab.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
            }
        }
    }
}