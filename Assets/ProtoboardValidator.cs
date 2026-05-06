using UnityEngine;
using UnityEngine.Events; // Necesario para UnityEvent
using UnityEngine.XR.Interaction.Toolkit;

public class ProtoboardValidator : MonoBehaviour
{
    // Asigna estos GameObjects en el Inspector:
    public GameObject botonObjeto;
    public GameObject ledObjeto;
    
    // Asigna el sistema de partículas de explosión en el Inspector:
    public GameObject explosionPrefab;
    
    // Variables de estado para la validación:
    private bool botonEnSlot = false;
    private bool ledEnSlot = false;
    
    // El objeto que usaremos para el clic final (puede ser el mismo botón físico o uno nuevo)
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable clickTrigger;

    void Start()
    {
        // Suscribe la función de validación al evento de clic/selección del botón
        if (clickTrigger != null)
        {
            // Usaremos el evento OnSelectEntered para detectar el "clic"
            clickTrigger.selectEntered.AddListener(CheckAssembly);
        }
    }

    // --- Funciones Llamadas por los Slots (Triggers) ---

    // Llamado cuando un componente entra al Slot Trigger
    public void SetComponentStatus(GameObject component, bool status)
    {
        if (component == botonObjeto)
        {
            botonEnSlot = status;
            Debug.Log("Botón en posición: " + status);
        }
        else if (component == ledObjeto)
        {
            ledEnSlot = status;
            Debug.Log("LED en posición: " + status);
        }
    }

    // --- Función de Validación Principal ---

    private void CheckAssembly(SelectEnterEventArgs args)
    {
        if (botonEnSlot && ledEnSlot)
        {
            // 1. ENSAMBLAJE CORRECTO: Encender LED con Textura
            
            // Llama a una función para cambiar la textura del LED
            EncenderLED();
            
            Debug.Log("¡Correcto! Circuito ensamblado.");
        }
        else
        {
            // 2. ENSAMBLAJE INCORRECTO: Animación de Explosión
            
            // Instancia la explosión en la posición del protoboard
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            
            Debug.Log("¡Incorrecto! Hay un error en el ensamblaje.");
        }
    }
    
    // Función para manejar el cambio de textura/material del LED
    private void EncenderLED()
    {
        // Supongamos que tienes un material "LED_Encendido"
        Renderer ledRenderer = ledObjeto.GetComponent<Renderer>();
        if (ledRenderer != null)
        {
            // Busca o asigna un material de encendido (Luz/Emisión)
            // Debes asignar este material en el Inspector o cargarlo
            Material encendidoMaterial = Resources.Load<Material>("LED_Encendido");
            if (encendidoMaterial != null)
            {
                ledRenderer.material = encendidoMaterial;
            }
            else
            {
                // Alternativa simple: cambia el color a uno brillante
                ledRenderer.material.color = Color.green;
            }
        }
    }
}