using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

    [SerializeField]
    GameObject GameCanvas;
    [SerializeField]
    GameObject InGameUI;
    [SerializeField]
    GameObject GameOverUI;
    [SerializeField]
    GameObject MainMenuUI;

    public void GameRestart()
    {
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);

        TheGame.Instance.GameClear();
        TheGame.Instance.GameStart();
    }


    public void Menu()
    {
        MainMenuUI.SetActive(true);
        GameOverUI.SetActive(false);
        TheGame.Instance.GameClear();
        TheGame.Instance.SetDefault();

    }
}
