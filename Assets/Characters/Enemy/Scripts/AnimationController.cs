using UnityEngine;

namespace EnemyManager
{
    public class AnimationController : MonoBehaviour
    {
        EnemyContext _enemy;
        Animator animator;

        // Animation Hashes
        private static readonly int WalkHash = Animator.StringToHash("isWalking");
        private static readonly int AttackHash = Animator.StringToHash("isAttacking");
        private static readonly int HurtHash = Animator.StringToHash("isTakingDamage");
        private static readonly int DeathHash = Animator.StringToHash("isDead");


        #region Custom Functions
        void ChangeAnimation()
        {
            animator.SetBool(WalkHash, _enemy.IsWalking);
            animator.SetBool(AttackHash, _enemy.IsAttacking);
            animator.SetBool(HurtHash, _enemy.IsTakingDamage);
            animator.SetBool(DeathHash, _enemy.IsDead);
        }

        #endregion

        #region Unity Functions
        private void Awake()
        {
            _enemy = GetComponent<EnemyContext>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }
        private void Update()
        {
            ChangeAnimation();
        }
        #endregion
    }
}