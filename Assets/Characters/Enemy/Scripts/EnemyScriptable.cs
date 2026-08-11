using UnityEngine;

namespace EnemyManager
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Create New Enemy Type")]
    public class EnemyScriptable : ScriptableObject
    {
        // Variables
        [Header("General Settings")]
        [SerializeField] EnemyType enemyType;
        [SerializeField] int maxHealth = 10;
        [SerializeField][Range(1, 10)] int walkSpeed = 1;

        [Header("Combat Settings")]
        [SerializeField] int attackDamage = 10;
        [SerializeField] float rangeOfView = 8;
        [SerializeField] float rangeOfAttack = 1.25f;
        [SerializeField] float attackResetTime = 1;
        [SerializeField] LayerMask playerLayer = 1 << 7;

        // Properties
        public EnemyType EnemyType { get { return enemyType; } }
        public int MaxHealth { get { return maxHealth; } }
        public int WalkSpeed { get { return walkSpeed; } }
        public int AttackDamage { get { return attackDamage; } }
        public float RangeOfView { get { return rangeOfView; } }
        public float RangeOfAttack { get { return rangeOfAttack; } }
        public float AttackResetTime { get { return attackResetTime; } }
        public LayerMask PlayerLayer { get { return playerLayer; } }
    }
}