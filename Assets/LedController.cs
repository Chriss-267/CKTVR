using UnityEngine;

public class LedController : MonoBehaviour
{
    
    public Material materialEncendido;
    public Material materialApagado;

    private MeshRenderer meshRenderer;

    void Awake() 
    {
        // Obtener el MeshRenderer del objeto Object_8
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Este método público lo conectaremos al botón
    public void SetLedState(bool isOn)
    {
        if (meshRenderer == null) return;

        if (isOn)
        {
            meshRenderer.material = materialEncendido;
        }
        else
        {
            meshRenderer.material = materialApagado;
        }
    }
}