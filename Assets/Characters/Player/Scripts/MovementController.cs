using UnityEngine;

namespace PlayerManager
{
    public class MovementController : MonoBehaviour
    {
        PlayerContext _player;

        #region Custom Functions
        void Walk()
        {
            if (!(_player.CanWalk && _player.WalkInput != 0)) { return; }

            if (_player.WalkInput > 0)
                transform.localScale = new Vector2(1, transform.localScale.y);
            else
                transform.localScale = new Vector2(-1, transform.localScale.y);

            _player.Rigidbody.linearVelocity = new Vector2(_player.WalkInput * _player.WalkSpeed, _player.Rigidbody.linearVelocity.y);
        }

        void Jump()
        {
            if (!(_player.IsGrounded && _player.JumpInput)) { return; }

            _player.Rigidbody.AddForce(Vector2.up * 75 * _player.JumpForce, ForceMode2D.Impulse);
            _player.IsGrounded = false;
        }

        #endregion

        #region Unity Functions
        private void Start()
        {
            _player = GetComponent<PlayerContext>();            
        }

        private void FixedUpdate()
        {
            Walk();
        }

        private void Update()
        {
            Jump();
        }
        #endregion
    }
}

