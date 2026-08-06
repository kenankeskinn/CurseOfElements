using PlayerManager;
using System.Collections;
using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class CombatController : MonoBehaviour
    {
        CompanionContext _companion;
        Coroutine releaseAttackCoroutine;

        #region Custom Functions
        public void SetTarget(GameObject target)
        {
            if (_companion.Target != null) { return; }

            // Set the target
            _companion.Target = target;
            _companion.CanFallowCharacter = false;

            if (releaseAttackCoroutine == null) StartCoroutine(ReleaseAttack());
        }

        void Attack(Transform target)
        {
            if (!_companion.CanAttack) { return; }

            target.GetComponent<EnemyManager.CombatController>().TakeDamage(_companion.MeleeDamage);
        }

        void Fallow(Transform target)
        {
            if (!_companion.CanWalk) { return; }

            if (transform.position.x < target.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = 1f * _companion.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                _companion.Rigidbody.linearVelocityX = -1f * _companion.WalkSpeed;
            }
        }

        IEnumerator ReleaseAttack()
        {
            yield return new WaitForSeconds(2);

            _companion.Target = null;
            releaseAttackCoroutine = null;
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _companion = GetComponent<CompanionContext>();
        }

        private void Update()
        {
            if (_companion.Target == null) { _companion.CanFallowCharacter = true; return; }

            // Raycast
            int direction;
            if (transform.localScale.x == 1) direction = 1;
            else direction = -1;
            RaycastHit2D targetHit = Physics2D.Raycast(transform.position, Vector2.right, direction * _companion.RangeOfAttack, _companion.TargetLayer);
            Debug.DrawRay(transform.position, Vector2.right * direction * _companion.RangeOfAttack, Color.darkViolet);

            // If we don't hit the target, follow it
            if (targetHit.collider == null) Fallow(_companion.Target.transform);
            else
            {
                // We have a hit: set as target, stop following player and attack
                _companion.Target = targetHit.transform.gameObject;
                _companion.CanFallowCharacter = false;
                Attack(targetHit.transform);
            }
        }
        #endregion
    }
}