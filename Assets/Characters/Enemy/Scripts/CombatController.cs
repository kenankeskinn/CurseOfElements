using PlayerManager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyManager
{
    [RequireComponent(typeof(EnemyContext))]
    class CombatController : MonoBehaviour
    {
        EnemyContext _enemy;
        Coroutine attackCoroutine;
        Coroutine takeDamageCoroutine;
        Vector2 detectorPos;

        PlayerManager.CombatController playerCombatController;
        CompanionManager.CombatController companionCombatController;

        [Header("UI Operations")]
        [SerializeField] Image healthBar;

        #region Custom Functions
        void PlayerCheck()
        {
            Vector2 detectorPos = new Vector2(transform.position.x, transform.position.y - 0.25f);

            _enemy.DetectorAttackHit = Physics2D.Raycast(detectorPos, _enemy.DetectorDirection, _enemy.EnemyScriptable.RangeOfAttack, _enemy.EnemyScriptable.AttackableCharacterLayers);
            Debug.DrawRay(detectorPos, _enemy.DetectorDirection * _enemy.EnemyScriptable.RangeOfAttack, Color.aquamarine);

            // if AttackHit null
            if (_enemy.DetectorAttackHit.collider == null) { return; }

            // if Can Attack
            if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
        }

        IEnumerator Attack()
        {
            if (!_enemy.CanAttack || _enemy.IsTakingDamage) { attackCoroutine = null; yield break; }

            // Attack Function (there is only Player to take damage because of that we don't need to take damage takeable object)
            if (_enemy.DetectorAttackHit.collider.gameObject == _enemy.PlayerGameObject)
            {
                playerCombatController.TakeDamage(_enemy.EnemyScriptable.AttackDamage, gameObject);
                Debug.Log("Attacked to Player");
            }
            else if (_enemy.DetectorAttackHit.collider.gameObject == _enemy.CompanionGameObject)
            {
                companionCombatController.TakeDamage(_enemy.EnemyScriptable.AttackDamage, gameObject);
                Debug.Log("Attacked to Companion");
            }
            else
            {
                Debug.LogError("DetectorAttackHit hitted unknown object!");
            }

            // Reset Operations
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;

            _enemy.IsAttacking = true; 
            yield return new WaitForSeconds(.5f); // animation reset time
            _enemy.IsAttacking = false;

            yield return new WaitForSeconds(_enemy.EnemyScriptable.AttackResetTime);

            _enemy.CanWalk = true;
            _enemy.CanAttack = true;

            attackCoroutine = null;
        }

        public void TakeDamage(int damage, GameObject character = null)
        {
            if (!_enemy.CanTakeDamage) { return; }

            if (_enemy.IsTakingDamage) { return; }

            _enemy.CurrentHealth -= damage;
            healthBar.fillAmount -= damage / (float)_enemy.EnemyScriptable.MaxHealth;
            if (takeDamageCoroutine == null) takeDamageCoroutine = StartCoroutine(ResetTakeDamage());

            _enemy.IsPlayerSeen = true;

            if (character != null) _enemy.ChaseTarget = character.transform;

            if (_enemy.IsDead) StartCoroutine(Die());
        }

        IEnumerator Die()
        {
            GetComponent<Collider2D>().enabled = false;
            _enemy.Rigidbody.bodyType = RigidbodyType2D.Static;
            _enemy.Rigidbody.linearDamping = 1;
            _enemy.Rigidbody.angularDamping = 1;
            yield return new WaitForSeconds(3);

            Debug.Log($"{name} Died!");

            playerCombatController.Heal(_enemy.EnemyScriptable.HealReward);
            companionCombatController.Heal(_enemy.EnemyScriptable.HealReward + 5);

            Destroy(gameObject);
        }

        IEnumerator ResetTakeDamage()
        {
            _enemy.IsTakingDamage = true;

            yield return new WaitForSeconds(.34f); // Take damage animation reset

            _enemy.IsTakingDamage = false;

            takeDamageCoroutine = null;
        }

        public void ApplyEffectSelf(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Burn:
                    if (!_enemy.IsBurning) StartCoroutine(BurnEffect());
                    break;
                case EffectType.Slow:
                    if (!_enemy.IsSlowing) StartCoroutine(SlowEffect());
                    break;
                case EffectType.Push:
                    if (!_enemy.IsPushing) StartCoroutine(PushEffect());
                    break;
            }
        }

        // Element Effects
        IEnumerator BurnEffect()
        {
            _enemy.IsBurning = true;

            yield return new WaitForSeconds(1);
            TakeDamage(1);
            yield return new WaitForSeconds(1);
            TakeDamage(1);
            yield return new WaitForSeconds(1);

            _enemy.IsBurning = false;
        }

        IEnumerator SlowEffect()
        {
            _enemy.IsSlowing = true;

            yield return new WaitForSeconds(3);

            _enemy.IsSlowing = false;
        }
        IEnumerator PushEffect()
        {
            _enemy.IsPushing = true;      

            if (_enemy.PlayerGameObject.transform.localScale.x == 1)
                _enemy.Rigidbody.linearVelocityX = _enemy.EnemyScriptable.WalkSpeed * 5f;
            else
                _enemy.Rigidbody.linearVelocityX = -_enemy.EnemyScriptable.WalkSpeed * 5f;

            yield return new WaitForSeconds(2);

            _enemy.IsPushing = false;
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _enemy = GetComponent<EnemyContext>();
        }

        private void Start()
        {
            playerCombatController = _enemy.PlayerGameObject.GetComponent<PlayerManager.CombatController>();
            companionCombatController = _enemy.CompanionGameObject.GetComponent<CompanionManager.CombatController>();
        }

        private void Update()
        {
            if (!_enemy.IsDead)
                PlayerCheck();
        }
        #endregion
    }
}