using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleSecondPlayerJoin : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> startingPoints;

    private PlayerInputManager playerInputManager;

    public GameObject playerToInstantiate;


    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        InstantiateTwoPlayers();
    }

    private void OnEnable()
    {
        //playerInputManager.onPlayerJoined += AddPlayer;
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
            
        }
        else
        {
            Debug.LogError("Could not find two gamepads to use as controllers.");
        }
    }

    public void AddPlayer(PlayerInput player) {
        players.Add(player);

        Transform playerTransform = player.transform;
        playerTransform.position = startingPoints[players.Count - 1].position;
        playerTransform.rotation = startingPoints[players.Count - 1].rotation;

    }
}
