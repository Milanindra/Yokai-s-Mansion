using UnityEngine;

namespace Player.PlayerCamera
{
    public class CameraRotation : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private Transform _playerRotationTransform;
        [SerializeField] private Transform _playerCameraTransform;
        #endregion

        #region Fields
        private Vector2 _playerRotation;
        
        private const int YAngleLimit = 90;
        #endregion
        
        #region Setup
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion

        #region Public
        public void Look(Vector2 rotation)
        {
            _playerRotation += rotation;
            
            _playerRotation.x = Mathf.Clamp(_playerRotation.x, -YAngleLimit, YAngleLimit);
            
            _playerCameraTransform.rotation = Quaternion.Euler(-_playerRotation.x, _playerRotation.y, 0);
            _playerRotationTransform.rotation = Quaternion.Euler(0, _playerRotation.y, 0);
        }
        #endregion
    }
}
