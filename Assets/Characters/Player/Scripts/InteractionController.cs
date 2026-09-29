using InteractableManager;
using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    class InteractionController : MonoBehaviour
    {
        #region Custom Functions
        void Interaction()
        {
            float lookDirection;
            if (transform.localScale.x == 1) lookDirection = 1f;    // Right
            else lookDirection = -1f;                               // Left

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, lookDirection, LayerMask.GetMask("Interactable"));
            Debug.DrawRay(transform.position, transform.right * lookDirection, Color.blue); // Just for see on scene

            if (hit.collider != null && hit.collider.GetComponent<IInteractable>().canInteract)
            {
                var interactionText = PlayerContext.Instance.InteractionText;
                interactionText.transform.localScale = new Vector2(.55f * lookDirection, .55f);
                interactionText.canvas.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 1f);
                interactionText.gameObject.SetActive(true);
            }
            else 
            {
                PlayerContext.Instance.InteractionText.gameObject.SetActive(false); 
            }

            if (!PlayerContext.Instance.InteractionInput) { return; }

            if (hit.collider == null) { return; } // If there is no Interactable object, it returns

            hit.collider.GetComponent<IInteractable>().Interact();
        }
        #endregion

        #region Unity Functions
        private void Update()
        {
            Interaction();
        }
        #endregion
    }
}