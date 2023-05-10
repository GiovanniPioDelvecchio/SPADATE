using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SG
{
    public class InputHandlerModified : MonoBehaviour
    {
        /*
         * This class is needed to handle the input obtained from the 
         * Input Action asset. In that asset, keybindings are defined
         * and abstracted to the corresponding action, wich is handled 
         * here.
         */

        // these attributes are needed for the movement of the Player in 
        // the 2D plane where it stands
        public float horizontal;
        public float vertical;
        public float moveAmount;

        // these attributes are needed for the handle of the camera 
        public float mouseX;
        public float mouseY;
        public bool cameraLockFlag = true;
        private bool cameraLockTriggered = false;

        // these attributes are needed to handle the roll of the Player
        public bool b_Input;
        public bool rollFlag;

        // this attribute is needed to do not allow the Player to perform
        // an animation before the end of the previous one 
        public bool isInteracting;
        
        // this attribute is needed to handle the attack action as
        // the consequence of a trigger, in order to have a better
        // coherence with the frames of the game
        public bool attackTrigger = false;

        // this attributes is used to recognize if 
        // an attack has been performed right after a roll
        public bool attackAfterRollFlag = false;

        // this attribute is needed to specify
        // the kind of attack that has to be performed
        public int attackIndex = 0;

        public int lastAttackIndex = 0;         //probably useless, this specifies the last attack

        // these attributes are needed to specify the timings
        // between consecutive attacks
        public float lastAttackTime = 0f;
        public float secondAttackTimer = 0.3f;

        // these attributes are needed to specify the timings
        // between a roll and the following attack
        public float lastRollTime = 0f;
        public float attackAfterRollTimer = 0.3f;
        public bool attackAfterRollPerformed = false;

        // this attribute is needed to handle the uccellagione action
        public bool uccellagioneFlag = false;

        //PlayerControls inputActions;
        private InputActionAsset inputAsset;    // the input action asset
        private InputActionMap movementMap;     
        private InputActionMap actionsMap;

        private InputAction movementAction;
        private InputAction cameraAction;

        // these attributes are what we use to acutally handle the input
        // but they must be derived from the action maps
        private InputAction cameraLockAction;
        private InputAction rollAction;
        private InputAction attackAction;
        private InputAction uccellagioneAction;

        private Animator anim;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            // the first thing that we need to do is to get the input asset
            // and the corresponding maps
            inputAsset = this.GetComponent<PlayerInput>().actions;
            anim = GetComponent<Animator>();
            movementMap = inputAsset.FindActionMap("Player Movement");
            actionsMap = inputAsset.FindActionMap("Player Actions");
        }

        public void OnEnable()
        {
            // here we give value to the actions from the maps
            movementMap.FindAction("Movement").performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            movementMap.FindAction("Camera").performed += i => cameraInput = i.ReadValue<Vector2>();
            movementAction = movementMap.FindAction("Movement");
            cameraAction = movementMap.FindAction("Camera");

            cameraLockAction = actionsMap.FindAction("Camera Lock");
            rollAction = actionsMap.FindAction("Roll");
            attackAction = actionsMap.FindAction("Attack");
            uccellagioneAction = actionsMap.FindAction("Uccellagione");

            movementMap.Enable();
            actionsMap.Enable();
        }
        

        public void TickInput(float delta)
        {
            HandleCameraLock(delta);
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleUccellagione(delta);
        }

        private void OnDisable()
        {
            // here we disable the maps
            movementMap.Disable();
            actionsMap.Disable();
        }

        private void HandleRollInput(float delta)
        {
            // Different kinds of inputs have different kind of phases, buttons, for instance, 
            // have the following phases:
            // -Started, when pressed (although it seems not to be working here)
            // -Performed, after you press the button
            // -Waiting, when you are not pressing anything
            b_Input = rollAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                rollFlag = true;
                lastRollTime = Time.time;
            }
        }

        public void HandleCameraLock(float delta)
        {
            if (cameraLockAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed && !cameraLockTriggered)
            {
                cameraLockTriggered = true;
                cameraLockFlag = !cameraLockFlag;
            }
            else if (cameraLockAction.phase == UnityEngine.InputSystem.InputActionPhase.Waiting)
            {
                cameraLockTriggered = false;
            }
        }

        public void HandleUccellagione(float delta) {
            if (uccellagioneAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed && !attackTrigger)
            {
                attackTrigger = true;
                uccellagioneFlag = true;
            }
            else if (uccellagioneAction.phase == UnityEngine.InputSystem.InputActionPhase.Waiting)
            {
                attackTrigger = false;
                uccellagioneFlag = false;

            }
        }

        public void HandleAttackInput(float delta)
        {
            if (attackAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed && !attackTrigger)
            {
                attackTrigger = true;
                if (Time.time - lastRollTime <= attackAfterRollTimer) 
                {
                    anim.SetBool("rollAttack", true);
                }
                else if ((Time.time - lastAttackTime <= secondAttackTimer) && lastAttackIndex == 1)
                {
                    attackIndex = 2;
                }
                else
                {
                    attackIndex = 1;
                }
                lastAttackTime = Time.time;
            }
            else if (attackAction.phase == UnityEngine.InputSystem.InputActionPhase.Waiting)
            {
                attackTrigger = false;
                attackIndex = 0;
            }
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    }
}

