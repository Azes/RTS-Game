using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IHuman
{
    public Material om;
    Material _m;
    bool toggel;
    Color nc;

    // Start is called before the first frame update
    void Start()
    {
        nc = om.GetColor("_BaseColor");
        _m = Instantiate(om);
        GetComponent<Renderer>().material = _m;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectet != toggel)
        {
            toggel = isSelectet;
            if (isSelectet) _m.SetColor("_BaseColor", Color.red);
            else _m.SetColor("_BaseColor", nc);
        }
    }
}
