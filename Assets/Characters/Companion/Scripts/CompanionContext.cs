using EnemyManager;
using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CompanionContext : MonoBehaviour
    {
        #region Variables
        // References
        private Rigidbody2D rb;

        [Header("-- General --")]
        [SerializeField] string characterName = "";
        [SerializeField][Range(0, 100)] int currentHealth;
        [SerializeField] int maxHealth = 100;
        [SerializeField] Transform playerTransform;

        [Space(20)]

        [Header("-- MOVEMENT --")]
        [Header("Settings")]
        [SerializeField][Range(1, 5)] float walkSpeed = 1.5f;
        [SerializeField][Range(1, 5)] float jumpForce = 4;
        [SerializeField][Range(1, 5)] float minFollowDistance = 2.5f;
        [SerializeField] LayerMask jumpOnObjectsLayer;

        [Header("Gameplay Info")]
        [SerializeField] bool canWalk = true;
        [SerializeField] bool canJump = true;
        [SerializeField] bool canFallowCharacter = true;

        [Space(20)]

        [Header("-- COMBAT --")]
        [Header("Settings")]
        [SerializeField] int meleeDamage = 15;
        [SerializeField] int rangedDamage = 10;
        [SerializeField] float rangeOfAttack = 1.25f;
        [SerializeField] LayerMask targetLayer;

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;
        [SerializeField] GameObject target;

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

        // General
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
        public Transform PlayerTransform { get { return playerTransform; } }

        // Movement
        public float WalkSpeed { get { return walkSpeed; } }
        public float JumpForce { get { return jumpForce; } }
        public float MinFollowDistance { get { return minFollowDistance; } }
        public LayerMask JumpOnObjectsLayer { get { return jumpOnObjectsLayer; } }
        public bool CanWalk { get { return canWalk; } set { canWalk = value; } }
        public bool CanJump { get { return canJump; } set { canJump = value; } }
        public bool CanFallowCharacter { get { return canFallowCharacter; } set { canFallowCharacter = value; }  }

        // Combat
        public int MeleeDamage { get { return meleeDamage; } }
        public int RangedDamage { get { return rangedDamage; } }
        public float RangeOfAttack { get { return rangeOfAttack; } set { rangeOfAttack = value; } }
        public LayerMask TargetLayer { get { return targetLayer; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
        public GameObject Target { get { return target; } set { target = value; } }

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
            currentHealth = maxHealth;
            jumpOnObjectsLayer = ~LayerMask.GetMask("Companion", "Player", "Ground", "Enemy"); // Interactable
            targetLayer = LayerMask.GetMask("Enemy");
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        #endregion
    }
}
