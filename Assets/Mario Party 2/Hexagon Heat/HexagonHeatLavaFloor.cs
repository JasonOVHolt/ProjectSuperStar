using UnityEngine;

public class HexagonHeatLavaFloor : MonoBehaviour
{
    [SerializeField] HexagonHeatGameManager gameManager;
    [SerializeField] float lat;
    [SerializeField] Vector2 height;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            
            //HexagonHeatPlayerMovement playerMovement = other.gameObject.GetComponent<HexagonHeatPlayerMovement();
            //gameManager.PlayerDeath(playerMovement.playerIndex);
            //playerMovement.enabled = false;
            other.gameObject.GetComponent<Rigidbody>().AddForce(RandomizeLaunch(), ForceMode.Impulse);
            
        }
    }

    private Vector3 RandomizeLaunch()
    {
        float X = Random.Range(-lat, lat);
        float Y = Random.Range(-lat, lat);
        float Z = Random.Range(height.x, height.y);
        return new Vector3(X, Z, Y);
    }
}
