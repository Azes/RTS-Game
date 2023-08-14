using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : IHuman
{

    public Material stM;
    Material _m;
    bool toggel;


    // Start is called before the first frame update
    void Start()
    {
        _m = Instantiate(stM);
        GetComponent<Renderer>().material = _m;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelectet != toggel)
        {
            toggel = isSelectet;
            if (isSelectet) _m.SetColor("_BaseColor", Color.green);
            else _m.SetColor("_BaseColor", Color.white);
        }    
    }
}
