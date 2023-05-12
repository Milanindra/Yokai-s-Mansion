using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraPositionTransform;
    void Update()
    {
        transform.position = _cameraPositionTransform.position;
    }
}
