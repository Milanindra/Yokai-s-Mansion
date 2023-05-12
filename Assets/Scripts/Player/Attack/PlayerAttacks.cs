using Detection;
using UnityEngine;

namespace Player.Attack
{
    [RequireComponent(typeof(DetectionSystem))]
    public class PlayerAttacks : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private Transform _playerOrigin;
        #endregion
        
        #region Fields
        private DetectionSystem _detectionSystem;
        private float _viewAngle = 60f;
        #endregion

        #region Setup
        private void Awake()
        {
            _detectionSystem = GetComponent<DetectionSystem>();
            var mainCamera = Camera.main;
            if (mainCamera != null)
                _viewAngle = mainCamera.fieldOfView;
            else
                Debug.LogError(this + " was unable to locate the main camera.");
        }
        #endregion

        #region Public
        public void Attack(int damage, float range)
        {
            var targets = _detectionSystem.DetectEnemiesInRange(range);
            if (targets.Count > 0)
            {
                foreach (var enemy in targets)
                {
                    if (_detectionSystem.IsTargetInCone(_playerOrigin, enemy.transform.position, _viewAngle))
                    {
                        if (_detectionSystem.IsTargetInCone(enemy.transform, _playerOrigin.position, _viewAngle))
                        {
                            //TODO: Play attack animation
                            //TODO: Play attack sound
                            Debug.Log("Hit enemy: " + enemy.name);
                            enemy.TakeDamage(damage);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
