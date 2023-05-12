using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class DetectionSystem : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private Transform _playerOrigin;
        [SerializeField] private LayerMask _nonPlayerLayerMask;
        #endregion

        #region Fields
        private List<GameObject> _aliveEnemies;
        #endregion

        #region Setup
        private void SetEnemies(List<GameObject> enemies)
        {
            _aliveEnemies = enemies;
        }
        #endregion

        #region Public
        public List<GameObject> DetectEnemiesInRange(float range)
        {
            var origin = _playerOrigin.position;
            List<GameObject> detectedEnemies = new();
            
            for (int i = 0; i < _aliveEnemies.Count; i++)
            {
                var direction = _aliveEnemies[i].transform.position - _playerOrigin.position.normalized;
                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, range, _nonPlayerLayerMask))
                {
                    if (hit.transform.gameObject == _aliveEnemies[i])
                    {
                        detectedEnemies.Add(_aliveEnemies[i]);        
                    }
                }
            }
            return detectedEnemies;
        }

        public bool IsTargetInCone(Transform origin, Vector3 target, float angle)
        {
            return Vector3.Angle(origin.forward, target - origin.position) < angle / 2f;
        }
        #endregion
    }
}
