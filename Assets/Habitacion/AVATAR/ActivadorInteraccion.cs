using UnityEngine;

public class ActivadorInteraccion : MonoBehaviour
{

    [Header("Configuración de Audio")]
    public AudioSource audioSource; // Referencia al AudioSource que se reproducirá
    public bool reproducirUnaVez = true; // Si el audio se reproduce solo una vez

    [Header("Configuración de Trigger")]
    public string tagJugador = "Player"; // Tag del objeto jugador

    private bool yaActivado = false; // Control para reproducción única
    void Start()
    {
        // Verificar si hay un AudioSource en el mismo objeto
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Si aún no hay AudioSource, buscar en hijos
        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        // Advertencia si no se encuentra AudioSource
        if (audioSource == null)
        {
            Debug.LogWarning("No se encontró un AudioSource en " + gameObject.name);
        }
    }

    // Este método se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra tiene el tag del jugador
        if (other.CompareTag(tagJugador))
        {
            // Si está configurado para reproducir una vez y ya fue activado, salir
            if (reproducirUnaVez && yaActivado)
                return;

            ActivarAudio();
            yaActivado = true;
        }
    }

    // Opcional: Si quieres que el audio se active también al salir del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            // Detener el audio cuando el jugador se aleje
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Audio detenido: " + audioSource.clip.name);
            }
        }
    }
    public void ActivarAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audio activado: " + audioSource.clip.name);
        }
    }

    // Método público para resetear el activador (útil si quieres reutilizarlo)
    public void ResetearActivador()
    {
        yaActivado = false;
    }
}
