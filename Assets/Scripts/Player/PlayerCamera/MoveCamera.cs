using UnityEngine;

namespace Player.PlayerCamera
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPositionTransform;
        void Update()
        {
            transform.position = _cameraPositionTransform.position;
        }
    }
}
