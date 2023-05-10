using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG {

    public class AnimationHandler : MonoBehaviour
    {
        // this attribute is the actual Object needed to play animations
        public Animator anim;

        // this attribute is needed to check wether the Player is interacting or
        // not
        public InputHandlerModified inputHandler;

        // this attribute is needed to get the rigidbody of the Player
        public PlayerLocomotion playerLocomotion;

        // these attributes are needed to save the vertical and horizontal
        // movement values
        int vertical;
        int horizontal;

        // if this attribute is set to false, the Player will not rotate
        public bool canRotate = true;

        public void Initialize() {
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandlerModified>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement) {
            #region Vertical
            float v = 0;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {

                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {

                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion



            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting) {

            anim.applyRootMotion = isInteracting;
            anim.SetBool("isinteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }


        public void CanRotate() {
            canRotate = true;
        }

        public void StopRotation()  {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (inputHandler.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }

}

