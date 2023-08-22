using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heiler : IHuman
{
    public Material stM;
    Material _m;
    bool toggel;
    Color or;

    public int range = 40;
    public int healPower = 10;

    // Start is called before the first frame update
    void Start()
    {

        _m = Instantiate(stM);
        GetComponent<Renderer>().material = _m;

        or = _m.GetColor("_BaseColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectet != toggel)
        {
            toggel = isSelectet;
            if (isSelectet) _m.SetColor("_BaseColor", typeColor);
            else _m.SetColor("_BaseColor", or);
        }

        if (isDead()) Destroy(gameObject);
    }
}
