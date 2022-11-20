using System.Linq;
using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        public float AttackCooldown = 3f;
        public float EffectiveDistance = 0.5f;
        public float Cleavage = 0.5f;
        public float Damage = 10f;
        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];
        private float _currentAttackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private bool _attackIsActive;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            
            if (CanAttack()) 
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(),Cleavage,1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _currentAttackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        public void EnableAttack()
        {
            _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _attackIsActive = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

        private void UpdateCooldown()
        {
            if (!ColldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
            _isAttacking = true;
        }

        private bool CanAttack() => 
            _attackIsActive && !_isAttacking && ColldownIsUp();

        private bool ColldownIsUp() => 
            _currentAttackCooldown <= 0;
    }
}