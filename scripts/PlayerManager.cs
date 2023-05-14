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

        public BoxCollider bodyCollider;
        public Rigidbody rigid;

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

        public bool isTakingDamage = false;

        public GameObject menuObject;
        public MainMenu menu;

        public float deathTime = -1.0f;

        public float endGameTimer = 3.0f;

        public void getMainMenuObject() {
            menuObject = GameObject.Find("PlayerInputManagerHolder");
            menu = menuObject.GetComponent<MainMenu>();
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandlerModified>();
            anim = GetComponentInChildren<Animator>();
            animationH = GetComponent<AnimationHandler>();
            rigid = GetComponent<Rigidbody>();
            maxHealth = SetMaxHealthFromHealthPoints();
            healthBar.SetMaxHealth(maxHealth);
            currentHealth = maxHealth;
            damage = damageObject.damage;
            getMainMenuObject();
        }

        // Update is called once per frame
        void Update()
        {
            inputHandler.isInteracting = anim.GetBool("isinteracting");
            inputHandler.attackAfterRollFlag = anim.GetBool("rollAttack");
            inputHandler.rollFlag = false;
            isTakingDamage = false;

            if (menuObject == null) {
                getMainMenuObject();
            }
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY, inputHandler.cameraLockFlag);
            }
            if (deathTime > 0 && Time.time - deathTime >= endGameTimer) {
                menu.goToFirst();
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
            isTakingDamage = true;


            if (currentHealth <= 0) {

                
                currentHealth = 0;
                healthBar.SetCurrentHealth(currentHealth);
                deathTime = Time.time;
            } else {
                //if (anim.GetBool("isinteracting")) return;

                //animationH.PlayTargetAnimation("damage_1", true);
                
            }
        }

        public void setTo1HP() {
            currentHealth = (int)(currentHealth * 20f / 100f);
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
