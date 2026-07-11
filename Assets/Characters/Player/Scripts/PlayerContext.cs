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
        [SerializeField][Range(0, 100)] int currentHealth = 100;
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

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;
        [EnumButtons] [SerializeField] Element[] usableElements = { };
        [EnumButtons] [SerializeField] Element selectedElement = Element.None;

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
                if (value < 0) value = 0; 
                else if (value > MaxHealth) value = 100;
                else currentHealth = value; 
            } 
        }
        public int MaxHealth { get { return maxHealth; } }
        
        // Movement
        public int WalkSpeed { get { return walkSpeed; } }
        public int JumpForce { get { return jumpForce; } }
        public bool CanWalk { get { return canWalk; } }
        public bool CanJump { get { return canJump; } }
        public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
        public float WalkInput { get { return walkInput; } }
        public bool JumpInput { get { return jumpInput; } }

        // Combat
        public int MeleeDamage { get { return meleeDamage; } }
        public int RangedDamage { get { return rangedDamage; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
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
        #endregion

        #region Unity Functions
        private void Awake()
        {
            inputs = new InputEvents();
            rb = GetComponent<Rigidbody2D>();

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

        private void OnCollisionEnter2D(Collision2D collision) // ??????
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) IsGrounded = true;
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
