using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    public class AnimationController : MonoBehaviour
    {
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
            animator.SetBool(WalkHash, PlayerContext.Instance.IsWalking);
            animator.SetBool(JumpHash, PlayerContext.Instance.IsJumping);
            animator.SetBool(FallHash, PlayerContext.Instance.IsFalling);
            animator.SetBool(InteractHash, PlayerContext.Instance.IsInteracting);
            animator.SetBool(MeleeHash, PlayerContext.Instance.IsMeleeAttacking);
            animator.SetBool(RangedHash, PlayerContext.Instance.IsRangedAttacking);
            animator.SetBool(DamageHash, PlayerContext.Instance.IsTakingDamage);
            animator.SetBool(DeadHash, PlayerContext.Instance.IsDead);
        }

        public void ResetAnimationStates()
        {
            PlayerContext.Instance.IsWalking = false;
            PlayerContext.Instance.IsJumping = false;
            PlayerContext.Instance.IsFalling = false;
            PlayerContext.Instance.IsInteracting = false;
            PlayerContext.Instance.IsMeleeAttacking = false;
            PlayerContext.Instance.IsRangedAttacking = false;
            PlayerContext.Instance.IsTakingDamage = false;
            PlayerContext.Instance.IsDead = false;
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            ChangeAnimation();
        }
        #endregion
    }
}