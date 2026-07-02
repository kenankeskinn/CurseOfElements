using UnityEngine;

namespace PlayerManager
{
    public class MovementController : MonoBehaviour
    {
        PlayerContext _player;
        LayerMask groundLayer;
        Vector2 rightFootPos, leftFootPos;

        #region Custom Functions
        void Walk()
        {
            if (!(_player.CanWalk && _player.WalkInput != 0)) { _player.IsWalking = false; return; }

            if (_player.WalkInput > 0)
                transform.localScale = new Vector2(1, transform.localScale.y);
            else
                transform.localScale = new Vector2(-1, transform.localScale.y);

            _player.Rigidbody.linearVelocity = new Vector2(_player.WalkInput * _player.WalkSpeed, _player.Rigidbody.linearVelocity.y);
            _player.IsWalking = true;
        }

        void Jump()
        {
            if (_player.Rigidbody.linearVelocityY > 0.001) { _player.IsJumping = true; _player.IsFalling = false; }
            else if (_player.Rigidbody.linearVelocityY < -0.001) { _player.IsJumping = false; _player.IsFalling = true; }
            else { _player.IsJumping = false; _player.IsFalling = false; }

            if (!(_player.IsGrounded && _player.JumpInput)) { return; }

            _player.Rigidbody.linearVelocity = new Vector2(_player.Rigidbody.linearVelocity.x, _player.JumpForce * 3f);
            _player.IsGrounded = false;
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
            Debug.DrawRay(rightFootPos, Vector2.down * .25f, Color.red);
            Debug.DrawRay(leftFootPos, Vector2.down * .25f, Color.red);

            _player.IsGrounded = hitR.collider != null || hitL.collider != null;
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            _player = GetComponent<PlayerContext>();
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

