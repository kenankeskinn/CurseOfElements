using UnityEngine;

namespace PlayerManager
{
    public class AnimationController : MonoBehaviour
    {
        PlayerContext _player;
        Animator animator;

        // Animation Hashes
        private static readonly int WalkHash = Animator.StringToHash("isWalking");
        private static readonly int JumpHash = Animator.StringToHash("isJumping");
        private static readonly int FallHash = Animator.StringToHash("isFalling");
        private static readonly int InteractHash = Animator.StringToHash("isInteracting");
        private static readonly int MeleeHash = Animator.StringToHash("isMeleeAttacking");
        private static readonly int RangedHash = Animator.StringToHash("isRangedAttacking");
        private static readonly int DamageHash = Animator.StringToHash("isTakingDamage");
        private static readonly int DeadHash = Animator.StringToHash("isDead");


        #region Custom Functions
        void ChangeAnimation()
        {
            animator.SetBool(WalkHash, _player.IsWalking);
            animator.SetBool(JumpHash, _player.IsJumping);
            animator.SetBool(FallHash, _player.IsFalling);
            animator.SetBool(InteractHash, _player.IsInteracting);
            animator.SetBool(MeleeHash, _player.IsMeleeAttacking);
            animator.SetBool(RangedHash, _player.IsRangedAttacking);
            animator.SetBool(DamageHash, _player.IsTakingDamage);
            animator.SetBool(DeadHash, _player.IsDead);
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            _player = GetComponent<PlayerContext>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            ChangeAnimation();
        }
        #endregion
    }
}