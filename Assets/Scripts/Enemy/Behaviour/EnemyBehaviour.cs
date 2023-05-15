using System.Collections.Generic;
using Enemy.Movement;
using UnityEngine;
using UnityEngine.Events;
using Detection;
using Random = System.Random;

namespace Enemy.Behaviour
{
    public class EnemyBehaviour : MonoBehaviour
    {
        #region Unity editor fields
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private Transform _origin;

        [SerializeField] private float _memoryTime;
        #endregion
        
        #region Fields
        private EnemyMovement _enemyMovement;

        private int _characterID = 0;
        private int[][] RoomConnections = new int[7][];
        
        List<int> _roomHistory = new();
        private int _currentRoom = 0;
        private int _roundCount = 0;
        private int _enemyCount;
        private List<int> _seenBodies = new();
        private bool _seenPlayer = false;
        
        private bool _isLowHealth = false;
        private bool _isBeingScared = false;
        private bool _isHearingSound = false;
        private bool _isPlayerInSight = false;
        private float _playerLastSeenTime = 0;
        
        private float _viewAngle = 60;
        private float _viewDistance = 10;
        #endregion
        
        #region Properties
        public Transform Origin => _origin;
        
        public bool IsLowHealth { set => _isBeingScared = value; }
        public bool IsBeingScared { set => _isBeingScared = value; }
        public bool IsHearingSound { set => _isHearingSound = value; }
        public bool IsPlayerInSight { set => _isPlayerInSight = value; }
        public float PlayerLastSeenTime { set => _playerLastSeenTime = value; }
        
        public float ViewAngle => _viewAngle;
        public float ViewDistance => _viewDistance;
        #endregion

        #region Unity events
        [SerializeField] private UnityEvent _onHurtingDetectPlayer;
        [SerializeField] private UnityEvent _onHurting;
        [SerializeField] private UnityEvent _onScare;
        [SerializeField] private UnityEvent _onEncounter;
        [SerializeField] private UnityEvent _onFirstEncounter;
        [SerializeField] private UnityEvent _onDetectPlayer;
        [SerializeField] private UnityEvent _onDetectLoudSound;
        [SerializeField] private UnityEvent _onDefault;
        #endregion

        #region Setup
        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();

            RoomConnections[0] = new int[] {1, 2, 3};
            RoomConnections[1] = new int[] {0, 4};
            RoomConnections[2] = new int[] {0};
            RoomConnections[3] = new int[] {0, 6};
            RoomConnections[4] = new int[] {1, 5};
            RoomConnections[5] = new int[] {4, 6};
            RoomConnections[6] = new int[] {3, 5};
        }
        #endregion

        #region Update
        private void Update()
        {
            if (_isLowHealth || _seenBodies.Count > _enemyCount - 2)
            {
                if (_playerLastSeenTime < _memoryTime)
                    _onHurtingDetectPlayer.Invoke();
                else
                    _onHurting.Invoke();
            }
            else if (_isBeingScared)
            {
                _onScare.Invoke();
                _isBeingScared = false;
            }
            else if (_isPlayerInSight)
            {
                if (_playerLastSeenTime < _memoryTime)
                    _onDetectPlayer.Invoke();
                else if (_seenPlayer)
                    _onEncounter.Invoke();
                else
                    _onFirstEncounter.Invoke();
            }
            else if (_isHearingSound)
            {
                _onDetectLoudSound.Invoke();
                _isHearingSound = false;
            }
            else
            {
                _onDefault.Invoke();
            }
        }
        #endregion
            
        #region Public
        public void Wander()
        {
            var nextRoom = GetNextRoom();
            _roomHistory.Add(nextRoom);
            if (_roomHistory.Count > 4)
                _roomHistory.RemoveAt(0);
            _enemyMovement.WalkToPoint(_waypoints[nextRoom].position);
            _currentRoom = nextRoom;
        }

        public void FindCorpse(EnemyBehaviour corpse)
        {
            if (!_seenBodies.Contains(corpse.GetInstanceID()))
            {
                _seenBodies.Add(corpse.GetInstanceID());
                _isBeingScared = true;
            }
        }
        #endregion
        
        #region Private
        private int GetNextRoom()
        {
            Random random = new Random(GetSeed());
            
            var nextRoom = RoomConnections[_currentRoom][random.Next(RoomConnections[_currentRoom].Length)];
            if (_roomHistory.Contains(nextRoom))
                nextRoom = RoomConnections[_currentRoom][random.Next(RoomConnections[_currentRoom].Length)];
            
            return nextRoom;
        }

        private int GetSeed()
        {
            var seed = 3891604;
            seed ^= _characterID;
            seed ^= _roundCount;
            foreach (var roomID in _roomHistory)
            {
                seed ^= roomID;
            }

            return seed;
        }
        #endregion
    }
}
