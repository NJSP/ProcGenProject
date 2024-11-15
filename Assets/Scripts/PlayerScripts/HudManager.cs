using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerScripts
{
    public class HudManager : MonoBehaviour
    {
        // UI Text Components
        public GameObject _valueText;
        
        // Game Variables
        public Player _player;

        // Text Components
        TextMeshProUGUI valueText;

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            valueText = _valueText.GetComponent<TextMeshProUGUI>();
            if (_player == null)
            {
                Debug.LogError("Player not found in the scene.");
            }
            if (_valueText == null)
            {
                Debug.LogError("Text component not found in the scene.");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHud()
        {
            valueText.text = "Loot Value: " + _player.GetComponent<InventorySystem>.totalValue;
        }
    }
}