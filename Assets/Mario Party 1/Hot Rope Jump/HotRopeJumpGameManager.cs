using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotRopeJumpGameManager : MonoBehaviour
{
    public float rotateSpeed;
    float speedUpIncrement = 50;
    //350 max speed?????
    private int loopCount = 0;
    
    [SerializeField] GameObject centerFireball;

    bool isCenterUp = false;

    private List<int> deathOrder = new List<int>();

    private void Update()
    {
        if (isCenterUp && centerFireball.transform.position.y < 0.5)
        {
            isCenterUp = false;
            loopCount++;
            UpdateSpeed();
        }
        if(!isCenterUp && centerFireball.transform.position.y > 3.5)
        {
            isCenterUp = true;
        }
    }

    void UpdateSpeed()
    {
        switch (loopCount)
        {
            case 5:
            case 10:
            case 15:
            case 20:
            case 25:
            case 30:
                rotateSpeed += speedUpIncrement;
                break;
        }
    }

    public void PlayerDeath(int index)
    {
        deathOrder.Add(index);
    }
}
