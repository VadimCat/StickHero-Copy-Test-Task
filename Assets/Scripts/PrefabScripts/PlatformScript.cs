using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformScript : MonoBehaviour {


    [SerializeField]
    GameObject BonusPref;
    GameObject bonusInstance;


    void Awake ()
    {
        bonusInstance = Instantiate(BonusPref, new Vector3(0, 500), new Quaternion());
        bonusInstance.transform.SetParent(transform, false);
    }
}
