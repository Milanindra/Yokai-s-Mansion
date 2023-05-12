using System;
using Enemy.Behaviour;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private float _speed;
        #endregion

        #region Fields
        private EnemyBehaviour _enemyBehaviour;
        private NavMeshAgent _navMeshAgent;
        #endregion

        #region Setup
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
        }
        
        private void Start()
        {
            _navMeshAgent.speed = _speed;
            _navMeshAgent.destination = transform.position;
        }
        #endregion

        private void Update()
        {
            if (Vector3.Distance(transform.position, _navMeshAgent.destination) < 0.1f)
            {
                _enemyBehaviour.Wander();
            }
        }

        #region Public
        public void WalkToPoint(Vector3 point)
        {
            _navMeshAgent.SetDestination(point);
        }
        #endregion
    }
}
