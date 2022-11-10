using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroMover _move;
        [SerializeField] private HeroAttack _attack;
        [SerializeField] private GameObject _deathFx;
        private bool _isDead;

        private void Start() =>
            _heroHealth.HealthChanged += OnHealthChanged;

        private void OnDestroy() =>
            _heroHealth.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            if (!_isDead && _heroHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;
            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(_deathFx, transform.position, Quaternion.identity);
        }
    }
}