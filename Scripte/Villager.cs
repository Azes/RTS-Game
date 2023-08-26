using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : IHuman
{

    public Material stM;
    Material _m;
    bool toggel;
    Color or;

    public bool isWork;
    public int carryweight = 200;

    //new
    public Transform walkTarget;

    // Start is called before the first frame update
    void Start()
    {
        //new
         agent = GetComponent<NavMeshAgent>();
         agent.speed = Speed;
        //

        _m = Instantiate(stM);
        GetComponent<Renderer>().material = _m;

        or = _m.GetColor("_BaseColor");
    }

    //new
    protected override void nextUpdate()
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
