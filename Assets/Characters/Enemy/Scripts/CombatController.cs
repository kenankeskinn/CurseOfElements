using System.Collections;
using UnityEngine;

namespace EnemyManager
{
    [RequireComponent(typeof(EnemyContext))]
    class CombatController : MonoBehaviour
    {
        EnemyContext _enemy;
        Coroutine attackCoroutine;
        Coroutine takeDamageCoroutine;
        PlayerManager.CombatController playerCombatController;

        #region Custom Functions
        void PlayerCheck()
        {
            _enemy.DetectorAttackHit = Physics2D.Raycast(transform.position, _enemy.DetectorDirection, _enemy.RangeOfAttack, _enemy.PlayerLayer);
            Debug.DrawRay(transform.position, _enemy.DetectorDirection * _enemy.RangeOfAttack, Color.aquamarine);

            // if AttackHit null
            if (_enemy.DetectorAttackHit.collider == null) { return; }

            // if Can Attack
            if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
        }

        IEnumerator Attack()
        {
            if (!_enemy.CanAttack) { attackCoroutine = null; yield break; }

            // Attack Function (there is only Player to take damage because of that we don't need to take damage takeable object)
            playerCombatController.TakeDamage(_enemy.AttackDamage, gameObject);

            // Reset Operations
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;

            _enemy.IsAttacking = true; 
            yield return new WaitForSeconds(.5f); // animation reset time
            _enemy.IsAttacking = false;

            yield return new WaitForSeconds(_enemy.AttackResetTime);

            _enemy.CanWalk = true;
            _enemy.CanAttack = true;

            attackCoroutine = null;
        }

        public void TakeDamage(int damage)
        {
            if (!_enemy.CanTakeDamage) { return; }

            _enemy.CurrentHealth -= damage;
            if (takeDamageCoroutine == null) takeDamageCoroutine = StartCoroutine(ResetTakeDamage());

            if (_enemy.CurrentHealth <= 0) Die();
        }

        void Die()
        {
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;
            _enemy.CanTakeDamage = false;

            Debug.Log($"{name} Died!");
            Destroy(gameObject);
        }

        IEnumerator ResetTakeDamage()
        {
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;
            _enemy.IsTakingDamage = true;

            yield return new WaitForSeconds(.34f); // Take damage animation reset

            _enemy.CanWalk = true;
            _enemy.CanAttack = true;
            _enemy.IsTakingDamage = false;

            takeDamageCoroutine = null;
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _enemy = GetComponent<EnemyContext>();
            playerCombatController = _enemy.PlayerGameObject.GetComponent<PlayerManager.CombatController>();
        }

        private void Update()
        {
            PlayerCheck();
        }
        #endregion
    }
}