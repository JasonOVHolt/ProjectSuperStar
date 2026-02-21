using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalloonBurstPlayerMovement : MonoBehaviour
{
    string lastButtonPressed = "east";
    GameObject balloon;
    int playerIndex=1;
    [SerializeField] float inflationSize;
    Color[] balloonColors = new Color[] { Color.yellow, Color.orange, Color.red };
    BalloonBurstGameManagerMP1 gameManager;
    bool hasPopped = false;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<BalloonBurstGameManagerMP1>();
        balloon = GameObject.Find("Balloon" + playerIndex);
    }

    public void OnJump(InputValue value)
    {
        if(lastButtonPressed == "east" && !hasPopped)
        {
            Inflate();
            lastButtonPressed = "south";
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (lastButtonPressed == "south" && !hasPopped)
        {
            Inflate();
            lastButtonPressed = "east";
        }
    }

    void Inflate()
    {
        if (balloon.transform.localScale.x > 3)
        {
            gameManager.PlayerPopped(playerIndex);
            balloon.SetActive(false);
            hasPopped = true;
            Debug.Log("BALLOON POPPED");
            this.enabled = false;
        }
        else
            balloon.transform.localScale = new Vector3(balloon.transform.localScale.x + inflationSize, balloon.transform.localScale.y + inflationSize, balloon.transform.localScale.z + inflationSize);

        if (balloon.transform.localScale.x < 0.75)
        {

        }
        else if (balloon.transform.localScale.x < 1.5)
        {
            balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = balloonColors[0];
        }
        else if (balloon.transform.localScale.x < 2.5)
        {
            balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = balloonColors[1];
        }
        else
        {
            balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = balloonColors[2];
        }

        
    
    }



    //3 is pop size
    //
}
