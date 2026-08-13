using System.Collections;
using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    class CombatController : MonoBehaviour
    {
        Coroutine attackCoroutine;
        Coroutine takeDamageCoroutine;
        LayerMask enemyLayer;
        CompanionManager.CombatController companionCombatController;

        #region Custom Functions

        void Attack()
        {
            // Debug Ray
            if (transform.localScale.x == 1) 
                Debug.DrawRay(transform.position, Vector2.right * PlayerContext.Instance.RangeOfAttack, Color.darkRed);
            else
                Debug.DrawRay(transform.position, Vector2.left * PlayerContext.Instance.RangeOfAttack, Color.darkRed);

            // -------------------------------------------- Function Task --------------------------------------------

            if (!PlayerContext.Instance.CanAttack || (!PlayerContext.Instance.MeleeInput && !PlayerContext.Instance.RangedInput) || attackCoroutine != null) { return; }

            AttackType attackType;

            if (PlayerContext.Instance.MeleeInput) attackType = AttackType.Melee;
            else attackType = AttackType.Ranged;
            SetAttackState(attackType);

            int lookDirection;
            if (transform.localScale.x == 1) lookDirection = 1;
            else lookDirection = -1;

            // ---------------------------- Ranged Attack ----------------------------
            if (attackType == AttackType.Ranged)
            {
                GameObject bulletPrefab;

                if (PlayerContext.Instance.SelectedElement == Element.Fire)
                    bulletPrefab = PlayerContext.Instance.ElementBullets[0];
                else if (PlayerContext.Instance.SelectedElement == Element.Water)
                    bulletPrefab = PlayerContext.Instance.ElementBullets[1];
                else if (PlayerContext.Instance.SelectedElement == Element.Wind)
                    bulletPrefab = PlayerContext.Instance.ElementBullets[2];
                else
                    bulletPrefab = null;

                if (bulletPrefab != null)
                {
                    StartCoroutine(CreateBullet(bulletPrefab, lookDirection));                 
                }
            }
            // ----------------------------------------------------------------------


            // ---------------------------- Melee Attack ----------------------------
            if (attackType == AttackType.Melee)
            {                
                RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, Vector2.right, lookDirection * PlayerContext.Instance.RangeOfAttack, enemyLayer);

                // -- After hitting an enemy --
                if (enemyHit.collider != null)
                {
                    EnemyManager.CombatController enemyCombat = enemyHit.transform.GetComponent<EnemyManager.CombatController>();

                    // 1-) Deal Damage
                    enemyCombat.TakeDamage(CalculateDamage(AttackType.Melee));

                    // 2-) Apply Attack Effects if there is an Element.
                    if (PlayerContext.Instance.SelectedElement == Element.Fire) enemyCombat.ApplyEffectSelf(EffectType.Burn);
                    else if (PlayerContext.Instance.SelectedElement == Element.Water) enemyCombat.ApplyEffectSelf(EffectType.Slow);
                    else if (PlayerContext.Instance.SelectedElement == Element.Wind) enemyCombat.ApplyEffectSelf(EffectType.Push);
                }
            }            
            // ----------------------------------------------------------------------

            // Reset Attack
            attackCoroutine = StartCoroutine(ResetAttack(attackType));
        }

        public void TakeDamage(int damage, GameObject enemy)
        {
            if (!PlayerContext.Instance.CanTakeDamage) { return; }

            PlayerContext.Instance.CurrentHealth -= damage;
            if (takeDamageCoroutine == null) takeDamageCoroutine = StartCoroutine(ResetTakeDamage());

            // Send message to companion
            if (companionCombatController != null)
                companionCombatController.SetTarget(enemy);
            else
            {
                //Debug.LogError("Companion Combat Controller is Null");
                return;
            }

            if (PlayerContext.Instance.CurrentHealth <= 0) Die();
        }

        public void Heal(int heal)
        {
            PlayerContext.Instance.CurrentHealth += heal;
        }

        void Die()
        {
            PlayerContext.Instance.CanWalk = false;
            PlayerContext.Instance.CanJump = false;
            PlayerContext.Instance.CanAttack = false;
            PlayerContext.Instance.CanTakeDamage = false;

            Debug.Log($"{name} Died!");
            Destroy(gameObject);
        }

        // Support Functions
        IEnumerator ResetAttack(AttackType attackType)
        {
            PlayerContext.Instance.CanWalk = false;
            PlayerContext.Instance.CanJump = false;
            PlayerContext.Instance.Rigidbody.linearVelocityY = 0;
            PlayerContext.Instance.Rigidbody.gravityScale = 0;

            if (attackType == AttackType.Melee) yield return new WaitForSeconds(PlayerContext.Instance.MeleeResetTime);
            else yield return new WaitForSeconds(PlayerContext.Instance.RangedResetTime);
            
            PlayerContext.Instance.Rigidbody.gravityScale = 2;
            PlayerContext.Instance.IsMeleeAttacking = false;
            PlayerContext.Instance.IsRangedAttacking = false;

            yield return new WaitForSeconds(.25f);

            PlayerContext.Instance.CanWalk = true;
            PlayerContext.Instance.CanJump = true;

            attackCoroutine = null;
        }

        IEnumerator ResetTakeDamage()
        {
            PlayerContext.Instance.CanWalk = false;
            PlayerContext.Instance.CanJump = false;
            PlayerContext.Instance.CanAttack = false;
            PlayerContext.Instance.IsTakingDamage = true;

            yield return new WaitForSeconds(.34f);

            PlayerContext.Instance.CanWalk = true;
            PlayerContext.Instance.CanJump = true;
            PlayerContext.Instance.CanAttack = true;
            PlayerContext.Instance.IsTakingDamage = false;

            takeDamageCoroutine = null;
        }

        void SetAttackState(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
            {
                PlayerContext.Instance.IsRangedAttacking = false;
                PlayerContext.Instance.IsMeleeAttacking = true;
            }
            else if (attackType == AttackType.Ranged)
            {
                PlayerContext.Instance.IsMeleeAttacking = false;
                PlayerContext.Instance.IsRangedAttacking = true;
            }
        }

        public static int CalculateDamage(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
            {
                switch (PlayerContext.Instance.SelectedElement)
                {
                    case Element.Fire: return PlayerContext.Instance.MeleeDamage + 3;
                    case Element.Water: return PlayerContext.Instance.MeleeDamage + 5;
                    case Element.Wind: return PlayerContext.Instance.MeleeDamage + 10;
                    default: return PlayerContext.Instance.MeleeDamage;
                }
            }
            else
            {
                switch (PlayerContext.Instance.SelectedElement)
                {
                    case Element.Fire: return PlayerContext.Instance.RangedDamage;
                    case Element.Water: return PlayerContext.Instance.RangedDamage + 3;
                    case Element.Wind: return PlayerContext.Instance.RangedDamage + 5;
                    default: return 0;
                }
            }            
        }

        IEnumerator CreateBullet(GameObject bulletPrefab, float lookDirection)
        {
            yield return new WaitForSeconds(PlayerContext.Instance.RangedResetTime - 0.2f);

            Transform bullet = Instantiate(bulletPrefab).transform;
            bullet.localPosition = PlayerContext.Instance.BulletStartTransform.position;
            bullet.rotation = Quaternion.Euler(0, 0, 90 * lookDirection);
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            enemyLayer = LayerMask.GetMask("Enemy");
            companionCombatController = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionManager.CombatController>();
        }

        private void Update()
        {
            Attack();
        }
        #endregion
    }
}
