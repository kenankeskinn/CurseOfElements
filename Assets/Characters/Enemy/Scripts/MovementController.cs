using UnityEngine;

namespace EnemyManager
{
    [RequireComponent(typeof(EnemyContext))]
    public class MovemenetController : MonoBehaviour
    {
        EnemyContext _enemy;
        Vector2[] patrolPositions = new Vector2[2];
        int currentPatrolState = 0;

        #region Custom Functions

        #region Status Decision-Makers
        void PlayerSeenCheck()
        {
            RaycastHit2D detectorSeenHit = Physics2D.Raycast(transform.position, _enemy.DetectorDirection, _enemy.EnemyScriptable.RangeOfView, _enemy.EnemyScriptable.PlayerLayer); // PlayerLayer + CompanionLayer
            Debug.DrawRay(transform.position, _enemy.DetectorDirection * _enemy.EnemyScriptable.RangeOfView, Color.purple);

            _enemy.IsPlayerSeen = detectorSeenHit.collider != null;

            if (!_enemy.IsPlayerSeen) Patrol();
            else _enemy.ChaseTarget = _enemy.PlayerGameObject.transform;
        }

        void Patrol()
        {
            if (!_enemy.CanWalk) { return; }

            if (currentPatrolState >= patrolPositions.Length) currentPatrolState = 0;

            if (currentPatrolState == 0) Move(true);
            else Move(false);

            float distance = Mathf.Abs(transform.position.x - patrolPositions[currentPatrolState].x);

            if (distance < 0.25f) currentPatrolState++;
        }

        void Chase()
        {
            if (!_enemy.CanWalk) { _enemy.IsWalking = false; return; }

            LookAtTheTarget(_enemy.ChaseTarget);

            if (_enemy.DetectorAttackHit.collider == null)
            {
                if (_enemy.ChaseTarget.position.x > transform.position.x) Move(true);
                else if (_enemy.ChaseTarget.position.x < transform.position.x) Move(false);
                else _enemy.IsWalking = false;
            }
            else _enemy.IsWalking = false;
        }
        #endregion

        #region Actioners
        void Move(bool isRight)
        {
            float speedMultiplier = 1;

            if (_enemy.IsSlowing)
            {
                speedMultiplier = .35f;
            }

            if (isRight)
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
                _enemy.Rigidbody.linearVelocityX = _enemy.EnemyScriptable.WalkSpeed * speedMultiplier;
            }
            else
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
                _enemy.Rigidbody.linearVelocityX = -_enemy.EnemyScriptable.WalkSpeed * speedMultiplier;
            }

            _enemy.IsWalking = true;
        }

        void LookAtTheTarget(Transform target)
        {
            if (target.position.x > transform.position.x)
                transform.localScale = new Vector2(1, transform.localScale.y);
            else if (target.position.x < transform.position.x)
                transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        void StopTheSystem()
        {
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;
            _enemy.CanTakeDamage = false;
        }
        #endregion

        #endregion

        #region Unity Functions
        private void Awake()
        {
            _enemy = GetComponent<EnemyContext>();

            patrolPositions[0] = new Vector2(transform.position.x + 5, transform.position.y);
            patrolPositions[1] = new Vector2(transform.position.x - 5, transform.position.y);
        }

        private void FixedUpdate()
        {
            if (_enemy.PlayerGameObject == null || _enemy.IsDead) { StopTheSystem(); return; }

            // Detector direction control
            if (transform.localScale.x == 1)
            {
                _enemy.DetectorDirection = Vector2.right;
            }
            else if (transform.localScale.x == -1)
            {
                _enemy.DetectorDirection = Vector2.left;
            }

            if (!_enemy.IsPlayerSeen)
                PlayerSeenCheck();
            else
                Chase();
        }
        #endregion
    }
}