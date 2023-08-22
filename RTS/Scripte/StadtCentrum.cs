using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StadtCentrum : IBuilding
{

    //ressoucres
    public int Gold, Stone, Wood, Food, Population, MaxPopulation = 25;
    int gbuff, sbuff, wbuff, fbuff, pbuff, mpbuff;
    public MainUI mui;

    //Humans
    public GameObject humanPrebas;
    public Transform spawn;

    public List<IHuman> ALL_HUMANS = new List<IHuman>();

    //UI elemente
    public TextMeshProUGUI processCount, jobless, buildhealth;
    public ProcessElement pe;

    int healthbuff;

    // Start is called before the first frame update
    void Start()
    {
        buildingHealth = 100000;
        pe.processLimet = 120;
    }

    // Update is called once per frame
    void Update()
    {
        processCount.text = pe.waitHumans.Count.ToString();
        updateJobless();
        updateBuildHealthText();
        UpdateResouces();

        if (pe.endProcess)
        {
            bool can = (Population + 1 < MaxPopulation ? true : false);
         
            if (can)
            {
                IHuman human = pe.spawnHuman(spawn, true).GetComponent<IHuman>();
                ALL_HUMANS.Add(human);
                Population = ALL_HUMANS.Count;
            }
        }
        
    }

    void UpdateResouces()
    {
        if(Gold != gbuff || Stone != sbuff || Wood != wbuff 
            || Food != fbuff || Population != pbuff|| MaxPopulation != mpbuff)
        {
            gbuff = Gold; sbuff = Stone;
            wbuff = Wood; fbuff = Food;
            pbuff = Population; mpbuff = MaxPopulation;
        
            mui.gold_value.text = Gold.ToString();
            mui.stone_value.text = Stone.ToString();
            mui.wood_value.text = Wood.ToString();
            mui.food_value.text = Food.ToString();
            mui.pop_value.text = Population.ToString();
            mui.maxpop_value.text = MaxPopulation.ToString();

        }
    }

    void updateJobless()
    {
        int j = 0;

        for (int i = 0; i < ALL_HUMANS.Count; i++)
        {
            if (ALL_HUMANS[i] is Villager)
            {
                if (!ALL_HUMANS[i].GetComponent<Villager>().isWork)
                {
                    j++;
                }
            }
        }

        jobless.text = j.ToString();
    }

    void updateBuildHealthText()
    {
        if(buildingCurrentHealth != healthbuff)
        {
            healthbuff = buildingCurrentHealth;

            buildhealth.text = buildingCurrentHealth.ToString() + " | " + buildingHealth.ToString();
        }
    }

    public void addHumanToProgess()
    {
        var info = humanPrebas.GetComponent<IHuman>().costInfo;
        if (Gold - info.Gold >= 0 && Food - info.Food >= 0)
        {
            Gold -= info.Gold; 
            Food -= info.Food;
            pe.addHumanToProcess(humanPrebas.GetComponent<IHuman>());
        }
    }

}
