﻿using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CharacterController _characterController;
        private Collider[] _hits = new Collider[3];
        private int _layerMask;
        private IInputService _input;
        private float _radius;
        private Stats _stats;

        public void Constructor(IInputService inputService)
        {
            _input = inputService;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if(_input == null)
                return;
            
            if (_input.IsAttackButtonUp() && !_animator.IsAttacking)
                _animator.PlayAttack();
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) =>
            _stats = progress.HeroStats;

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint(), _stats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z) + transform.forward;
    }
}