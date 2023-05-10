using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG {
    public class PlayerLocomotion : MonoBehaviour
    {

        Transform cameraObject;

        // this attribute is needed because certain animations
        // must be performed depending on some flags
        InputHandlerModified inputHandler;

        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;

        [HideInInspector]
        public AnimationHandler animationHandler;

        public new Rigidbody rigidbody;
        public GameObject Camera;

        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandlerModified>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
            cameraObject = Camera.GetComponent<Transform>();
            myTransform = transform;
            animationHandler.Initialize();
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;
        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = rotationSpeed;

            // the movement direction is a transform depending 
            // on our input and the current position of the camera,
            // so to get the rotation we just need to consider the 
            // quaternion obtained by looking in the direction of that
            // transform
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
            

            myTransform.rotation = targetRotation;
            targetDir.y = 0;
        }

        #endregion

        public void HandleMovement(float delta) {

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;

            normalVector = Vector3.up;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            // just changing the velocity here, a decaying acceleration should be more appropriate

            rigidbody.velocity = projectedVelocity;

            animationHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animationHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        // here there was the update 
        private void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRolling(delta);
        }

        // in the tutorial this method is called HandleRollingAndSprinting,
        // however here we just need to handle the rolls, since we don't need
        // sprinting in this game
        public void HandleRolling(float delta) {

         
            if (animationHandler.anim.GetBool("isinteracting")) return;

            if (inputHandler.rollFlag) {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    
                    animationHandler.PlayTargetAnimation("roll_forward", true);

                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else {
                    
                    animationHandler.PlayTargetAnimation("step_back", true);
                }
            }
        }
    }
}

