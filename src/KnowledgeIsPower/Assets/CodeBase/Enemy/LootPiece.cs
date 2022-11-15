using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject Skull; 
        [SerializeField] private GameObject PickupFx; 
        [SerializeField] private TextMeshPro LootText; 
        [SerializeField] private GameObject PickupPopup; 
        
        private Loot _loot;
        private WorldData _worldData;
        private bool _picked;
        private float _destroyDelay = 1.5f;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }
        
        private void OnTriggerEnter(Collider other) => PickUp();

        private void PickUp()
        {
            if(_picked)
                return;
            _picked = true;

            UpdateWorldData();
            HideSkull();
            PlayPickupFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => 
            Skull.SetActive(false);

        private void PlayPickupFx() => 
            Instantiate(PickupFx, transform.position, Quaternion.identity);

        private void ShowText()
        {
            LootText.text = _worldData.LootData.Collected.ToString();
            PickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }
    }
}