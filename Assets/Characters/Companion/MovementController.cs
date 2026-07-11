using UnityEngine;

namespace Companion
{
    [RequireComponent(typeof(CompanionContext))]
    class MovementController : MonoBehaviour
    {
        CompanionContext _companion;
        Transform playerTransform;

        #region Custom Functions
        void FollowCharacter()
        {
            if (!_companion.CanWalk) { return; }
            
            float distance = Mathf.Abs(transform.position.x - playerTransform.position.x);
            if (distance <= _companion.MinFollowDistance) { return; }
            
            if (transform.position.x < playerTransform.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = 1f * _companion.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = -1f * _companion.WalkSpeed;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            _companion = GetComponent<CompanionContext>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            FollowCharacter();
        }
        #endregion
    }
}
