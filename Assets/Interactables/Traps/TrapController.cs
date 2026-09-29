using UnityEngine;

namespace InteractableManager
{
    class TrapController : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) // Kill player
            {
                collision.gameObject.GetComponent<PlayerManager.CombatController>().TakeDamage(PlayerManager.PlayerContext.Instance.CurrentHealth);
            }
            else if (collision.gameObject.CompareTag("Companion")) // Kill companion
            {
                collision.gameObject.GetComponent<CompanionManager.CombatController>().TakeDamage(CompanionManager.CompanionContext.Instance.CurrentHealth);
            }
        }
    }
}
