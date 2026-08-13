using PlayerManager;
using UnityEngine;

namespace EffectManager
{
    class BulletController : MonoBehaviour
    {
        [SerializeField] BulletScriptable bulletScriptable;

        private void Start()
        {
            Destroy(gameObject, 5);
        }

        private void Update()
        {
            if (transform.eulerAngles.z == 90) 
                transform.Translate(Vector2.right * 10 * Time.deltaTime, Space.World);
            else 
                transform.Translate(Vector2.left * 10 * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == bulletScriptable.EnemyLayer) // if collider layer is enemy
            {
                EnemyManager.CombatController enemyCombat = collider.GetComponent<EnemyManager.CombatController>();

                enemyCombat.TakeDamage(CombatController.CalculateDamage(AttackType.Ranged));

                switch (bulletScriptable.Type)
                {
                    case BulletType.Fireball:
                        enemyCombat.ApplyEffectSelf(EffectType.Burn);
                        break;
                    case BulletType.Waterball:
                        enemyCombat.ApplyEffectSelf(EffectType.Slow);
                        break;
                    case BulletType.Windball:
                        enemyCombat.ApplyEffectSelf(EffectType.Push);
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}
