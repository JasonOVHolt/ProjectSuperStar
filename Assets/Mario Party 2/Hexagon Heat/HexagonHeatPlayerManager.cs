using UnityEngine;
using UnityEngine.InputSystem;

public class HexagonHeatPlayerManager : MonoBehaviour
{
    [SerializeField] Transform[] menuSpawnPoints;
    [SerializeField] Transform players;
    private int numPlayers;
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Keep your spawning logic
        
        if (players != null)
        {
            playerInput.transform.parent = players;
            // Simplified naming logic
            playerInput.transform.gameObject.name = "P" + (players.GetSiblingIndex());
        }
        playerInput.GetComponent<Rigidbody>().position = menuSpawnPoints[numPlayers].position;
        //no longer allow people to join the game when hitting the limit
        if (numPlayers > 3)
        {
            GetComponent<PlayerInputManager>().DisableJoining();
        }
        numPlayers++;   //add one to varible to keep track of number of players

    }
}
