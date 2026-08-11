using System;
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

        // Stats
        public int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (value < 0) value = 0;
                else if (value > EnemyScriptable.MaxHealth) value = EnemyScriptable.MaxHealth;
                else currentHealth = value;
            }
        }

        public EnemyScriptable EnemyScriptable { get { return enemyScriptable; } }

        // Animation
        public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
        public bool IsAttacking { get { return isAttacking; }  set { isAttacking = value; } }
        public bool IsTakingDamage { get { return isTakingDamage; } set { isTakingDamage = value; } }

        // Info
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
            currentHealth = EnemyScriptable.MaxHealth;
            rb = GetComponent<Rigidbody2D>();
            playerGameObject = GameObject.FindWithTag("Player");

            // Set enemy attributes
            //switch (enemyType)
            //{
                
            //}
        }
        #endregion
    }
}
