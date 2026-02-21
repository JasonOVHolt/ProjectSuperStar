using System.Collections.Generic;
using UnityEngine;

public class BalloonBurstGameManagerMP1 : MonoBehaviour
{
    private List<int> popOrder = new List<int>();


    public void PlayerPopped(int index)
    {
        popOrder.Add(index);
    }
}


