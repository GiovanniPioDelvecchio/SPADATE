using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG {
    public class PlayerTakesDamage : MonoBehaviour
    {
        
        public AnimationHandler animationHandler;
        public PlayerManager player;

        void Start()
        {
            animationHandler = GetComponent<AnimationHandler>();
            player = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            bool damageFlag = player.isTakingDamage;
            int currentHealth = player.currentHealth;
            float delta = Time.deltaTime;
            HandleDamage(delta, damageFlag, currentHealth);
        }

        public void HandleDamage(float delta, bool damageFlag, int currentHealth)
        {

            //if (animationHandler.anim.GetBool("isinteracting")) return;
            if (damageFlag) {
                if (currentHealth <= 0)
                {
                    animationHandler.PlayTargetAnimation("damage_3", true);
                }
                else
                {
                    animationHandler.PlayTargetAnimation("damage 1", true);
                }
            }         
        }
    }

}

