using UnityEngine;

namespace EffectManager
{
    public enum BulletType
    {
        Fireball,
        Waterball,
        Windball
    }

    [CreateAssetMenu(fileName = "BulletScriptable", menuName = "Scriptable Objects/Create New Bullet Type")]
    public class BulletScriptable : ScriptableObject
    {
        // Variables
        [SerializeField] int enemyLayer = 9;
        [SerializeField] BulletType type;

        // Properties
        public int EnemyLayer { get { return enemyLayer; } }
        public BulletType Type { get { return type; } }
    }
}

