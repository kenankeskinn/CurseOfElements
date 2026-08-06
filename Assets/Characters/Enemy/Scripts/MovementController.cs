using UnityEngine;

namespace EnemyManager
{
    [RequireComponent(typeof(EnemyContext))]
    public class MovementController : MonoBehaviour
    {
        EnemyContext _enemy;
        Vector2[] patrolPositions = new Vector2[2];
        int currentPatrolState = 0;

        #region Custom Functions
        void PlayerSeenCheck()
        {
            RaycastHit2D detectorSeenHit = Physics2D.Raycast(transform.position, _enemy.DetectorDirection, _enemy.RangeOfView, _enemy.PlayerLayer);
            Debug.DrawRay(transform.position, _enemy.DetectorDirection * _enemy.RangeOfView, Color.purple);

            _enemy.PlayerDetected = detectorSeenHit.collider != null;

            if (!_enemy.PlayerDetected) Patrol();
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

            LookAtPlayer();

            if (_enemy.DetectorAttackHit.collider == null)
            {
                if (_enemy.PlayerGameObject.transform.position.x > transform.position.x) Move(true);
                else if (_enemy.PlayerGameObject.transform.position.x < transform.position.x) Move(false);
                else { _enemy.IsWalking = false; }
            }
            else _enemy.IsWalking = false;
        }

        void Move(bool isRight)
        {
            if (isRight)
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
                _enemy.Rigidbody.linearVelocityX = _enemy.WalkSpeed;
            }
            else
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
                _enemy.Rigidbody.linearVelocityX = -_enemy.WalkSpeed;
            }

            _enemy.IsWalking = true;
        }

        void DetectorController()
        {
            if (transform.localScale.x == 1)
            {
                _enemy.DetectorDirection = Vector2.right;
            }
            else
            {
                _enemy.DetectorDirection = Vector2.left;
            }
        }

        void LookAtPlayer()
        {
            if (_enemy.PlayerGameObject.transform.position.x > transform.position.x) 
                transform.localScale = new Vector2(1, transform.localScale.y);
            else if (_enemy.PlayerGameObject.transform.position.x < transform.position.x)
                transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        void StopTheSystem()
        {
            _enemy.CanWalk = false;
            _enemy.CanAttack = false;
            _enemy.CanTakeDamage = false;
        }
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
            if (_enemy.PlayerGameObject == null) { StopTheSystem(); return; }

            DetectorController();

            if (!_enemy.PlayerDetected) PlayerSeenCheck();
            else Chase();
        }
        #endregion
    }
}