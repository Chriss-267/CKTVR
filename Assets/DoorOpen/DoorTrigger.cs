using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            animator.SetBool("IsOpen", true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            animator.SetBool("IsOpen", false);
    }
}