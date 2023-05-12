using UnityEngine;
using UnityEngine.InputSystem;
using Player.Movement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {

        #region Unity editor fields
        
        [SerializeField] private float _movementSpeed;

        #endregion
        
        #region Fields
        
        private Vector2 _moveInput;
        private PlayerMovement _playerMovement;
        
        #endregion
        
        #region Setup

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        #endregion
        
        #region Update

        private void FixedUpdate()
        {
            var movement = _moveInput.x * transform.right + _moveInput.y * transform.forward;
            movement = movement.normalized;
            movement *= _movementSpeed;
            
            _playerMovement.Move(movement);
        }

        #endregion

        #region Public

        public void SetMoveInput(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        #endregion
        
    }

}
