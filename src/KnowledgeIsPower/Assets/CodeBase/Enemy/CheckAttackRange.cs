using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void OnEnable()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;
            _attack.DisableAttack();
        }

        private void TriggerExit(Collider obj) => 
            _attack.DisableAttack();

        private void TriggerEnter(Collider obj) => 
            _attack.EnableAttack();
    }
}