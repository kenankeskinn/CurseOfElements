using System.Collections;
using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(typeof(PlayerContext))]
    class CombatController : MonoBehaviour
    {
        PlayerContext _player;
        Coroutine attackCoroutine;
        Coroutine takeDamageCoroutine;
        LayerMask enemyLayer;
        CompanionManager.CombatController companionCombatController;

        #region Custom Functions

        void Attack()
        {
            // Debug Ray
            if (transform.localScale.x == 1) 
                Debug.DrawRay(transform.position, Vector2.right * _player.RangeOfAttack, Color.darkRed);
            else
                Debug.DrawRay(transform.position, Vector2.left * _player.RangeOfAttack, Color.darkRed);

            // -------------------------------------------- Function Task --------------------------------------------

            if (!_player.CanAttack || (!_player.MeleeInput && !_player.RangedInput) || attackCoroutine != null) { return; }

            AttackType attackType;

            if (_player.MeleeInput) attackType = AttackType.Melee;
            else attackType = AttackType.Ranged;
            SetAttackState(attackType);

            int lookDirection;
            if (transform.localScale.x == 1) lookDirection = 1;
            else lookDirection = -1;

            RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, Vector2.right, lookDirection * _player.RangeOfAttack, enemyLayer);

            // -- After hitting an enemy --
            if (enemyHit.collider != null)
            {
                // 1-) Deal Damage
                enemyHit.transform.GetComponent<EnemyManager.CombatController>().TakeDamage(CalculateDamage(attackType));

                // 2-) Apply Attack Effects if there is an Element.
                if      (_player.SelectedElement == Element.Wind)   WindEffect (attackType,  enemyHit.collider.gameObject);
                else if (_player.SelectedElement == Element.Water)  WaterEffect(attackType,  enemyHit.collider.gameObject);
                else if (_player.SelectedElement == Element.Fire)   FireEffect (attackType,  enemyHit.collider.gameObject);
            }
            // ----------------------------

            // Reset Attack
            attackCoroutine = StartCoroutine(ResetAttack(attackType));
        }

        public void TakeDamage(int damage, GameObject enemy)
        {
            if (!_player.CanTakeDamage) { return; }

            _player.CurrentHealth -= damage;
            if (takeDamageCoroutine == null) takeDamageCoroutine = StartCoroutine(ResetTakeDamage());

            // Send message to companion
            if (companionCombatController != null)
                companionCombatController.SetTarget(enemy);
            else
            {
                Debug.LogError("Companion Combat Controller is Null");
                return;
            }

            if (_player.CurrentHealth <= 0) Die();
        }

        void Die()
        {
            _player.CanWalk = false;
            _player.CanJump = false;
            _player.CanAttack = false;
            _player.CanTakeDamage = false;

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
            _player.CanWalk = false;
            _player.CanJump = false;

            if (attackType == AttackType.Melee) yield return new WaitForSeconds(_player.MeleeResetTime);
            else yield return new WaitForSeconds(_player.RangedResetTime);

            _player.CanWalk = true;
            _player.CanJump = true;
            _player.IsMeleeAttacking = false;
            _player.IsRangedAttacking = false;

            attackCoroutine = null;
        }

        IEnumerator ResetTakeDamage()
        {
            _player.CanWalk = false;
            _player.CanJump = false;
            _player.CanAttack = false;
            _player.IsTakingDamage = true;

            yield return new WaitForSeconds(.34f);

            _player.CanWalk = true;
            _player.CanJump = true;
            _player.CanAttack = true;
            _player.IsTakingDamage = false;

            takeDamageCoroutine = null;
        }

        void SetAttackState(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
            {
                _player.IsRangedAttacking = false;
                _player.IsMeleeAttacking = true;
            }
            else if (attackType == AttackType.Ranged)
            {
                _player.IsMeleeAttacking = false;
                _player.IsRangedAttacking = true;
            }
        }

        int CalculateDamage(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
            {
                switch (_player.SelectedElement)
                {
                    case Element.Wind: return _player.MeleeDamage + 3;
                    case Element.Water: return _player.MeleeDamage + 5;
                    case Element.Fire: return _player.MeleeDamage + 10;
                    default: return _player.MeleeDamage;
                }
            }
            else if (attackType == AttackType.Ranged)
            {
                switch (_player.SelectedElement)
                {
                    case Element.Wind: return _player.RangedDamage + 3;
                    case Element.Water: return _player.RangedDamage + 5;
                    case Element.Fire: return _player.RangedDamage + 10;
                    default: return _player.RangedDamage;
                }
            }
            else return 0;
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            _player = GetComponent<PlayerContext>();
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
