using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HexagonHeatGameManager : MonoBehaviour
{
    [SerializeField] GameObject[] colorObj;
    [SerializeField] Image colorImage;
    private string[] color = new string[] { "yellow", "red", "aqua", "white", "green", "blue", "pink" };
    private List<Animator> animators = new List<Animator>();
    private List<int> deathOrder = new List<int>();

    float timer = 10;
    float resetTime = 3;
    [SerializeField] Vector2 setTime;
    [SerializeField] float shrinkSize;
    bool areHexDown = false;

    int rounds = 0;

    int colorIndex;

    private void Awake()
    {
        PickColor();
        for (int i = 0; i < colorObj.Length; i++)
        {
            animators.Add(colorObj[i].GetComponent<Animator>());
        }
    }

    private void Update()
    {

        if( timer >= 0 )
            timer -= Time.deltaTime;
        else
        {
            if (areHexDown)
            {
                MoveHexUp(colorIndex);
                UpdateRoundTimes();
                timer = Random.Range(setTime.x, setTime.y);
            }
            else
            {
                MoveHexDown(colorIndex);
                timer = resetTime;
            }
        }
    }




    public void PlayerDeath(int index)
    {
        deathOrder.Add(index);
    }

    private void MoveHexUp(int index)
    {
        for (int i = 0; i < colorObj.Length; i++)
        {
            if (i != index)
            {
                animators[i].SetTrigger("HexUp");
            }

        }
        areHexDown = false;
        rounds++;
        PickColor();

    }

    private void MoveHexDown(int index)
    {
        for (int i = 0; i < colorObj.Length; i++)
        {
            if (i != index)
            {
                animators[i].SetTrigger("HexDown");
            }
        }
        areHexDown = true;
    }

    private void PickColor()
    {
        colorIndex = Random.Range(0, colorObj.Length);
        UpdateColorImage();
        Debug.Log("Color Index: " +  colorIndex);
    }


    private void UpdateRoundTimes()
    {
        switch (rounds)
        {
            case 5:
            case 10:
            case 15:
            case 20:
                setTime = new Vector2(setTime.x - shrinkSize, setTime.y - shrinkSize);
                break;
        }
    }

    private void UpdateColorImage()
    {
        switch (colorIndex)
        {
            case 0:
                colorImage.color = colorObj[0].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 1:
                colorImage.color = colorObj[1].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 2:
                colorImage.color = colorObj[2].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 3:
                colorImage.color = colorObj[3].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 4:
                colorImage.color = colorObj[4].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 5:
                colorImage.color = colorObj[5].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
            case 6:
                colorImage.color = colorObj[6].transform.GetChild(0).GetComponent<Renderer>().material.color;
                break;
        }
    }

}
