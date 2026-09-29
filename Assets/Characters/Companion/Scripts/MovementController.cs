using PlayerManager;
using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class MovementController : MonoBehaviour
    {
        LayerMask groundLayer;
        Vector2 jumpOnCheckerPosition, jumpOnCheckerSize;
        Vector2 slopeCastPos;

        #region Custom Functions
        void FollowPlayer()
        {
            if (!CompanionContext.Instance.IsGrounded && JumpControl()) // If character on air and don't need follow to player and should continue jump
            { 
                Jump();
                float force = transform.localScale.x == 1 ? CompanionContext.Instance.WalkSpeed : -CompanionContext.Instance.WalkSpeed;
                CompanionContext.Instance.Rigidbody.linearVelocityX = force;
            }

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
            if (distance >= 13f) { TeleportToPlayer(); return; }

            if (transform.position.x < CompanionContext.Instance.PlayerTransform.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = CompanionContext.Instance.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = -CompanionContext.Instance.WalkSpeed;
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

        void SlopeCheck()
        {
            if (transform.localScale.x == 1)
                slopeCastPos = new Vector2(transform.position.x + .5f, transform.position.y - .35f);
            else
                slopeCastPos = new Vector2(transform.position.x - .5f, transform.position.y - .35f);

            RaycastHit2D slopeHit = Physics2D.Raycast(slopeCastPos, Vector2.down, .6f, groundLayer);
            Debug.DrawRay(slopeCastPos, Vector2.down * .6f, Color.gold);

            if (slopeHit.collider == null) return;

            float currentSlopeAngle = Vector2.Angle(Vector2.up, slopeHit.normal);

            if (currentSlopeAngle <= 0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
                return;
            }

            if (currentSlopeAngle <= CompanionContext.Instance.SlopeLimit)
            {
                float targetAngel = transform.localScale.x == 1 ? currentSlopeAngle : -currentSlopeAngle;
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetAngel));
            }
        }

        bool JumpControl()
        {
            float direction = transform.localScale.x == 1 ? 0.7f : -0.7f;

            jumpOnCheckerPosition = new Vector2(transform.position.x + direction, transform.position.y);

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
            groundLayer = LayerMask.GetMask("Ground");
            jumpOnCheckerSize = new Vector2(.5f, transform.localScale.y);
        }

        private void Update()
        {
            if (CompanionContext.Instance.IsDead) { return; }

            FollowPlayer();
            SlopeCheck();
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
