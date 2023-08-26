using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseHoverBuilding : MonoBehaviour
{
    public GameObject buildingInfoObject, createCosteInfoObject, abbrechenUI;
    IINFO info;

    public bool onBuild;
    public float drawDeleay;
    float t;

    private void OnMouseEnter()
    {
        MainUI.inWorldUI = true;
    }

    private void OnMouseOver()
    {
        if (t > drawDeleay)
        {
            if (onBuild)
            {
                buildingInfoObject.SetActive(false);
                createCosteInfoObject.SetActive(false);
                abbrechenUI.SetActive(true);
            }
            else
            {
                //info
            }
        }
        else t += Time.deltaTime;
    }

    private void OnMouseExit()
    {
        t = 0;
        MainUI.inWorldUI = false;
        buildingInfoObject.SetActive(false);
        createCosteInfoObject.SetActive(false);
        abbrechenUI.SetActive(false);
    }

}
