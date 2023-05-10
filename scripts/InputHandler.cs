using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG {
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool rollFlag;
        public bool isInteracting;
        public bool cameraLockFlag = true;
        public bool attackTrigger = false;
        public bool attackAfterRollFlag = false;

        public int attackIndex = 0;
        public int lastAttackIndex = 0;

        public float lastAttackTime = 0f;
        public float secondAttackTimer = 0.3f;

        public float lastRollTime = 0f;
        public float attackAfterRollTimer = 0.3f;


        private bool cameraLockTriggered = false;

        PlayerControls inputActions;
        public CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            //cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null) {
                
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY, cameraLockFlag);
                
            }
        }

        public void OnEnable()
        {
            if (inputActions == null) {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void HandleCameraLock(float delta)
        {
            if (inputActions.PlayerActions.CameraLock.phase == UnityEngine.InputSystem.InputActionPhase.Performed && !cameraLockTriggered)
            {
                cameraLockTriggered = true;
                cameraLockFlag = !cameraLockFlag;
            }
            else if (inputActions.PlayerActions.CameraLock.phase == UnityEngine.InputSystem.InputActionPhase.Waiting)
            {
                cameraLockTriggered = false;
            }
        }

        public void TickInput(float delta) {
            HandleCameraLock(delta);
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
        }

        public void HandleAttackInput(float delta)
        {
            if (inputActions.PlayerActions.Attack.phase == UnityEngine.InputSystem.InputActionPhase.Performed && !attackTrigger)
            {
                attackTrigger = true;
                if (Time.time - lastRollTime <= attackAfterRollTimer ) {
                    attackAfterRollFlag = true;
                }
                else if ((Time.time - lastAttackTime <= secondAttackTimer) && lastAttackIndex == 1)
                {
                    attackIndex = 2;
                }
                else if (attackIndex != 3) {
                    attackIndex = 1;
                }
                lastAttackTime = Time.time;
            }
            else if (inputActions.PlayerActions.Attack.phase == UnityEngine.InputSystem.InputActionPhase.Waiting)
            {
                attackTrigger = false;
                attackIndex = 0; 
                
            }

        }

        private void MoveInput(float delta) {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta) {

            // Different kinds of inputs have different kind of phases, buttons, for instance, 
            // have the following phases:
            // -Started, when pressed (although it seems not to be working here)
            // -Performed, after you press the button
            // -Waiting, when you are not pressing anything
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input) {
                rollFlag = true;
                lastRollTime = Time.time;
            }
        }
    }
}

