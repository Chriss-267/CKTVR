using UnityEngine;

public class SlotTrigger : MonoBehaviour
{
    // Asigna el validador y el objeto requerido (como antes)
    public ProtoboardValidator validator;
    public GameObject requiredObject;

    // Asigna el MeshRenderer del objeto visual del slot (el Quad/Cube)
    public MeshRenderer slotRenderer;

    // Colores para el feedback visual (Asignar en el Inspector)
    public Color neutralColor = new Color(0.5f, 0.5f, 0.5f, 0.2f); // Gris semitransparente
    public Color hintColor = new Color(0f, 1f, 0f, 0.6f);      // Verde brillante

    private bool componentIsNear = false;
    
    // Bandera para saber si el objeto está en la posición exacta de validación
    private bool componentIsValidated = false; 

    void Start()
    {
        // Asegúrate de que el slotRenderer tenga el color neutral al inicio.
        if (slotRenderer != null)
        {
            // Es crucial usar '.material' aquí para manipular una instancia única del material.
            slotRenderer.material.color = neutralColor;
        }
    }

    // --- Lógica de Proximidad y Validación (Usando un ÚNICO Collider Trigger) ---

    private void OnTriggerEnter(Collider other)
    {
        // 1. Detección de validación (el objeto entra en la zona pequeña y exacta)
        // Usamos la misma función para la validación exacta, asumiendo que el collider es el área de validación.
        if (other.gameObject == requiredObject)
        {
            // Notifica al validador que el componente está en posición (true)
            validator.SetComponentStatus(requiredObject, true);
            componentIsValidated = true;
            
            // También activa el visual de pista si no estaba activo
            if (!componentIsNear)
            {
                SetHintVisual(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. Detección de validación (el objeto sale de la zona pequeña y exacta)
        if (other.gameObject == requiredObject)
        {
            // Notifica al validador que el componente ya no está en posición (false)
            validator.SetComponentStatus(requiredObject, false);
            componentIsValidated = false;

            // También desactiva el visual de pista
            SetHintVisual(false);
        }
    }
    
    // NOTA: Si usas dos Colliders (uno grande para la pista, uno pequeño para la validación), 
    // tendrías que mover el código de SetHintVisual a un OnTriggerStay/OnTriggerExit 
    // en el Collider GRANDE.

    // --- Función de Control Visual ---

    private void SetHintVisual(bool isNear)
    {
        componentIsNear = isNear;
        if (slotRenderer != null)
        {
            if (isNear)
            {
                // Cambia al color de pista (Verde)
                slotRenderer.material.color = hintColor;
            }
            else
            {
                // Vuelve al color neutral (Gris)
                slotRenderer.material.color = neutralColor;
            }
        }
    }
    
    // --- Lógica para la Vista de Escena (Gizmos) ---

    private void OnDrawGizmos()
    {
        // Define el color para el Gizmo (visible solo en la escena de Unity)
        Gizmos.color = Color.yellow; 
        
        // Intenta obtener el Box Collider
        Collider col = GetComponent<Collider>();
        if (col is BoxCollider box)
        {
            // Dibuja el contorno del Box Collider (área de detección)
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(box.center, box.size);
            
            // Dibuja una caja semitransparente para mejor visualización
            Gizmos.color = new Color(1f, 1f, 0f, 0.1f); // Amarillo semitransparente
            Gizmos.DrawCube(box.center, box.size);
        }
        else
        {
            // Si no hay un Collider, solo dibuja una pequeña esfera en la posición
            Gizmos.DrawWireSphere(transform.position, 0.05f);
        }
    }
}