using System.Collections.Generic;
using Enemy.Behaviour;
using Player;
using UnityEngine;

namespace Detection
{
    public class DetectionSystem : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private Transform _playerOrigin;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _corpseLayer;
        #endregion

        #region Fields
        private List<EnemyBehaviour> _aliveEnemies;
        private List<EnemyBehaviour> _deadEnemies;
        private List<GameObject> _currentSounds;
        #endregion

        #region Setup
        private void SetEnemies(List<EnemyBehaviour> enemies)
        {
            _aliveEnemies = enemies;
        }
        #endregion

        #region Public
        public List<EnemyLogic> DetectEnemies()
        {
            var origin = _playerOrigin.position;
            List<EnemyLogic> detectedEnemies = new();
            
            // Check if there are any enemies without obstructions between them and the player.
            foreach (var enemy in _aliveEnemies)
            {
                var direction = (enemy.Origin.position - _playerOrigin.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit))
                {
                    var enemyHit = hit.transform.GetComponent<EnemyBehaviour>();;
                    // Check if the hit object is the enemy we are currently checking.
                    if (enemyHit.Equals(enemy))
                    {
                        detectedEnemies.Add(enemy.GetComponent<EnemyLogic>());
                    }
                }
            }
            return detectedEnemies;
        }

        public bool DetectPlayer(EnemyBehaviour enemy)
        {
            var target = _playerOrigin.position;

            // Check if there are any enemies without obstructions between them and the player.
            var origin = enemy.Origin.position;
            var direction = (target - origin).normalized;
            if (Physics.Raycast(origin, direction, 99, _playerLayer))
                return true;
            return false;
        }

        public List<EnemyBehaviour> DetectCorpses(Vector3 origin)
        {
            List<EnemyBehaviour> detectedCorpses = new();
            
            // Check if there are any corpses without obstructions between them and the enemy.
            foreach (var corpse in _deadEnemies)
            {
                var targetOrigin = corpse.Origin.position;
                var direction = (origin - targetOrigin).normalized;
                if (Physics.Raycast(targetOrigin, direction, 99, _corpseLayer))
                    detectedCorpses.Add(corpse);
            }
            return detectedCorpses;
        }

        public List<EnemyBehaviour> DetectEnemiesInSoundRange()
        {
            List<EnemyBehaviour> enemies = new();
            
            foreach (var sound in _currentSounds)
            {
                //TODO: check if in range of sound.
                //TODO: destroy sound.
            }
            return enemies;
        }
        
        public bool IsTargetInCone(Transform origin, Vector3 target, float angle)
        {
            return Vector3.Angle(origin.forward, target - origin.position) < angle / 2f;
        }
        #endregion
    }
}
