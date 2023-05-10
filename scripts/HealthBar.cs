using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace SG {
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public PlayerManager playerStats;

        private void Start()
        {
            slider = GetComponent<Slider>();
            SetMaxHealth(playerStats.maxHealth);
        }

        private void OnEnable()
        {
            GameObject secondPlayer = GameObject.Find("player_holder(Clone)");
            if (this.gameObject.transform.parent.transform.parent.gameObject.Equals(secondPlayer))
            {
                RectTransform thisTransform = this.GetComponent<RectTransform>();
                thisTransform.localPosition = new Vector2(131, 210);
            }
        }

        public void SetMaxHealth(int maxHealth) {

            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            
        }

        public void SetCurrentHealth(int currentHealth) {
            slider.value = currentHealth;
        }
    }
}

