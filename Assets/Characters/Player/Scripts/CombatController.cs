using System.Collections;
using UnityEngine;

namespace PlayerManager
{
    class CombatController : MonoBehaviour
    {
        PlayerContext _player;
        float meleeResetTime = .75f;
        float rangedResetTime = .75f;

        #region Custom Functions

        void Attack()
        {
            if (!_player.CanAttack || (!_player.MeleeInput && !_player.RangedInput)) { return; }

            AttackType attackType;

            if (_player.MeleeInput) attackType = AttackType.Melee;
            else attackType = AttackType.Ranged;

            // -- After hitting an enemy --
            // 1-) Deal Damage
            Debug.Log($"Damage: {CalculateDamage(attackType)}");

            // 2-) Apply Attack Effects if there is an Element.
            if (_player.SelectedElement == Element.Wind)        WindEffect(attackType);
            else if (_player.SelectedElement == Element.Water)  WaterEffect(attackType);
            else if (_player.SelectedElement == Element.Fire)   FireEffect(attackType);
            // ----------------------------

            // Reset Attack
            StartCoroutine(ResetAttack(attackType));
        }

        void TakeDamage(int damage)
        {
            _player.CurrentHealth -= damage;
        }

        // Element Effects
        void WindEffect(AttackType attackType) 
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

        void WaterEffect(AttackType attackType)
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

        void FireEffect(AttackType attackType)
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
            _player.CanAttack = false;

            if (attackType == AttackType.Melee) yield return new WaitForSeconds(meleeResetTime);
            else yield return new WaitForSeconds(rangedResetTime);

            _player.CanAttack = true;
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
        }

        private void Update()
        {
            Attack();
        }
        #endregion
    }
}
