using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractableManager
{
    class ChestController : MonoBehaviour, IInteractable
    {
        public bool canInteract { get; set; }
        public KeyType keyAward;
        [SerializeField] Animator keyAnimator;

        public void Interact()
        {
            if (canInteract)
            {
                StartCoroutine(OpenChest());
                canInteract = false;
            }
        }

        private void Awake()
        {
            canInteract = true;
        }

        IEnumerator OpenChest()
        {
            Animator chestAnim = GetComponent<Animator>();

            chestAnim.Play(Animator.StringToHash("OpenChest"));
            yield return new WaitForSeconds(2f);
            Destroy(keyAnimator.gameObject);

            PlayerManager.PlayerContext.Instance.Key = keyAward;

            chestAnim.Play(Animator.StringToHash("CloseChest"));
            yield return new WaitForSeconds(.5f);

            GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(.8f);

            Destroy(gameObject);
        }

        public void GiveKey()
        {
            keyAnimator.gameObject.SetActive(true);
            keyAnimator.Play(Animator.StringToHash("ShowKey"));
        }

    }
}