using System.Collections;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    bool canInteract = true;

    public void Interact()
    {
        if (!canInteract) { return; }

        Debug.Log($"Interaction Completed! Interactable Name: {gameObject.name}");

        /* Not real Interactable objects just for understand the system.
        if (gameObject.CompareTag("Chest")) Chest();
        else if (gameObject.CompareTag("Door")) Door();
        
         .
         .
         .
         */

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        canInteract = false;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void Chest()
    {

    }

    void Door()
    {

    }
}
