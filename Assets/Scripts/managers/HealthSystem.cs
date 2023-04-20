using System;
using UnityEngine;

namespace managers {
    public class HealthSystem : MonoBehaviour {
        public event Action OnDamage;
        public event Action OnDie;
        private bool _initialized;
        
        public void Initialize(int maxHealth) {
            if (_initialized) return;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _initialized = true;
        }

        public int MaxHealth { get; private set; }

        public int CurrentHealth { get; private set; }

        public float CurrentHealthNormalized => (float) CurrentHealth / MaxHealth;
        public bool IsDead => CurrentHealth <= 0;
        
        public bool IsFullHealth => CurrentHealth >= MaxHealth;

        public void Damage(int damageAmount) {
            CurrentHealth -= damageAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            OnDamage?.Invoke();
            
            if (IsDead) {
                OnDie?.Invoke();
            }
            //TODO TRIGGER DESTROY EVENT?
        }
    }
}