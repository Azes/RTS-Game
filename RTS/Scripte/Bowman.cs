using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : IHuman
{
    public Material om;
    Material _m;
    bool toggel;
    Color nc;
    public float Range;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Health;
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
            if (isSelectet) _m.SetColor("_BaseColor", typeColor);
            else _m.SetColor("_BaseColor", nc);
        }
        if (isDead()) Destroy(gameObject);
    }
}
