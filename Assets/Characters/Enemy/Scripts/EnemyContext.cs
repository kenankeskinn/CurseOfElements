using System;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyManager
{
    #region Enums
    public enum EnemyType
    {
        Ghoul,
        Skeleton,
        Golem
    }
    #endregion

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyContext : MonoBehaviour
    {
        #region Variables
        // References
        private Rigidbody2D rb;
        private GameObject playerGameObject;

        [Header("-- General --")]
        [SerializeField] EnemyType enemyType;
        [SerializeField] float rangeOfView = 8;
        [SerializeField] float rangeOfAttack = .4f;
        [SerializeField] float attackResetTime = 1f;

        [Header("-- Stats --")]
        [SerializeField] int maxHealth = 100;
        [SerializeField][Range(0, 100)] int currentHealth;
        [SerializeField] int attackDamage = 20;
        [SerializeField][Range(1, 10)] int walkSpeed = 2;

        [Header("-- Animation --")]
        [SerializeField] bool isWalking;
        [SerializeField] bool isAttacking;
        [SerializeField] bool isTakingDamage;
        [SerializeField] bool isDead;

        [Header("-- Info --")]
        [SerializeField] LayerMask playerLayer;
        [SerializeField] RaycastHit2D detectorAttackHit;
        [SerializeField] Vector2 detectorDirection;
        [SerializeField] bool playerDetected;

        [Header("-- Gameplay Info --")]
        [SerializeField] bool canWalk = true;
        [SerializeField] bool canAttack = true;
        [SerializeField] bool canTakeDamage = true;
        #endregion

        #region Properties
        // References
        public Rigidbody2D Rigidbody { get { return rb; } }
        public GameObject PlayerGameObject { get { return playerGameObject; } }

        // General
        public EnemyType EnemyType { get { return enemyType; } }
        public float RangeOfView { get { return rangeOfView; } }
        public float RangeOfAttack { get { return rangeOfAttack; } }
        public float AttackResetTime { get { return attackResetTime; } }

        // Stats
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (value < 0) value = 0;
                else if (value > MaxHealth) value = MaxHealth;
                else currentHealth = value;
            }
        }
        public int AttackDamage { get { return attackDamage; } }
        public int WalkSpeed { get { return walkSpeed; } }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsAttacking { get { return isAttacking; }  set { isAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }

        // Info
        public LayerMask PlayerLayer { get { return playerLayer; } }
        public RaycastHit2D DetectorAttackHit { get { return detectorAttackHit; } set { detectorAttackHit = value; } }
        public Vector2 DetectorDirection { get { return detectorDirection; } set { detectorDirection = value; } }
        public bool PlayerDetected { get { return playerDetected; } set { playerDetected = value; } }

        // Gameplay Info
        public bool CanWalk { get { return canWalk; } set { canWalk = value; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
        public bool CanTakeDamage { get { return canTakeDamage; } set { canTakeDamage = value; } }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody2D>();
            playerGameObject = GameObject.FindWithTag("Player");
            playerLayer = LayerMask.GetMask("Player");

            // Set enemy attributes
            //switch (enemyType)
            //{
                
            //}
        }
        #endregion
    }
}
