using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SG {
    public class HandleSecondPlayerJoin : MonoBehaviour
    {
        private List<PlayerInput> players = new List<PlayerInput>();
        [SerializeField]
        private List<Transform> startingPoints;

        private PlayerInputManager playerInputManager;

        public GameObject playerToInstantiate;

        public MainMenu menu;

        public Color colorPlayer1;
        public Color colorPlayer2;

        public List<Material> materials;

        private PlayerManager p1Stats;
        private PlayerManager p2Stats;

        public float timeOfDeath = -1.0f;
        public float matchFinishedTimer = 5.0f;

        private GameObject feluca1;
        private GameObject feluca2;


        private void Awake()
        {
            menu = GetComponent<MainMenu>();
            playerInputManager = FindObjectOfType<PlayerInputManager>();
            InstantiateTwoPlayers();
        }


        public void Start()
        {
            feluca1.GetComponent<Renderer>().material = materials[menu.getColorP1()];
            feluca2.GetComponent<Renderer>().material = materials[menu.getColorP2()];
        }

        public void InstantiateTwoPlayers()
        {

            InputDevice[] devices = InputSystem.devices.ToArray();

            // Find the first gamepad
            InputDevice gamepad1 = devices[0];//.FirstOrDefault(d => d.deviceClass == typeof(Gamepad));

            // Find the second gamepad
            InputDevice gamepad2 = devices[1];//.Skip(1).FirstOrDefault(d => d.deviceClass == typeof(Gamepad));

            // Check that both gamepads were found
            if (gamepad1 != null && gamepad2 != null)
            {
                // Join the two players with the found gamepads
                GameObject player1 = Instantiate(playerToInstantiate, startingPoints[0].position, startingPoints[0].rotation);
                player1.name = "player 1";
                PlayerInput input1 = playerInputManager.JoinPlayer(-1, -1, "Default", gamepad1);
                GameObject player2 = Instantiate(playerToInstantiate, startingPoints[1].position, startingPoints[1].rotation);
                player2.name = "player 2";
                PlayerInput input2 = playerInputManager.JoinPlayer(0, 0, "Default", gamepad2);

                // Changing the color of the feluca according to the values in the menu
                feluca1 = player1.transform.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head/felucachestabeneintesta").gameObject;
                feluca2 = player2.transform.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head/felucachestabeneintesta").gameObject;

                //feluca1.GetComponent<Renderer>().material = materials[intCp1];
                //feluca2.GetComponent<Renderer>().material = materials[intCp2];

                // setting the private stats objects:
                //p1Stats = player1.GetComponent<PlayerManager>();
                //p2Stats = player2.GetComponent<PlayerManager>();

                //player1.GetComponent<PlayerManager>().menu = menu;
                //player2.GetComponent<PlayerManager>().menu = menu;

            }
            else
            {
                Debug.LogError("Could not find two gamepads to use as controllers.");
            }
        }
    }
}

