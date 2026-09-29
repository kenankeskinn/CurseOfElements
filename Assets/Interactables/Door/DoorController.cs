using PlayerManager;
using System.Collections;
using UnityEngine;

namespace InteractableManager
{
    class DoorController : MonoBehaviour, IInteractable
    {
        public bool canInteract { get; set; }
        public KeyType keyType;
        [SerializeField] GameObject text;

        public void Interact()
        {
            if (canInteract && PlayerContext.Instance.Key == keyType)
            {
                PlayerContext.Instance.Key = KeyType.None;
                Debug.Log("Door Opening!");
                Destroy(gameObject);
                canInteract = false;
            }
            else
            {
                if(!text.activeSelf)
                {
                    StartCoroutine(ShowText());
                }
            }
        }

        private void Awake()
        {
            canInteract = true;
        }

        IEnumerator ShowText()
        {
            text.SetActive(true);
            yield return new WaitForSeconds(3);
            text.SetActive(false);
        }
    }
}