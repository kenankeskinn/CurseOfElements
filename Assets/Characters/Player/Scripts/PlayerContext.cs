using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerManager
{
    #region Enums
    public enum Element
    {
        None,
        Fire,
        Water,
        Wind
    }

    enum AttackType
    {
        Melee,
        Ranged
    }

    public enum EffectType
    {
        Burn,
        Slow,
        Push
    }
    #endregion

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerContext : MonoBehaviour
    {
        #region Variables
        // References
        private InputEvents inputs;
        private Rigidbody2D rb;
        public static PlayerContext Instance { get; private set; }

        [Header("-- STATS --")]
        [SerializeField] string characterName = "";
        [SerializeField][Range(0, 1000)] int currentHealth;
        [SerializeField] int maxHealth = 100;

        [Space(20)]

        [Header("-- MOVEMENT --")]
        [Header("Settings")]
        [SerializeField][Range(1, 10)] int walkSpeed = 3;
        [SerializeField][Range(1, 5)] int jumpForce = 4;
        [SerializeField][Range(10, 60)] int slopeLimit = 45;

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
        [SerializeField] Transform bulletStartTransform;
        [SerializeField] GameObject[] elementBullets;

        [Header("Gameplay Info")]
        [SerializeField] bool canAttack = true;
        [SerializeField] bool canTakeDamage = true;

        [Header("Element Info")]
        [EnumButtons][SerializeField] Element[] usableElements = new Element[3];
        [EnumButtons][SerializeField] Element selectedElement = Element.None;

        [Header("Input Info")]
        [SerializeField] bool meleeInput = false;
        [SerializeField] bool rangedInput = false;
        [SerializeField] bool nextElementInput = false;
        [SerializeField] bool previousElementInput = false;

        [Space(20)]

        [Header("-- INTERACTION --")]
        [Header("Gameplay Info")]
        [SerializeField] bool canInteract = true;
        [SerializeField] InteractableManager.KeyType key = InteractableManager.KeyType.None;

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

        [Space(20)]

        [Header("-- UI OPERATIONS --")]
        [Header("GameObject")]
        [SerializeField] GameObject ui;
        [SerializeField] GameObject[] keysUI = new GameObject[3];

        [Header("Image")]
        [SerializeField] Image healthBar;
        [SerializeField] Image fireImage;
        [SerializeField] Image waterImage;
        [SerializeField] Image windImage;

        [Header("Animator")]
        [SerializeField] Animator fireAnimator;
        [SerializeField] Animator waterAnimator;
        [SerializeField] Animator windAnimator;

        [Header("Color")]
        [SerializeField] Color unUsedElement;
        [SerializeField] Color usedElement;

        [Header("Text")]
        [SerializeField] TMPro.TextMeshProUGUI interactionText;
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
        public int SlopeLimit { get { return slopeLimit; } }
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
        public Transform BulletStartTransform { get { return bulletStartTransform; } }
        public GameObject[] ElementBullets { get { return elementBullets; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
        public bool CanTakeDamage { get { return canTakeDamage; } set { canTakeDamage = value; } }
        public Element[] UsableElements { get { return usableElements; } }
        public Element SelectedElement 
        { 
            get { return selectedElement; } 
            set 
            {
                if (selectedElement != Element.None)
                {
                    switch (value)
                    {
                        case Element.Fire:
                            FireImage.color = usedElement;
                            FireAnimator.SetBool("isSelected", true);
                            break;
                        case Element.Water:
                            WaterImage.color = usedElement;
                            WaterAnimator.SetBool("isSelected", true);
                            break;
                        case Element.Wind:
                            WindImage.color = usedElement;
                            WindAnimator.SetBool("isSelected", true);
                            break;
                        default:
                            break;
                    }

                    switch (selectedElement)
                    {
                        case Element.Fire:
                            FireImage.color = unUsedElement;
                            FireAnimator.SetBool("isSelected", false);
                            break;
                        case Element.Water:
                            WaterImage.color = unUsedElement;
                            WaterAnimator.SetBool("isSelected", false);
                            break;
                        case Element.Wind:
                            WindImage.color = unUsedElement;
                            WindAnimator.SetBool("isSelected", false);
                            break;
                        default:
                            break;
                    }
                }           
                                
                selectedElement = value;
            } 
        }
        public bool MeleeInput { get { return meleeInput; } }
        public bool RangedInput { get { return rangedInput; } }
        public bool NextElementInput { get  { return nextElementInput; } }
        public bool PreviousElementInput { get  { return previousElementInput; } }

        // Interaction
        public bool InteractionInput { get { return interactionInput; } }
        public InteractableManager.KeyType Key 
        { 
            get { return key; } 
            set  
            { 
                key = value;

                // keysUI[0] = White Key UI
                // keysUI[1] = Red Key UI
                // keysUI[2] = Black Key UI
                switch (key)
                {
                    case InteractableManager.KeyType.None:
                        keysUI[0].SetActive(false);
                        keysUI[1].SetActive(false);
                        keysUI[2].SetActive(false);
                        break;
                    case InteractableManager.KeyType.White:
                        keysUI[0].SetActive(true);
                        keysUI[1].SetActive(false);
                        keysUI[2].SetActive(false);

                        keysUI[0].GetComponent<Animator>().Play(Animator.StringToHash("EarnKey"));
                        break;
                    case InteractableManager.KeyType.Red:
                        keysUI[0].SetActive(false);
                        keysUI[1].SetActive(true);
                        keysUI[2].SetActive(false);

                        keysUI[1].GetComponent<Animator>().Play(Animator.StringToHash("EarnKey"));
                        break;
                    case InteractableManager.KeyType.Black:
                        keysUI[0].SetActive(false);
                        keysUI[1].SetActive(false);
                        keysUI[2].SetActive(true);

                        keysUI[2].GetComponent<Animator>().Play(Animator.StringToHash("EarnKey"));
                        break;
                }
            } 
        }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        public bool IsInteracting { get { return isInteracting; } set { isInteracting = value; } }
        public bool IsMeleeAttacking { get { return isMeleeAttacking; } set { isMeleeAttacking = value; } }
        public bool IsRangedAttacking { get { return isRangedAttacking; } set { isRangedAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }

        // UI Operations
        public GameObject UI { get { return ui; } }
        public Image HealthBar { get { return healthBar; } }
        public Image FireImage { get { return fireImage; } }
        public Image WaterImage { get { return waterImage; } }
        public Image WindImage { get { return windImage; } }
        public Animator FireAnimator { get { return fireAnimator; } }
        public Animator WaterAnimator { get { return waterAnimator; } }
        public Animator WindAnimator { get { return windAnimator; } }
        public Color UnUsedElement { get { return unUsedElement; } }
        public Color UsedElement { get { return usedElement; } }
        public TMPro.TextMeshProUGUI InteractionText { get { return interactionText; } }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(UI);
            UI.SetActive(true);

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
            Inputs.Gameplay.NextElement.started += ctx => { nextElementInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.NextElement.canceled += ctx => { nextElementInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.PreviousElement.started += ctx => { previousElementInput = ctx.ReadValueAsButton(); };
            Inputs.Gameplay.PreviousElement.canceled += ctx => { previousElementInput = ctx.ReadValueAsButton(); };
        }

        private void OnEnable()
        {
            if (Inputs != null) Inputs.Enable();
        }
        private void OnDisable()
        {
            if (Inputs != null) Inputs.Disable();
        }
        #endregion

        #region Custom Functions
        public void StartPlayerSystem()
        {
            CurrentHealth = MaxHealth;
            HealthBar.fillAmount = 1;

            CanWalk = true;
            CanJump = true;
            CanAttack = true;
            CanTakeDamage = true;

            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            Rigidbody.gravityScale = 2;

            GetComponent<Collider2D>().enabled = true;

            GetComponent<AnimationController>().ResetAnimationStates();
            GetComponent<CombatController>().ResetCombatCoroutines();

            gameObject.SetActive(true);
        }
        #endregion
    }
}
