using Detection;
using Player.Attack;
using Player.PlayerCamera;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Movement;
using UnityEngine.Events;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {

        #region Unity editor fields
        [Header("Player settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Vector2 _cameraSensitivity;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private int _attackDamage;
        #endregion
        
        #region Fields
        private int _maxHealth;
        private int _currentHealth;
        
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private float _cooldownTimer;

        private Transform _transform;
        private PlayerMovement _playerMovement;
        private CameraRotation _cameraRotation;
        private PlayerAttacks _playerAttacks;
        private DetectionSystem _detectionSystem;
        #endregion

        #region Properties
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        #endregion

        #region Unity events
        public UnityEvent OnHit { get; } = new UnityEvent();
        public UnityEvent OnDie { get; } = new UnityEvent();
        public UnityEvent OnHeal { get; } = new UnityEvent();
        #endregion
        
        #region Setup
        private void Awake()
        {
            _transform = transform;
            _playerMovement = GetComponent<PlayerMovement>();
            _cameraRotation = GetComponent<CameraRotation>();
            _playerAttacks = GetComponent<PlayerAttacks>();
            _detectionSystem = GetComponent<DetectionSystem>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
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
            _cameraRotation.Look(rotation);

            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
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
        public void OnAttackInput()
        {
            if (_cooldownTimer <= 0f)
            {
                _playerAttacks.Attack(_attackDamage, _attackRange);
                _cooldownTimer = _attackCooldown;
            }
        }
        public void TakeDamage(float damage)
        {
            
        }
        public void Heal(float amount)
        {
            
        }
        #endregion
    }
}