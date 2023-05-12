using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Unity editor fields

        [SerializeField] private Transform _playerRotationTransform;

        #endregion
        
        #region Fields

        private Rigidbody _rigidbody;

        #endregion
        
        #region Setup

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion

        #region Public

        public void Move(Vector3 input)
        {
            var direction = input.z * _playerRotationTransform.forward + input.x * _playerRotationTransform.right;
            _rigidbody.AddForce(direction, ForceMode.Force);
        }

        #endregion
    }
}

