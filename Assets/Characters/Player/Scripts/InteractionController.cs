using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    class InteractionController : MonoBehaviour
    {
        PlayerContext _player;

        #region Custom Functions
        void Interaction()
        {
            if (!_player.InteractionInput) { return; }

            float lookDirection;
            if (transform.localScale.x == 1) lookDirection = 1f;    // Right
            else lookDirection = -1f;                               // Left

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, lookDirection, LayerMask.GetMask("Interactable"));
            Debug.DrawRay(transform.position, transform.right * lookDirection, Color.blue); // Just for see on scene

            if (hit.collider == null) { return; } // If there is no Interactable object, it returns

            hit.collider.GetComponent<InteractableController>().Interact();
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
           _player = GetComponent<PlayerContext>(); 
        }

        private void Update()
        {
            Interaction();
        }
        #endregion
    }
}