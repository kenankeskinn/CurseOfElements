// Layer = Interactable
namespace InteractableManager
{
    interface IInteractable
    {
        bool canInteract { get; set; }

        void Interact();
    }

    public enum KeyType
    {
        None,
        White,
        Red,
        Black
    }
}