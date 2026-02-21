using UnityEngine;

public class HotRopeJumpFireball : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] float speed;
    private HotRopeJumpGameManager gameManager;
    [SerializeField] float lat;
    [SerializeField] Vector2 height;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<HotRopeJumpGameManager>();
    }
    private void Update()
    {
        transform.RotateAround(targetPoint.position, Vector3.left, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        speed = gameManager.rotateSpeed;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");

            HotRopeJumpPlayerMovement playerMovement = other.gameObject.GetComponent<HotRopeJumpPlayerMovement>();
            //gameManager.PlayerDeath(playerMovement.playerIndex);
            playerMovement.enabled = false;
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
