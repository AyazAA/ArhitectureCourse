﻿using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        public BoxCollider Collider;
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Constructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            Debug.Log("Progress saved");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 120);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}