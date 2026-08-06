using System;
using UnityEngine;

namespace PlayerManager
{
    #region Enums
    public enum Element
    {
        None,
        Wind,
        Water,
        Fire
    }

    enum AttackType
    {
        Melee,
        Ranged
    }
    #endregion

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerContext : MonoBehaviour
    {
        #region Variables
        // References
        private InputEvents inputs;
        private Rigidbody2D rb;

        [Header("-- STATS --")]
        [SerializeField] string characterName = "";
        [SerializeField][Range(0, 1000)] int currentHealth;
        [SerializeField] int maxHealth = 100;

        [Space(20)]

        [Header("-- MOVEMENT --")]
        [Header("Settings")]
        [SerializeField][Range(1, 10)] int walkSpeed = 3;
        [SerializeField][Range(1, 5)] int jumpForce = 4;

        [Header("Gameplay Info")]
        [SerializeField] bool canWalk = true;
        [SerializeField] bool canJump = true;
        [SerializeField] bool isGrounded = false;

        [Header("Input Info")]
        [SerializeField] float walkInput = 0;
        [SerializeField] bool jumpInput = false;

        [Space(20)]

        [Header("-- COMBAT --")]
        [Header("Settings")]
        [SerializeField] int meleeDamage = 15;
        [SerializeField] int rangedDamage = 10;
        [SerializeField] float meleeResetTime = .58f;
        [SerializeField] float rangedResetTime = .58f;
        [SerializeField] float rangeOfAttack = 1f;

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;
        [SerializeField] bool canTakeDamage = true;

        [Header("Element Info")]
        [EnumButtons][SerializeField] Element[] usableElements = { };
        [EnumButtons][SerializeField] Element selectedElement = Element.None;

        [Header("Input Info")]
        [SerializeField] bool meleeInput = false;
        [SerializeField] bool rangedInput = false;

        [Space(20)]

        [Header("-- INTERACTION --")]
        [Header("Gameplay Info")]
        [SerializeField] bool canInteract = true;

        [Header("Input Info")]
        [SerializeField] bool interactionInput = false;

        [Space(20)]

        [Header("-- ANIMATION --")]
        [Header("States")]
        [SerializeField] bool isWalking = false;
        [SerializeField] bool isJumping = false;
        [SerializeField] bool isFalling = false;
        [SerializeField] bool isInteracting = false;
        [SerializeField] bool isMeleeAttacking = false;
        [SerializeField] bool isRangedAttacking = false;
        [SerializeField] bool isTakingDamage = false;
        [SerializeField] bool isDead = false;
        #endregion

        #region Properties
        // References
        public InputEvents Inputs { get { return inputs; } }
        public Rigidbody2D Rigidbody { get { return rb; } }

        // Stats
        public int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (value < 0) currentHealth = 0;
                else if (value > MaxHealth) currentHealth = MaxHealth;
                else currentHealth = value;
            }
        }
        public int MaxHealth { get { return maxHealth; } }

        // Movement
        public int WalkSpeed { get { return walkSpeed; } }
        public int JumpForce { get { return jumpForce; } }
        public bool CanWalk { get { return canWalk; } set { canWalk = value; } }
        public bool CanJump { get { return canJump; } set { canJump = value; } }
        public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
        public float WalkInput { get { return walkInput; } }
        public bool JumpInput { get { return jumpInput; } }

        // Combat
        public int MeleeDamage { get { return meleeDamage; } }
        public int RangedDamage { get { return rangedDamage; } }
        public float MeleeResetTime { get { return meleeResetTime; } }
        public float RangedResetTime { get { return rangedResetTime; } }
        public float RangeOfAttack { get { return rangeOfAttack; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
        public bool CanTakeDamage { get { return canTakeDamage; } set { canTakeDamage = value; } }
        public Element[] UsableElements { get { return usableElements; } }
        public Element SelectedElement { get { return selectedElement; } }
        public bool MeleeInput { get { return meleeInput; } }
        public bool RangedInput { get { return rangedInput; } }

        // Interaction
        public bool InteractionInput { get { return interactionInput; } }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        public bool IsInteracting { get { return isInteracting; } set { isInteracting = value; } }
        public bool IsMeleeAttacking { get { return isMeleeAttacking; } set { isMeleeAttacking = value; } }
        public bool IsRangedAttacking { get { return isRangedAttacking; } set { isRangedAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }

        // 
        #endregion

        #region Unity Functions
        private void Awake()
        {
            inputs = new InputEvents();
            rb = GetComponent<Rigidbody2D>();

            currentHealth = maxHealth;

            Inputs.Gameplay.Walk.started += ctx => { walkInput = ctx.ReadValue<float>(); };
            Inputs.Gameplay.Walk.canceled += ctx => { walkInput = ctx.ReadValue<float>(); };
            Inputs.Gameplay.Jump.started += ctx => { jumpInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.Jump.canceled += ctx => { jumpInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.MeleeAttack.started += ctx => { meleeInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.MeleeAttack.canceled += ctx => { meleeInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.RangedAttack.started += ctx => { rangedInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.RangedAttack.canceled += ctx => { rangedInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.Interaction.started += ctx => { interactionInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.Interaction.canceled += ctx => { interactionInput = ctx.ReadValueAsButton(); };
        }

        private void OnEnable()
        {
            Inputs.Enable();
        }
        private void OnDisable()
        {
            Inputs.Disable();
        }
        #endregion
    }
}
