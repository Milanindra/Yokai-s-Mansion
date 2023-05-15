using UnityEngine;
using Detection;
using System.Collections.Generic;
using Enemy.Behaviour;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private DetectionSystem _detectionSystem;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _enemyDetectionCheckCooldown;
    private float _enemyDetectionCheckTimer = 0;
    private List<EnemyBehaviour> _enemies;

    private void Update()
    {
        if (_enemyDetectionCheckTimer > 0f)
            _enemyDetectionCheckTimer -= Time.deltaTime;
        else
        {
            // Check for all enemies that are not obstructed by anything.
            foreach (var enemy in _enemies)
            {
                if (_detectionSystem.DetectPlayer(enemy))
                {
                    // Check if the player is in sight of the enemy.
                    if (CheckInSight(enemy, _playerTransform.position))
                    {
                        enemy.PlayerLastSeenTime = 0;
                    }
                }
            }
            
            // Check for all enemies that are alive.
            foreach (var enemy in _enemies)
            {
                var bodies = _detectionSystem.DetectCorpses(enemy.Origin.position);
                
                // Check for all corpses that are not obstructed by anything.
                foreach (var body in bodies)
                {
                    // Checl if the corpse is in sight of the enemy.
                    if (CheckInSight(enemy, body.Origin.position))
                    {
                        enemy.FindCorpse(body);
                    }
                }
            }
            
            // Check for all enemies that can hear a sound.
            foreach (var enemy in _detectionSystem.DetectEnemiesInSoundRange())
            {
                enemy.IsHearingSound = true;
            }
            
            //TODO: Check if any enemies can detect a sound.
            _enemyDetectionCheckTimer = _enemyDetectionCheckCooldown;
        }
    }
    
    private bool CheckInSight(EnemyBehaviour enemyBehaviour, Vector3 target)
    {
        // Check if target is in range.
        if (Vector3.Distance(enemyBehaviour.transform.position, target) < enemyBehaviour.ViewDistance)
        {
            // Check if target is in sight.
            if (_detectionSystem.IsTargetInCone(enemyBehaviour.transform, target, enemyBehaviour.ViewAngle))
            {
                return true;
            }
        }

        return false;
    }
}
