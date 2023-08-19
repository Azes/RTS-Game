using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{

    public static bool inUI;

    //Human menu
    //ui gameobject f�r einheiten
    public GameObject HumanSelect;
    //die verschiednenen einheiten icons
    public GameObject villager, knight, bowman;
    //tmp f�r die anzahl der einheiten 
    public TextMeshProUGUI v_value, kn_value, b_value;
    //gameobjecte f�r die einzelinfo und die gruppen info
    public GameObject SingleInfo, GroupInfo;

    //die einheiten liste the heart
    public List<IHuman> humans = new List<IHuman>();

    // Singel infos 
    //der index f�r die jeweilige einheiten info
    //[0] Villager [1] Knight [2] Bowman
    int singleIndex = -1;
    //tpm f�r den namen der einheit
    public TextMeshProUGUI singleInfo_Name;
    //array f�r die werte der jeweiligen kategorie
    public TextMeshProUGUI[] singleInfo_values;
    //eine list diee mit dem listen index der einheit
    //mit einem array das sagt welche infos activiert werden 
    List<int[]> activateInfosList = new List<int[]> {
        new int[] {0, 2, 3, 4, 6},//Villager
        new int[] {0, 1, 2, 3, 4},//Knight
        new int[] {0, 1, 2, 3, 4, 5}//Bowman
        };
    //["health"]      = [0]
    //["armor"]       = [1]
    //["speed"]       = [2]
    //["damage"]      = [3]
    //["dps"]         = [4]
    //["range"]       = [5]
    //["carryweight"] = [6]
   
    //eine hilfs klasse die das info update triggert 
    public class Trigger
    {
        private bool t = true;

        public bool Is()
        {
            bool i = t;
            t = false;
            return i;
        }

        public void reset()
        {
            t = true;
        }
    }

    Trigger singleTigger = new Trigger();
    //ein buffer um zusehen ob die anzahl der einheiten sich ver�ndert hat
    int humanCountBuffer;
    //ein buffer umzusehen ob die lisst sich ge�ndert hat
    IHuman hbuff;

    //ein hilfs buffer um zwischen den verschiedenen infos zuwelchseln von sing to group
    public bool infoToggle;


    ///Group

    //das gruppen parent gameobject also der container vom scrollrect
    public Transform GroupContent;
    //das prefabs von dem EinheitBar Ui Element
    public GameObject Einheit_bar;
    //die sortier buttons
    public ModeButton sortingButton, orderButton;
    //ob die liste sortiert ist oder nicht
    public bool sorted = false;
    //die reinfolge wie die eiheiten gelisted sind 
    //[0] garnicht
    //[1] mit amwenigsten leben zuerst
    //[2] mit amwenigsten leben zuletzt
    public int order = 0;




   
    // Update is called once per frame
    void Update()
    {
        // erst kommen die einheit simbole und updatet die anzahl mit humanValues
        if (humans.Count > 0)
        {
            //wir buffern ob sich die anzahl der einheiten ge�ndert hat
            bool isHchange = isHumanListChange();

            if (!HumanSelect.activeInHierarchy) HumanSelect.SetActive(true);
            humanValues(isHchange);

            //wir �berpr�fen mit checkIsSingel ob eine oder mehrere einheiten ausgew�hlt sind
            bool s = checkIsSingel();

            //wenn der buffer sich ver�ndert wwchseln wir zwischen den infos objecten
            if (infoToggle != s)
            {
                infoToggle = s;

                SingleInfo.SetActive(s);
                GroupInfo.SetActive(!s);
            }


            //wenn logischer weise der s(ingel) boolean fals ist haben wir mehrere einheiten
            //und aktivieren die group methoden
            if (!s)
            {///dann das 2.

                ///als letztes das 4.
                sorted = (sortingButton.mode == 1 ? true : false);
                order = orderButton.mode;
               
                if (sortingButton.isChange() || orderButton.isChange())
                    sortedList();///am ende 5.

                ///und dann das 3.
                isListValueChange(isHchange);
            }/// erst das 1.
            else singleInformationFields();

        }
        else if (HumanSelect.activeInHierarchy) HumanSelect.SetActive(false);

    }

    //eine methode damit wir nicht durchg�ng updaten sondern nur wenn wirs m�ssen
    bool isHumanListChange()
    {
        if (humans.Count != humanCountBuffer || humans[0] != hbuff)
        {
            hbuff = humans[0];
            humanCountBuffer = humans.Count;
            return true;
        }

        return false;
    }

    void humanValues(bool it)
    {
        if (it)
        {
            //ints die unsere einheiten z�hlen
            int v = 0;//villager
            int w = 0;//knght
            int b = 0;//bowman
            //wir z�hlen die jeweilige einheit hoch
            for (int i = 0; i < humans.Count; i++)
            {
                if (humans[i] is Villager) v++;
                else if (humans[i] is Warrior) w++;
                else if (humans[i] is Bowman) b++;
            }

            //wirr aktivieren nur die einheiten die auch asugew�hlt sind
            if (v >= 1)
            {
                if (!villager.activeInHierarchy) villager.SetActive(true);
                v_value.text = v.ToString();
            }
            else if (villager.activeInHierarchy) villager.SetActive(false);

            if (w >= 1)
            {
                if (!knight.activeInHierarchy) knight.SetActive(true);
                kn_value.text = w.ToString();
            }
            else if (knight.activeInHierarchy) knight.SetActive(false);

            if (b >= 1)
            {
                if (!bowman.activeInHierarchy) bowman.SetActive(true);
                b_value.text = b.ToString();
            }
            else if (bowman.activeInHierarchy) bowman.SetActive(false);
        }
    }

    bool checkIsSingel()
    {
        //ist die einheiten anzahl gr��er als 1 werden die singelField attribute zur�ck gesetzt
        if (humans.Count > 1)
        {
            singleIndex = -1;
            singleTigger.reset();
            return false;
        }
        else
        {
            //wir �berprufen hier damit wir nicht unn�tig updaten
            if (humans.Count > 0)
            {
                //�berpr�ft welcher typ einheit in unserer IHuman list ist und setzen den index
                if (humans[0] is Villager) singleIndex = 0;
                else if (humans[0] is Warrior) singleIndex = 1;
                else if (humans[0] is Bowman) singleIndex = 2;

                singleTigger.reset();
            }
            // giebt treu zur�ck wenn wir nur eine einheit ausgew�hlt haben
            return (humans.Count == 1);
        }
    }
   
    void singleInformationFields()
    {
        //wir nutzen den singelTrigger um einmal die information zu updaten und nicht jeden frame
        if (singleTigger.Is())
        {
            for (int j = 0; j < singleInfo_values.Length; j++)
            {
                singleInfo_values[j].text = " ";
                singleInfo_values[j].transform.parent.gameObject.SetActive(false);
            }
            //setzt den header namen
            if (singleIndex == 0) singleInfo_Name.text = "Villager";
            else if (singleIndex == 1) singleInfo_Name.text = "Knight";
            else if (singleIndex == 2) singleInfo_Name.text = "Bowman";

            //aktiviert nur die spezifischen feld f�r die jeweilige einheit
            for (int k = 0; k < activateInfosList[singleIndex].Length; k++)
            {
                for (int j = 0; j < singleInfo_values.Length; j++)
                {
                    if (j == activateInfosList[singleIndex][k])
                    {
                        singleInfo_values[j].transform.parent.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        //wir bufferm die einheit erleichte die schreibweise
        var h = humans[0];

        //updatet die singelInfos f�r die einheiten
        if (singleIndex == 0)
        {
            singleInfo_values[0].text = h.Health.ToString();
            singleInfo_values[2].text = h.Speed.ToString();
            singleInfo_values[3].text = h.Damage.ToString();
            singleInfo_values[4].text = (h.Damage * h.attackSpeed).ToString();
            //spizele nur einheit spezifische variabelm m�ssen einzeln  gecasted werden 
            //um drauf zuzugreifen
            if (h is Villager)
            {
                Villager v = (Villager)h;
                singleInfo_values[6].text = v.carryweight.ToString();
            }

        }
        else if (singleIndex == 1)
        {
            singleInfo_values[0].text = h.Health.ToString();
            singleInfo_values[1].text = h.Armor.ToString();
            singleInfo_values[2].text = h.Speed.ToString();
            singleInfo_values[3].text = h.Damage.ToString();
            singleInfo_values[4].text = (h.Damage * h.attackSpeed).ToString();
        }
        else if (singleIndex == 2)
        {
            singleInfo_values[0].text = h.Health.ToString();
            singleInfo_values[1].text = h.Armor.ToString();
            singleInfo_values[2].text = h.Speed.ToString();
            singleInfo_values[3].text = h.Damage.ToString();
            singleInfo_values[4].text = (h.Damage * h.attackSpeed).ToString();

            if (h is Bowman)
            {
                Bowman b = (Bowman)h;
                singleInfo_values[5].text = b.Range.ToString();
            }
        }
    }

  
    void isListValueChange(bool it)
    {
        if (it)
        {
            //wird �berpr�fen ob sich die anzahl der child objecte also auch die anzahl
            //der einheiten ver�ndert hat in dem fall f�gen wir die einheit in der group ui 
            //entweder hinzu oder l�schen sie wenn nicht mehr ausgew�hlt

            //buff f�r zul�schende grou child objecte
            List<GameObject> buff = new List<GameObject>();

            //�berpr�ft ob die ui group child objects mit der in humans aufgelisten
            //einheiten �bereinstimmen wenn nicht kommt sie in den l�sch buffer
            for (int i = 0; i < GroupContent.childCount - 1; i++)
            {
                if (!humans.Contains(GroupContent.GetChild(i).GetComponent<IHuman>()))
                    buff.Add(GroupContent.GetChild(i).gameObject);
            }
            //l�scht den buffer
            for (int i = 0; i < buff.Count; i++)
                Destroy(buff[i]);


            //f�gt die EinheitBar objecte der Group hinzu
            for (int i = 0; i < humans.Count; i++)
            {
                var ho = Instantiate(Einheit_bar);
                ho.GetComponent<EinheitBar>().ih = humans[i];
                ho.transform.SetParent(GroupContent);
            }

            //erst die buttens dann sortedList hinzuf�gen
            sortedList();
        }
        
    }
   
    void sortedList()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in GroupContent)
        {
            children.Add(child);
        }

        if (!sorted)
        {
            if (order == 1)
            {
                children.Sort((a, b) =>
                {
                    return a.GetComponent<EinheitBar>().value.CompareTo(b.GetComponent<EinheitBar>().value);
                });
            }
            else if( order == 2)
            {
                children.Sort((a, b) =>
                {
                    return b.GetComponent<EinheitBar>().value.CompareTo(a.GetComponent<EinheitBar>().value);
                });
            }

            if (order == 1 || order == 2)
            {
                for (int i = 0; i < children.Count; i++)
                    children[i].SetSiblingIndex(i);
            }
            else
            {
                List<Transform> sortedChildren = new List<Transform>();

                foreach (IHuman human in humans)
                {
                    foreach (Transform child in children)
                    {
                        if (child.GetComponent<EinheitBar>().ih == human)
                        {
                            sortedChildren.Add(child);
                            break;
                        }
                    }
                }

                // Setze die sortierte Reihenfolge der Objekte in den entsprechenden Containern zur�ck
                for (int i = 0; i < sortedChildren.Count; i++)
                {
                    sortedChildren[i].SetSiblingIndex(i);
                }
            }
        }
        else
        {
            children.Sort((a, b) =>
            {

                // ih ist die variable IHuman
                IHuman scriptA = a.GetComponent<EinheitBar>().ih.GetComponent<IHuman>();
                IHuman scriptB = b.GetComponent<EinheitBar>().ih.GetComponent<IHuman>();

                if (scriptA != null && scriptB != null)
                {
                    if (scriptA is Villager && !(scriptB is Villager))
                    {
                        return -1;
                    }
                    else if (scriptA is Warrior && scriptB is Bowman)
                    {
                        return -1;
                    }
                    else if (scriptA is Bowman && !(scriptB is Bowman))
                    {
                        return 1;
                    }
                    return 0;
                }
                return 0;
            });


            if (order == 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SetSiblingIndex(i);
                }
            }
            if (order == 1 || order == 2)
            {
                List<Transform> villagers = new List<Transform>();
                List<Transform> warriors = new List<Transform>();
                List<Transform> bowmen = new List<Transform>();

                foreach (Transform child in children)
                {
                    IHuman script = child.GetComponent<EinheitBar>().ih.GetComponent<IHuman>();
                    if (script is Villager)
                    {
                        villagers.Add(child);
                    }
                    else if (script is Warrior)
                    {
                        warriors.Add(child);
                    }
                    else if (script is Bowman)
                    {
                        bowmen.Add(child);
                    }
                }

                if (order == 1)
                {
                    villagers.Sort((a, b) => a.GetComponent<EinheitBar>().value.CompareTo(b.GetComponent<EinheitBar>().value));
                    warriors.Sort((a, b) => a.GetComponent<EinheitBar>().value.CompareTo(b.GetComponent<EinheitBar>().value));
                    bowmen.Sort((a, b) => a.GetComponent<EinheitBar>().value.CompareTo(b.GetComponent<EinheitBar>().value));
                }
                else if (order == 2)
                {
                    villagers.Sort((a, b) => b.GetComponent<EinheitBar>().value.CompareTo(a.GetComponent<EinheitBar>().value));
                    warriors.Sort((a, b) => b.GetComponent<EinheitBar>().value.CompareTo(a.GetComponent<EinheitBar>().value));
                    bowmen.Sort((a, b) => b.GetComponent<EinheitBar>().value.CompareTo(a.GetComponent<EinheitBar>().value));
                }

                int currentIndex = 0;
                foreach (Transform child in villagers)
                {
                    child.SetSiblingIndex(currentIndex);
                    currentIndex++;
                }
                foreach (Transform child in warriors)
                {
                    child.SetSiblingIndex(currentIndex);
                    currentIndex++;
                }
                foreach (Transform child in bowmen)
                {
                    child.SetSiblingIndex(currentIndex);
                    currentIndex++;
                }
            }
        }
    }


}
