using Player.Camera;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Movement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {

        #region Unity editor fields
        
        [Header("Player settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Vector2 _cameraSensitivity;
        #endregion
        
        #region Fields
        
        private Vector2 _moveInput;
        private Vector2 _lookInput;

        private Transform _transform;
        private PlayerMovement _playerMovement;
        private PlayerCamera _playerCamera;

        #endregion
        
        #region Setup

        private void Awake()
        {
            _transform = transform;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCamera = GetComponent<PlayerCamera>();
        }

        #endregion
        
        #region Update

        private void FixedUpdate()
        {
            var movement = _moveInput.x * _transform.right + _moveInput.y * _transform.forward;
            movement = movement.normalized;
            movement *= _movementSpeed;
            _playerMovement.Move(movement);

            var rotation = new Vector2(_lookInput.y, _lookInput.x) * _cameraSensitivity;
            _playerCamera.Look(rotation);
        }

        #endregion

        #region Public

        public void SetMoveInput(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        
        public void SetLookInput(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        #endregion

    }

}
