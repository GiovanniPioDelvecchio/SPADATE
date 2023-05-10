using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace SG {
    public class PlayerManager : MonoBehaviour
    {
        // this attribute is needed to handle input
        InputHandlerModified inputHandler;

        // this attributes are needed to handle animations
        Animator anim;
        private AnimationHandler animationH;

        // these attributes represent the additional colliders of the player
        public BoxCollider swordCollider;
        public BoxCollider leftHandCollider;
        public BoxCollider hatCollider;

        public CapsuleCollider bodyCollider;

        // these attributes are the stats of the player
        public int healthPoints = 10;
        public int maxHealth = 1000;
        public int currentHealth;

        // this attribute is a damage object, it is needed to fix the amount
        // of damage to be dealt with the sword
        public DamagePlayer damageObject;

        // this attribute is the actual damage to be dealt either
        // with the sword or by the uccellagione 
        public int damage;

        // this will be set to true if the uccellagione has been performed
        public bool uccellagioneFlag = false;

        public HealthBar healthBar;

        public CameraHandler cameraHandler;

        void Start()
        {
            inputHandler = GetComponent<InputHandlerModified>();
            anim = GetComponentInChildren<Animator>();
            animationH = GetComponent<AnimationHandler>();
            maxHealth = SetMaxHealthFromHealthPoints();
            healthBar.SetMaxHealth(maxHealth);
            currentHealth = maxHealth;
            damage = damageObject.damage;
        }

        // Update is called once per frame
        void Update()
        {
            inputHandler.isInteracting = anim.GetBool("isinteracting");
            inputHandler.attackAfterRollFlag = anim.GetBool("rollAttack");
            inputHandler.rollFlag = false;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY, inputHandler.cameraLockFlag);
            }
        }

        private int SetMaxHealthFromHealthPoints()
        {
            maxHealth = healthPoints * 10;
            return maxHealth;
        }

        public void SetDamage(int to_deal) {
            damage = to_deal;
        }

        public void TakeDamage(int damage) {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);
            
            if (currentHealth <= 0) {

                animationH.PlayTargetAnimation("damage_3", true);
                currentHealth = maxHealth;
                healthBar.SetCurrentHealth(currentHealth);
            } else {
                animationH.PlayTargetAnimation("damage_1", true);
                
            }
        }

        public void setTo1HP() {
            currentHealth = 1;
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void EnableSwordCollider() {
            swordCollider.enabled = true;
        }

        public void DisableSwordCollider() {
            swordCollider.enabled = false;
        }

        public void EnableUccellagioneCollider() {
            leftHandCollider.enabled = true;
        }

        public void DisableUccellagioneCollider() {
            leftHandCollider.enabled = false;
        }

        public void EnableBodyCollider()
        {
            bodyCollider.enabled = true;
        }

        public void DisableBodyCollider()
        {
            bodyCollider.enabled = false;
        }
    }

}