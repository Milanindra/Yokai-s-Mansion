using System;
using System.Collections.Generic;
using Enemy.Movement;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Enemy.Behaviour
{
    public class EnemyBehaviour : MonoBehaviour, IDamageable
    {
        #region Unity editor fields
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private int _maxHealth;
        #endregion
        
        
        #region Fields
        private int _currentHealth;
        private int[][] RoomConnections = new int[7][];
        private EnemyMovement _enemyMovement;

        List<int> _roomHistory = new();
        private int _currentRoom = 0;
        private int _characterID = 0;
        private int _roundCount = 0;
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
        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            
            RoomConnections[0] = new int[] {1, 2, 3};
            RoomConnections[1] = new int[] {0, 4};
            RoomConnections[2] = new int[] {0};
            RoomConnections[3] = new int[] {0, 6};
            RoomConnections[4] = new int[] {1, 5};
            RoomConnections[5] = new int[] {4, 6};
            RoomConnections[6] = new int[] {3, 5};
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        #endregion

        #region Public
        public void Wander()
        {
            var nextRoom = GetNextRoom();
            _roomHistory.Add(nextRoom);
            if (_roomHistory.Count > 4)
                _roomHistory.RemoveAt(0);
            _enemyMovement.WalkToPoint(_waypoints[nextRoom].position);
            _currentRoom = nextRoom;
        }
        
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
        
        #region Private
        private int GetNextRoom()
        {
            Random random = new Random(GetSeed());
            
            var nextRoom = RoomConnections[_currentRoom][random.Next(RoomConnections[_currentRoom].Length)];
            if (_roomHistory.Contains(nextRoom))
                nextRoom = RoomConnections[_currentRoom][random.Next(RoomConnections[_currentRoom].Length)];
            
            return nextRoom;
        }

        private int GetSeed()
        {
            var seed = 3891604;
            seed ^= _characterID;
            seed ^= _roundCount;
            foreach (var roomID in _roomHistory)
            {
                seed ^= roomID;
            }

            return seed;
        }
        #endregion

        
    }
}
