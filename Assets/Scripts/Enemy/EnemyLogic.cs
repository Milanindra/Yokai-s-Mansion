using System;
using UnityEngine;
using Interfaces;
using UnityEngine.Events;
using Enemy.Behaviour;

[RequireComponent(typeof(EnemyBehaviour))]
public class EnemyLogic : MonoBehaviour, IDamageable
{
    #region Unity editor fields
    [SerializeField] private int _maxHealth;
    #endregion
    
    #region Fields
    private EnemyBehaviour _enemyBehaviour;
    
    private int _currentHealth;
    #endregion
    
    #region  Properties
    
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    #endregion
    
    #region Unity events
    [SerializeField] private UnityEvent _onHit;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;
    #endregion
    
    #region Setup
    private void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }
    
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
            if (_currentHealth < _maxHealth * .3f)
                _enemyBehaviour.IsLowHealth = true;
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
            if (_currentHealth > _maxHealth * .3f)
                _enemyBehaviour.IsLowHealth = false;
            _onHeal.Invoke();
        }
    }
    #endregion
}
