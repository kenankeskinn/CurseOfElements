using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class MovementController : MonoBehaviour
    {
        Vector2 jumpOnCheckerPosition, jumpOnCheckerSize;

        #region Custom Functions
        void FollowPlayer()
        {
            if (!CompanionContext.Instance.CanWalk || !CompanionContext.Instance.CanFollowPlayer) 
            {
                CompanionContext.Instance.IsWalking = false;
                CompanionContext.Instance.IsJumping = false;
                return;
            }
            
            float distance = Mathf.Abs(transform.position.x - CompanionContext.Instance.PlayerTransform.position.x);
            if (distance <= CompanionContext.Instance.MinFollowDistance) 
            {
                CompanionContext.Instance.IsWalking = false;
                return; 
            }
            if (distance >= 10f) { TeleportToPlayer(); return; }

            if (transform.position.x < CompanionContext.Instance.PlayerTransform.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = 1f * CompanionContext.Instance.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = -1f * CompanionContext.Instance.WalkSpeed;
            }

            CompanionContext.Instance.IsWalking = true;

            if (!JumpControl()) { return; } // If character don't need to jump
            Jump();
        }

        public void TeleportToPlayer()
        {
            if (transform.position.x < CompanionContext.Instance.PlayerTransform.position.x) 
                CompanionContext.Instance.Rigidbody.MovePosition(new Vector2(CompanionContext.Instance.PlayerTransform.position.x - 4f, 2));
            else 
                CompanionContext.Instance.Rigidbody.MovePosition(new Vector2(CompanionContext.Instance.PlayerTransform.position.x + 4f, 2));

            CompanionContext.Instance.Target = null;
        }
        
        void Jump()
        {
            if (!CompanionContext.Instance.CanJump) { return; }

            CompanionContext.Instance.Rigidbody.linearVelocityY = CompanionContext.Instance.JumpForce * 3 ;
        }

        bool JumpControl()
        {
            if (transform.localScale.x == 1) 
                jumpOnCheckerPosition = new Vector2(transform.position.x + .7f, transform.position.y);
            else 
                jumpOnCheckerPosition = new Vector2(transform.position.x - .7f, transform.position.y);

            if (Physics2D.OverlapBox(jumpOnCheckerPosition, jumpOnCheckerSize, 0, CompanionContext.Instance.JumpOnObjectsLayer) != null) return true;
            else return false;
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
            jumpOnCheckerSize = new Vector2(.5f, transform.localScale.y);
        }

        private void Update()
        {
            FollowPlayer();
        }

        // Just for state check
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                CompanionContext.Instance.IsGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                CompanionContext.Instance.IsGrounded = false;
            }
        }
        #endregion
    }
}
