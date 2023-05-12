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
        bool healthbarFixedFlag1 = false;
        bool healthbarFixedFlag2 = false;

        private void Start()
        {
            slider = GetComponent<Slider>();
            SetMaxHealth(playerStats.maxHealth);
        }


        private void Update()
        {
            fixHealthBar();
        }

        private void OnEnable()
        {
            //GameObject secondPlayer = GameObject.Find("player 2");
            if (this.gameObject.transform.parent.transform.parent.gameObject.name == "player 2")
            {
                RectTransform thisTransform = this.GetComponent<RectTransform>();
                thisTransform.localPosition = new Vector2(131, 210);
            }
        }

        void fixHealthBar() {
            RectTransform canvasTransform = this.transform.parent.GetComponent<RectTransform>();
            float maxX = canvasTransform.rect.width;
            float maxY = canvasTransform.rect.height;
            
    

            if (!healthbarFixedFlag1 && this.gameObject.transform.parent.transform.parent.gameObject.name == "player 1")
            {
                RectTransform thisTransform = this.GetComponent<RectTransform>();

                //thisTransform.localPosition = new Vector2(maxX, -maxY);
                //Debug.Log("changing healthbar pos");
                //Debug.Log("max X:" + maxX);
                //float xpos = (maxX * (3.0 / 100));
                //Debug.Log("trying to set as x:" + xpos);
                thisTransform.anchoredPosition = new Vector2(maxX * (3.0f/ 100.0f), -maxY * (10.0f / 100.0f));
                //healthbarFixedFlag1 = true;
            }

            if (!healthbarFixedFlag2 && this.gameObject.transform.parent.transform.parent.gameObject.name == "player 2")
            {
                RectTransform thisTransform = this.GetComponent<RectTransform>();
                thisTransform.anchoredPosition = new Vector2(maxX - (maxX * (3.0f / 100.0f) + thisTransform.rect.width), -maxY * (10.0f / 100.0f));
                //healthbarFixedFlag2 = true;
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

