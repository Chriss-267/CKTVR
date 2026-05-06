// ExplosionTrigger.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExplosionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    
    
    private const float EXPLOSION_LIFETIME = 5f; 

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnObjectGrabbed);
            Debug.Log("ExplosionTrigger: Listener agregado y listo.");
        }
        else
        {
            Debug.LogError("ExplosionTrigger: ¡Error! No se encontró el componente XRGrabInteractable.");
        }
    }

    private void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        if (explosionPrefab != null)
        {
            // 1. Instancia el objeto de la explosión
            // Lo instanciamos en la posición del objeto agarrable
            GameObject newExplosion = Instantiate(
                explosionPrefab, 
                transform.position, 
                Quaternion.identity
            );
            
            // 2. Busca el sistema de partículas y lo reproduce explícitamente (capa de seguridad)
            ParticleSystem ps = newExplosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            // 3. ¡Destruye la instancia después de un tiempo!
            // Esto es crucial para limpiar los restos de la escena
            Destroy(newExplosion, EXPLOSION_LIFETIME); 

            Debug.Log("¡EXPLOSIÓN ACTIVADA!");
            
            // Opcional: Desactivar el script después del primer uso (si quieres que explote solo una vez)
            // this.enabled = false; 
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnObjectGrabbed);
        }
    }
}