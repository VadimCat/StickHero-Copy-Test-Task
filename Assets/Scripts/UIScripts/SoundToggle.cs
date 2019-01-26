using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggle : MonoBehaviour {

    [SerializeField]
    GameObject SoundOnButton;
    [SerializeField]
    GameObject SoundOffButton;

    bool IsOnButtonActive;
    bool IsOffButtonActive;

    void Start()
    {
        IsOnButtonActive = true;
        IsOffButtonActive = false;
    }

    public void ToggleSound()
    {
        TheGame.Instance.IsSoundOn = !TheGame.Instance.IsSoundOn;
        SoundOnButton.SetActive(IsOnButtonActive = !IsOnButtonActive);
        SoundOffButton.SetActive(IsOffButtonActive = !IsOffButtonActive);
    }
}
