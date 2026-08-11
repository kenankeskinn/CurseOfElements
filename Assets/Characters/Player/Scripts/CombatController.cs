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

            RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, Vector2.right, lookDirection * PlayerContext.Instance.RangeOfAttack, enemyLayer);

            // -- After hitting an enemy --
            if (enemyHit.collider != null)
            {
                // 1-) Deal Damage
                enemyHit.transform.GetComponent<EnemyManager.CombatController>().TakeDamage(CalculateDamage(attackType));

                // 2-) Apply Attack Effects if there is an Element.
                if      (PlayerContext.Instance.SelectedElement == Element.Wind)   WindEffect (attackType,  enemyHit.collider.gameObject);
                else if (PlayerContext.Instance.SelectedElement == Element.Water)  WaterEffect(attackType,  enemyHit.collider.gameObject);
                else if (PlayerContext.Instance.SelectedElement == Element.Fire)   FireEffect (attackType,  enemyHit.collider.gameObject);
            }
            // ----------------------------

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
                Debug.LogError("Companion Combat Controller is Null");
                return;
            }

            if (PlayerContext.Instance.CurrentHealth <= 0) Die();
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

        // Element Effects
        void WindEffect(AttackType attackType, GameObject target) 
        { 
            if (attackType == AttackType.Melee) // Pushes back to enemies
            {
                Debug.Log("Melee Wind Attack");
            }
            else                                // Throwing wind ball and pushes back little
            {
                Debug.Log("Ranged Wind Attack");
            }
        }

        void WaterEffect(AttackType attackType, GameObject target)
        {
            if (attackType == AttackType.Melee) // Freezes for a short time
            {
                Debug.Log("Melee Water Attack");
            }
            else                                // Movement and attackSpeed slow
            {
                Debug.Log("Ranged Water Attack");
            }
        }

        void FireEffect(AttackType attackType, GameObject target)
        {
            if (attackType == AttackType.Melee) // Extra damage
            {
                Debug.Log("Melee Fire Attack");
            }
            else                                // Deals damage over time with a burning effect
            {
                Debug.Log("Ranged Fire Attack");
            }
        }

        // Support Functions
        IEnumerator ResetAttack(AttackType attackType)
        {
            PlayerContext.Instance.CanWalk = false;
            PlayerContext.Instance.CanJump = false;

            if (attackType == AttackType.Melee) yield return new WaitForSeconds(PlayerContext.Instance.MeleeResetTime);
            else yield return new WaitForSeconds(PlayerContext.Instance.RangedResetTime);

            PlayerContext.Instance.CanWalk = true;
            PlayerContext.Instance.CanJump = true;
            PlayerContext.Instance.IsMeleeAttacking = false;
            PlayerContext.Instance.IsRangedAttacking = false;

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

        int CalculateDamage(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
            {
                switch (PlayerContext.Instance.SelectedElement)
                {
                    case Element.Wind: return PlayerContext.Instance.MeleeDamage + 3;
                    case Element.Water: return PlayerContext.Instance.MeleeDamage + 5;
                    case Element.Fire: return PlayerContext.Instance.MeleeDamage + 10;
                    default: return PlayerContext.Instance.MeleeDamage;
                }
            }
            else if (attackType == AttackType.Ranged)
            {
                switch (PlayerContext.Instance.SelectedElement)
                {
                    case Element.Wind: return PlayerContext.Instance.RangedDamage + 3;
                    case Element.Water: return PlayerContext.Instance.RangedDamage + 5;
                    case Element.Fire: return PlayerContext.Instance.RangedDamage + 10;
                    default: return PlayerContext.Instance.RangedDamage;
                }
            }
            else return 0;
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
