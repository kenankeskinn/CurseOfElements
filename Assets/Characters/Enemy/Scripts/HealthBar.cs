using UnityEngine;

class HealthBar : MonoBehaviour
{
    [SerializeField] Transform characterTransform;

    private void LateUpdate()
    {
        if (characterTransform.localScale.x == -1) transform.localScale = new Vector2(-1, 1);
        else if (characterTransform.localScale.x == 1) transform.localScale = new Vector2(1, 1);
    }
}