using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private AgentMoveToHero _agentMoveToHero;
        
        [SerializeField] private GameObject _deathFx;
        public event Action Happened;
          
        private void Start() =>
            _enemyHealth.HealthChanged += OnHealthChanged;

        private void OnDestroy() =>
            _enemyHealth.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            if (_enemyHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            _enemyHealth.HealthChanged -= OnHealthChanged;
            _animator.PlayDeath();
            _agentMoveToHero.enabled = false;
            SpawnDeathFx();
            StartCoroutine(DestroyTimer(3));
            Happened?.Invoke();
        }

        private void SpawnDeathFx() => 
            Instantiate(_deathFx, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}