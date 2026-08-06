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


        #region Custom Functions
        void ChangeAnimation()
        {
            animator.SetBool(WalkHash, _enemy.IsWalking);
            animator.SetBool(AttackHash, _enemy.IsAttacking);
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