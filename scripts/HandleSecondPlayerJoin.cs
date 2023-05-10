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


    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    public void AddPlayer(PlayerInput player) {
        players.Add(player);

        Transform playerTransform = player.transform;
        playerTransform.position = startingPoints[players.Count - 1].position;
        playerTransform.rotation = startingPoints[players.Count - 1].rotation;

    }
}
