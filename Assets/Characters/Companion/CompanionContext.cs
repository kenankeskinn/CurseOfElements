using UnityEngine;

namespace Companion
{
    public enum CompanionState
    {
        Idle,
        Walk,
        Jump, // Can change
        Attack
    }

    public class CompanionContext : MonoBehaviour
    {
        #region Variables
        // References
        private Rigidbody2D rb;

        [Header("Stats")]
        [SerializeField] string characterName = "";
        [SerializeField][Range(0, 100)] int currentHealth = 100;
        [SerializeField] int maxHealth = 100;
        [SerializeField][EnumButtons] CompanionState currentState;

        [Space(20)]

        [Header("-- MOVEMENT --")]
        [Header("Settings")]
        [SerializeField][Range(1, 5)] float walkSpeed = 1.5f;
        [SerializeField][Range(1, 5)] float minFollowDistance = 2.5f;

        [Header("Gameplay Info")]
        [SerializeField] bool canWalk = true;

        [Space(20)]

        [Header("-- COMBAT --")]
        [Header("Settings")]
        [SerializeField] int meleeDamage = 15;
        [SerializeField] int rangedDamage = 10;

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;

        [Space(20)]

        [Header("-- ANIMATION --")]
        [SerializeField] bool isWalking = false;
        [SerializeField] bool isJumping = false;
        [SerializeField] bool isFalling = false;
        [SerializeField] bool isMeleeAttacking = false;
        [SerializeField] bool isRangedAttacking = false;
        [SerializeField] bool isTakingDamage = false;
        [SerializeField] bool isDead = false;

        #endregion

        #region Properties
        // References
        public Rigidbody2D Rigidbody { get { return rb; } }

        // Stats
        public int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (value < 0) value = 0;
                else if (value > MaxHealth) value = 100;
                else currentHealth = value;
            }
        }
        public int MaxHealth { get { return maxHealth; } }
        public CompanionState CurrentState { get { return currentState; } set { currentState = value; } }

        // Movement
        public float WalkSpeed { get { return walkSpeed; } }
        public float MinFollowDistance { get { return minFollowDistance; } }
        public bool CanWalk { get { return canWalk; } }

        // Combat
        public int MeleeDamage { get { return meleeDamage; } }
        public int RangedDamage { get { return rangedDamage; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        public bool IsMeleeAttacking { get { return isMeleeAttacking; } set { isMeleeAttacking = value; } }
        public bool IsRangedAttacking { get { return isRangedAttacking; } set { isRangedAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion
    }
}
