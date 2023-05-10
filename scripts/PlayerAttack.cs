using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG {
    public class PlayerAttack : MonoBehaviour
    {
        private InputHandlerModified inputHandler;
        private AnimationHandler animationHandler;

        void Start()
        {
            inputHandler = GetComponent<InputHandlerModified>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            HandleAttacking(delta);
        }

        public void HandleAttacking(float delta)
        {

            if (animationHandler.anim.GetBool("isinteracting")) return;

            if (inputHandler.uccellagioneFlag == true)
            {
                animationHandler.PlayTargetAnimation("rig|uccellagione", true);
            }


            switch (inputHandler.attackIndex)
            {
                case 0:
                    return;
                case 1:
                    animationHandler.PlayTargetAnimation("oh_attack_1", true);
                    inputHandler.lastAttackIndex = 1;
                    break;
                case 2:
                    animationHandler.PlayTargetAnimation("oh_attack_2", true);
                    inputHandler.lastAttackIndex = 2;
                    break;
            }
        }
    }
}

