using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    public class MovementController : MonoBehaviour
    {
        LayerMask groundLayer;
        Vector2 rightFootPos, leftFootPos;

        #region Custom Functions
        void Walk()
        {
            if (!(PlayerContext.Instance.CanWalk && PlayerContext.Instance.WalkInput != 0)) { PlayerContext.Instance.IsWalking = false; return; }

            if (PlayerContext.Instance.WalkInput > 0)
                transform.localScale = new Vector2(1, transform.localScale.y);
            else
                transform.localScale = new Vector2(-1, transform.localScale.y);

            PlayerContext.Instance.Rigidbody.linearVelocityX = PlayerContext.Instance.WalkInput * PlayerContext.Instance.WalkSpeed;
            PlayerContext.Instance.IsWalking = true;
        }

        void Jump()
        {
            // Jump State Control
            if (PlayerContext.Instance.IsGrounded) { PlayerContext.Instance.IsJumping = false; PlayerContext.Instance.IsFalling = false;  }
            else if (PlayerContext.Instance.Rigidbody.linearVelocityY > 0.01) { PlayerContext.Instance.IsJumping = true; PlayerContext.Instance.IsFalling = false; }
            else if (PlayerContext.Instance.Rigidbody.linearVelocityY < -0.01) { PlayerContext.Instance.IsJumping = false; PlayerContext.Instance.IsFalling = true; }

            // -------------------------------------------- Function Task --------------------------------------------

            if (!(PlayerContext.Instance.CanJump && PlayerContext.Instance.JumpInput && PlayerContext.Instance.IsGrounded )) { return; }

            PlayerContext.Instance.Rigidbody.linearVelocity = new Vector2(PlayerContext.Instance.Rigidbody.linearVelocity.x, PlayerContext.Instance.JumpForce * 3f);
            PlayerContext.Instance.IsGrounded = false;
        }

        // Support Functions
        void GroundCheck()
        {
            if (transform.localScale.x == 1)
            {
                rightFootPos = new Vector2(transform.position.x - .2f, transform.position.y - .5f);
                leftFootPos = new Vector2(transform.position.x + .4f, transform.position.y - .5f);
            }
            else
            {
                rightFootPos = new Vector2(transform.position.x + .2f, transform.position.y - .5f);
                leftFootPos = new Vector2(transform.position.x - .4f, transform.position.y - .5f);
            }

            RaycastHit2D hitR = Physics2D.Raycast(rightFootPos, Vector2.down, .25f, groundLayer);
            RaycastHit2D hitL = Physics2D.Raycast(leftFootPos, Vector2.down, .25f, groundLayer);
            Debug.DrawRay(rightFootPos, Vector2.down * .25f, Color.whiteSmoke);
            Debug.DrawRay(leftFootPos, Vector2.down * .25f, Color.whiteSmoke);

            PlayerContext.Instance.IsGrounded = hitR.collider != null || hitL.collider != null;
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            groundLayer = LayerMask.GetMask("Ground");
            rightFootPos = transform.GetChild(1).GetChild(0).position; // Player -> Foots -> Foot_R
            leftFootPos = transform.GetChild(1).GetChild(1).position;  // Player -> Foots -> Foot_L
        }

        private void FixedUpdate()
        {
            GroundCheck();

            Walk();
            Jump();
        }
        #endregion
    }
}

