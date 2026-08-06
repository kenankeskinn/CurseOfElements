using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class MovementController : MonoBehaviour
    {
        CompanionContext _companion;
        Vector2 jumpOnCheckerPosition, jumpOnCheckerSize;

        #region Custom Functions
        void FollowCharacter()
        {
            if (!_companion.CanWalk && !_companion.CanFallowCharacter) { return; }
            
            float distance = Mathf.Abs(transform.position.x - _companion.PlayerTransform.position.x);
            if (distance <= _companion.MinFollowDistance) { return; }
            if (distance >= 10f) { TeleportToPlayer(); return; }
            
            if (transform.position.x < _companion.PlayerTransform.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = 1f * _companion.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = -1f * _companion.WalkSpeed;
            }

            if (!JumpControl()) { return; } // If character don't need to jump
            Jump();
        }

        public void TeleportToPlayer()
        {
            if (transform.position.x < _companion.PlayerTransform.position.x) 
                _companion.Rigidbody.MovePosition(new Vector2(_companion.PlayerTransform.position.x - 4f, 2));
            else 
                _companion.Rigidbody.MovePosition(new Vector2(_companion.PlayerTransform.position.x + 4f, 2));

            _companion.Target = null;
            _companion.CanFallowCharacter = true;
        }
        
        void Jump()
        {
            if (!_companion.CanJump) { return; }

            _companion.Rigidbody.linearVelocityY = _companion.JumpForce * 3 ;
        }

        bool JumpControl()
        {
            if (transform.localScale.x == 1) 
                jumpOnCheckerPosition = new Vector2(transform.position.x + .7f, transform.position.y);
            else 
                jumpOnCheckerPosition = new Vector2(transform.position.x - .7f, transform.position.y);

            if (Physics2D.OverlapBox(jumpOnCheckerPosition, jumpOnCheckerSize, 0, _companion.JumpOnObjectsLayer) != null) return true;
            else return false;
        }

        void StopTheSystem()
        {
            _companion.CanWalk = false;
            _companion.CanJump = false;
            _companion.CanAttack = false;
            _companion.CanFallowCharacter = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(jumpOnCheckerPosition, jumpOnCheckerSize);
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            _companion = GetComponent<CompanionContext>();
            jumpOnCheckerSize = new Vector2(.5f, transform.localScale.y);
        }

        private void Update()
        {
            if (_companion.PlayerTransform == null) { StopTheSystem(); return; }

            FollowCharacter();
        }
        #endregion
    }
}
