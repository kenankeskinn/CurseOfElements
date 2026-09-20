using System;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyManager
{
    #region Enums
    public enum EnemyType
    {
        Zombie,
        Skeleton,
        Ghoul,
        FlyingDemon,
        Necromancer
    }
    #endregion

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyContext : MonoBehaviour
    {
        #region Variables
        // References
        private Rigidbody2D rb;
        private GameObject playerGameObject;
        private GameObject companionGameObject;

        [Header("-- General --")]
        [SerializeField][Range(0, 100)] int currentHealth;
        [SerializeField] EnemyScriptable enemyScriptable;

        [Header("-- Animation --")]
        [SerializeField] bool isWalking;
        [SerializeField] bool isAttacking;
        [SerializeField] bool isTakingDamage;
        [SerializeField] bool isDead;

        [Header("-- Info --")]
        [SerializeField] RaycastHit2D detectorAttackHit;
        [SerializeField] Vector2 detectorDirection;
        [SerializeField] Transform chaseTarget;
        [SerializeField] bool isPlayerSeen;

        [Header("-- Gameplay Info --")]
        [SerializeField] bool canWalk = true;
        [SerializeField] bool canAttack = true;
        [SerializeField] bool canTakeDamage = true;
        [SerializeField] bool isBurning = false;
        [SerializeField] bool isSlowing = false;
        [SerializeField] bool isPushing = false;
        #endregion

        #region Properties
        // References
        public Rigidbody2D Rigidbody { get { return rb; } }
        public GameObject PlayerGameObject { get { return playerGameObject; } }
        public GameObject CompanionGameObject { get { return companionGameObject; } }

        // Stats
        public int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (value <= 0)
                {
                    value = 0;
                    IsDead = true;
                }
                else if (value > EnemyScriptable.MaxHealth) value = EnemyScriptable.MaxHealth;
                
                currentHealth = value;
            }
        }

        public EnemyScriptable EnemyScriptable { get { return enemyScriptable; } }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsAttacking { get { return isAttacking; }  set { isAttacking = value; } }
        public bool IsTakingDamage 
        { 
            get { return isTakingDamage; } 
            set 
            { 
                if (value == true)
                {
                    CanWalk = false;
                    CanAttack = false;
                    CanTakeDamage = false;
                }
                else
                {
                    CanWalk = true;
                    CanAttack = true;
                    CanTakeDamage = true;
                }

                isTakingDamage = value; 
            } 
        }
        public bool IsDead 
        { 
            get { return isDead; } 
            set 
            { 
                if (value == true)
                {
                    CanWalk = false;
                    CanAttack = false;
                    CanTakeDamage = false;
                }
                isDead = value; 
            } 
        }

        // Info
        public RaycastHit2D DetectorAttackHit { get { return detectorAttackHit; } set { detectorAttackHit = value; } }
        public Vector2 DetectorDirection { get { return detectorDirection; } set { detectorDirection = value; } }
        public Transform ChaseTarget { get { return chaseTarget; } set { chaseTarget = value; } }
        public bool IsPlayerSeen { get { return isPlayerSeen; } set { isPlayerSeen = value; } }

        // Gameplay Info
        public bool CanWalk { get { return canWalk; } set { canWalk = value; } }
        public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
        public bool CanTakeDamage { get { return canTakeDamage; } set { canTakeDamage = value; } }
        public bool IsBurning { get { return isBurning; } set { isBurning = value; } }
        public bool IsSlowing { get { return isSlowing; } set { isSlowing = value; } }
        public bool IsPushing 
        { 
            get { return isPushing; } 
            set 
            { 
                if (value == true)
                {
                    CanWalk = false;
                    CanAttack = false;
                    CanTakeDamage = false;
                }
                else
                {
                    CanWalk = true;
                    CanAttack = true;
                    CanTakeDamage = true;
                }

                isPushing = value; 
            } 
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            currentHealth = EnemyScriptable.MaxHealth;
            rb = GetComponent<Rigidbody2D>();
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            companionGameObject = GameObject.FindGameObjectWithTag("Companion");
        }
        #endregion
    }
}
