using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strap : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TheGame.Instance.Score++;
        Debug.Log("Platform");
    }
}
