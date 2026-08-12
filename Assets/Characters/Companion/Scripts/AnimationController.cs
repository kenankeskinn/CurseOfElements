using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class AnimationController : MonoBehaviour
    {
        Animator animator;

        // Animation Hashes
        private static readonly int WalkHash = Animator.StringToHash("isWalking");
        private static readonly int JumpHash = Animator.StringToHash("isJumping");
        private static readonly int AttackHash = Animator.StringToHash("isAttacking");
        private static readonly int HurtHash = Animator.StringToHash("isTakingDamage");
        private static readonly int DeathHash = Animator.StringToHash("isDead");

        #region Custom Functions
        void ChangeAnimation()
        {
            animator.SetBool(WalkHash,      CompanionContext.Instance.IsWalking);
            animator.SetBool(JumpHash,      CompanionContext.Instance.IsJumping);
            animator.SetBool(AttackHash,    CompanionContext.Instance.IsAttacking);
            animator.SetBool(HurtHash,      CompanionContext.Instance.IsTakingDamage);
            animator.SetBool(DeathHash,     CompanionContext.Instance.IsDead);
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
