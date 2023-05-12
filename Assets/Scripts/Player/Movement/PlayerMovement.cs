using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {

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
            _rigidbody.AddForce(input, ForceMode.Force);
        }

        #endregion
    }
}

