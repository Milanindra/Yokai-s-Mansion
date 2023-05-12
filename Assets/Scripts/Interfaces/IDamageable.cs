using Player.Managers;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IDamageable
    { 
        int MaxHealth { get; }
        int CurrentHealth { get; }

        void TakeDamage(int damage);
        void Heal(int amount);
    }
}
