using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Managers
{
    public class PlayerManager : MonoBehaviour, IDamageable
    {
        #region Unity editor fields
        [SerializeField] private int _maxHealth;
        #endregion
        
        #region Fields
        private int _currentHealth;
        #endregion
        
        #region Properties
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        #endregion

        #region Unity events
        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        #endregion

        #region Setup
        private void Start()
        {
            _currentHealth = _maxHealth;
        }
        #endregion

        #region Public

        public void TakeDamage(int damage)
        {
            if (damage > _currentHealth)
                _onDie.Invoke();
            else if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative");
            else
            {
                _currentHealth -= damage;
                _onHit.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (amount > _maxHealth - _currentHealth)
                _currentHealth = _maxHealth;
            else if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal amount cannot be negative");
            else
            {
                _currentHealth -= amount;
                _onHeal.Invoke();
            }
        }
        #endregion
    }
}
