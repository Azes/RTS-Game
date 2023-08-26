using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IINFO : MonoBehaviour
{
    [System.Serializable]
    public class CreateInfo
    {
        public string Name;
        public Sprite icon;
        public float ProcessTime;
        public int Gold, Stone, Wood, Food;
    }

    public CreateInfo costInfo = new CreateInfo();

}
