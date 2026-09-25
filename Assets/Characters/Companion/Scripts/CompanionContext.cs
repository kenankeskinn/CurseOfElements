using UnityEngine;
using UnityEngine.UI;

namespace CompanionManager
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CompanionContext : MonoBehaviour
    {
        #region Variables
        // References
        public static CompanionContext Instance { get; private set; }

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
        [SerializeField][Range(10, 60)] int slopeLimit = 46;
        [SerializeField][Range(1, 5)] float minFollowDistance = 2.5f;
        [SerializeField] LayerMask jumpOnObjectsLayer;

        [Header("Gameplay Info")]
        [SerializeField] bool canWalk = true;
        [SerializeField] bool canJump = true;
        [SerializeField] bool canFollowPlayer = true;
        [SerializeField] bool isGrounded = true;

        [Space(20)]

        [Header("-- COMBAT --")]
        [Header("Settings")]
        [SerializeField] int attackDamage = 15;
        [SerializeField] float attackResetTime = .4f;
        [SerializeField] float rangeOfAttack = 1.25f;
        [SerializeField] LayerMask targetLayer;
        [SerializeField] Image healthBar1;
        [SerializeField] Image healthBar2;

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;
        [SerializeField] bool canTakeDamage = true;
        [SerializeField] GameObject target;

        [Space(20)]

        [Header("-- ANIMATION --")]
        [SerializeField] bool isWalking = false;
        [SerializeField] bool isJumping = false;
        [SerializeField] bool isFalling = false;
        [SerializeField] bool isAttacking = false;
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
                else if (value > MaxHealth) value = MaxHealth;

                currentHealth = value;

                if (currentHealth == 0)
                {
                    CanWalk = false;
                    CanJump = false;
                    CanAttack = false;
                    CanTakeDamage = false;
                    Rigidbody.bodyType = RigidbodyType2D.Static;
                    GetComponent<Collider2D>().enabled = false;

                    Debug.Log($"{name} Died!");
                    Destroy(gameObject);
                }
            }
        }
        public int MaxHealth { get { return maxHealth; } }
        public Transform PlayerTransform 
        { 
            get 
            { 
                if (playerTransform == null)
                {
                    CanWalk = false;
                    CanJump = false;
                    CanAttack = false;
                }

                return playerTransform;
            } 
        }

        // Movement
        public float WalkSpeed { get { return walkSpeed; } }
        public float JumpForce { get { return jumpForce; } }
        public int SlopeLimit { get { return slopeLimit; } }
        public float MinFollowDistance { get { return minFollowDistance; } }
        public LayerMask JumpOnObjectsLayer { get { return jumpOnObjectsLayer; } }
        public bool CanWalk 
        { 
            get { return canWalk; }
            private set
            {
                if (value == false) { IsWalking = false; }

                canWalk = value; 
            } 
        }
        public bool CanJump { get { return canJump; } private set { canJump = value; } }
        public bool CanFollowPlayer { get { return canFollowPlayer; } }
        public bool IsGrounded 
        { 
            get { return isGrounded; } 
            set 
            {
                if (value == true)  { IsJumping = false; }
                else                { IsJumping = true; }

                isGrounded = value; 
            } 
        }

        // Combat
        public int AttackDamage { get { return attackDamage; } }
        public float AttackResetTime { get { return attackResetTime; } }
        public float RangeOfAttack { get { return rangeOfAttack; } set { rangeOfAttack = value; } }
        public LayerMask TargetLayer { get { return targetLayer; } }
        public Image HealthBar1 { get { return healthBar1; } }
        public Image HealthBar2 { get { return healthBar2; } }
        public bool CanAttack 
        { 
            get { return canAttack; } 
            private set 
            { 
                if (value == false) { IsAttacking = false; }

                canAttack = value; 
            }
        }

        public bool CanTakeDamage
        {
            get { return canTakeDamage; }
            set { canTakeDamage = value; }
        }
        public GameObject Target 
        { 
            get { return target; } 
            set 
            { 
                if (value == null)  { canFollowPlayer = true; }
                else                { canFollowPlayer = false; }

                target = value;
            } 
        }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;
            DontDestroyOnLoad(this);

            rb = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
            jumpOnObjectsLayer = ~LayerMask.GetMask("Companion", "Player", "Ground", "Enemy"); // Interactable / None
            targetLayer = LayerMask.GetMask("Enemy");
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        #endregion
    }
}
