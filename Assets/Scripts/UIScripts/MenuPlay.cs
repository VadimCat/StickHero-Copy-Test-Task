using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlay : MonoBehaviour {

    [SerializeField]
    GameObject MainMenuCanvas;
    
    public void Play()
    {
        MainMenuCanvas.SetActive(false);
        TheGame.Instance.GameStart();
    }
}
