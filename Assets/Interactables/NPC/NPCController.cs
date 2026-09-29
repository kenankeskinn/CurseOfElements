using System.Collections;
using UnityEngine;

namespace InteractableManager
{
    class NPCController : MonoBehaviour, IInteractable
    {
        public bool canInteract { get; set; }
        [SerializeField][TextArea] string[] dialog;
        [SerializeField] TMPro.TextMeshProUGUI text;
        private SpriteRenderer spriteRenderer;

        public void Interact()
        {
            if (canInteract)
            {
                StartCoroutine(ShowDialog());

                canInteract = false;
            }
        }

        private void Awake()
        {
            canInteract = true;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        IEnumerator ShowDialog()
        {
            text.gameObject.SetActive(true);

            foreach (string item in dialog)
            {
                text.text = item;
                yield return new WaitForSeconds(5);
            }

            text.gameObject.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if ((gameObject.transform.position.x < collision.transform.position.x) && spriteRenderer.flipX)
                {
                    spriteRenderer.flipX = false;
                }
                else if ((gameObject.transform.position.x > collision.transform.position.x) && !spriteRenderer.flipX)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
    }
}