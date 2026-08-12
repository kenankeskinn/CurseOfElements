using System.Collections;
using UnityEngine;

namespace CompanionManager
{
    [RequireComponent(typeof(CompanionContext))]
    class CombatController : MonoBehaviour
    {
        Coroutine releaseAttackCoroutine;
        Coroutine resetAttackCoroutine;

        #region Custom Functions
        public void SetTarget(GameObject target)
        {
            if (CompanionContext.Instance.Target != null) { return; }

            // Set the target
            CompanionContext.Instance.Target = target;

            if (releaseAttackCoroutine == null) releaseAttackCoroutine = StartCoroutine(ReleaseAttack());
        }

        void Attack(Transform target)
        {
            if (!CompanionContext.Instance.CanAttack) { return; }

            if (resetAttackCoroutine == null) resetAttackCoroutine = StartCoroutine(ResetAttack(target));
        }

        void Follow(Transform target)
        {
            if (!CompanionContext.Instance.CanWalk) { return; }

            if (transform.position.x < target.position.x)
            {
                if (transform.localScale.x != 1) transform.localScale = new Vector2(1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = 1f * CompanionContext.Instance.WalkSpeed;
            }
            else
            {
                if (transform.localScale.x != -1) transform.localScale = new Vector2(-1, transform.localScale.y);
                CompanionContext.Instance.Rigidbody.linearVelocityX = -1f * CompanionContext.Instance.WalkSpeed;
            }

            CompanionContext.Instance.IsWalking = true;
        }

        IEnumerator ReleaseAttack()
        {
            yield return new WaitForSeconds(2);

            yield return new WaitUntil(() => CompanionContext.Instance.IsAttacking == false);

            CompanionContext.Instance.Target = null;
            releaseAttackCoroutine = null;
        }

        IEnumerator ResetAttack(Transform target)
        {
            target.GetComponent<EnemyManager.CombatController>().TakeDamage(CompanionContext.Instance.AttackDamage);

            CompanionContext.Instance.IsAttacking = true;
            Debug.Log("Reset Attack: Is Attacking = true");

            yield return new WaitForSeconds(CompanionContext.Instance.AttackResetTime); // attack reset time
            CompanionContext.Instance.IsAttacking = false;
            Debug.Log("Reset Attack: Is Attacking = false");

            yield return new WaitForSeconds(CompanionContext.Instance.AttackResetTime / 2);

            resetAttackCoroutine = null;
        }
        #endregion

        #region Unity Functions
        private void Update()
        {
            if (CompanionContext.Instance.Target == null) { return; }

            // ----------------------------------- If there is a Target -----------------------------------
            // Raycast Operations
            int direction;
            if (transform.localScale.x == 1) direction = 1;
            else direction = -1;
            RaycastHit2D targetHit = Physics2D.Raycast(transform.position, Vector2.right, direction * CompanionContext.Instance.RangeOfAttack, CompanionContext.Instance.TargetLayer);
            Debug.DrawRay(transform.position, Vector2.right * direction * CompanionContext.Instance.RangeOfAttack, Color.darkViolet);

            // Is there target
            if (targetHit.collider == null) 
            {
                // If we can't hit the target; follow it.
                Follow(CompanionContext.Instance.Target.transform);
            }
            else
            {
                // if we can hit the target; set as target, and stop following player and attack
                CompanionContext.Instance.Target = targetHit.transform.gameObject;
                Attack(targetHit.transform);
            }
            // --------------------------------------------------------------------------------------------
        }
        #endregion
    }
}