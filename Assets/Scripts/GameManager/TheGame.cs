using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TheGame : MonoBehaviour
{
    const int X_START = -260;
    const int Y_START = -225;
    const int DISTANCE_RANDOM_MIN = 500;
    const int DISTANCE_RANDOM_MAX = 800;

    GameObject heroCurrent;
    GameObject platformCurrent;
    GameObject platformNext;
    GameObject platformPrev;
    GameObject strapCurrent;
    Vector3 tempPos;
    AudioListener audioListener;

    [SerializeField] GameObject StrapPref;
    [SerializeField] GameObject HeroPref;
    [SerializeField] GameObject[] PlatformArray;
    [SerializeField] GameObject CanvasCurrent;
    [SerializeField] GameObject CameraFollow;
    [SerializeField] GameObject InGameUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject MainMenuUI;
    [SerializeField] Text GameOverScoreText;
    [SerializeField] Text ScoreText;
    [SerializeField] Text HighScoreText;
    [SerializeField] AudioSource SoundBackgorund;
    [SerializeField] AudioSource SoundGameOver;
    [SerializeField] AudioSource SoundSrapStretch;
    [SerializeField] AudioSource SoundStrapStop;
    [SerializeField] AudioSource SoundStrapFall;
    [SerializeField] AudioSource SoundBonus;


    public static TheGame Instance;

    bool isTapAvailable;

    bool hasReseted;
    public bool HasReseted
    {
        get
        {
            return hasReseted;
        }

        set
        {
            hasReseted = value;
        }
    }

    bool hasTouched;
    public bool HasTouched
    {
        get
        {
            return hasTouched;
        }

        set
        {
            hasTouched = value;
        }
    }


    int score;
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            HasTouched = true;
            score = value;
            ScoreText.text = score.ToString();
        }
    }


    bool isSoundOn;
    public bool IsSoundOn
    {
        get
        {
            return isSoundOn;
            
        }

        set
        {
           audioListener.enabled = isSoundOn = value;
        }
    }


    public void GameStart()
    {
        StartCoroutine(CameraStart());
        StartCoroutine(TapActivate());
        InGameUI.SetActive(true);
        ScoreText.text = "0";
        score = 0;
        if (heroCurrent == null)
        {
            Debug.Log("OOOOPS");
            SetDefault();
        }
        PlatformSet(DISTANCE_RANDOM_MIN, DISTANCE_RANDOM_MAX, PlatformArray);
    }


    public void SetDefault()
    {
        tempPos = new Vector3(X_START, Y_START, 0);
        CameraFollow.transform.position = new Vector3(0, 0, -10);
        PlatformSet(X_START, X_START, PlatformArray);
        heroCurrent = Instantiate(HeroPref, tempPos, new Quaternion(), CanvasCurrent.transform);
        CameraFollow.transform.SetParent(heroCurrent.transform, false);
    }


    public void GameClear()
    {
        Destroy(platformCurrent);
        Destroy(platformNext);
        Destroy(strapCurrent);
        Destroy(heroCurrent);
        heroCurrent = null;
    }


    public void PlayBonusSound()
    {
        SoundBonus.Play();
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        audioListener = CameraFollow.GetComponent<AudioListener>();
        IsSoundOn = true;
        if (PlayerPrefs.HasKey("Best"))
        {
            PlayerPrefs.SetInt("Best", 0);
        }

        SetDefault();
        isTapAvailable = false;
        MainMenuUI.SetActive(true);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (isTapAvailable))
        {
            Destroy(strapCurrent);
            strapCurrent = Instantiate(StrapPref, new Vector3(25, -50), new Quaternion());
            strapCurrent.transform.SetParent(heroCurrent.transform, false);
            strapCurrent.transform.SetParent(CanvasCurrent.transform, true);
            SoundSrapStretch.mute = false;
        }
        if (Input.GetMouseButton(0) && (isTapAvailable))
        {
            strapCurrent.transform.localScale += new Vector3(0, 2f, 0);
            
        }
        if (Input.GetMouseButtonUp(0) && (isTapAvailable))
        {
            SoundSrapStretch.mute = true;
            isTapAvailable = false;
            hasTouched = false;
            StartCoroutine(GameIteration());
        }
    }


    void PlatformSet(int min, int max, GameObject[] plArr)
    {
        int platformNumber = Random.Range(0, plArr.Length);
        if (platformNext != null)
        {
            tempPos = tempPos + new Vector3(Random.Range((plArr.Length - 1 - platformNumber) * 50 + 150, max), 0, 0);
        }
        platformPrev = platformCurrent;
        platformCurrent = platformNext;
        platformNext = Instantiate(PlatformArray[platformNumber], tempPos, new Quaternion());
        platformNext.transform.SetParent(CanvasCurrent.transform, false);
    }


    IEnumerator GameIteration()
    {
        SoundStrapStop.Play();
        yield return StartCoroutine(StrapRotate(strapCurrent));
        SoundStrapFall.Play();
        yield return new WaitForSeconds(0.1f);
        if (HasTouched)
        {
            PlatformSet(DISTANCE_RANDOM_MIN, DISTANCE_RANDOM_MAX, PlatformArray);
            yield return StartCoroutine(HeroMoveOk(heroCurrent));
            Destroy(platformPrev);
            isTapAvailable = true;
        }
        else
        {
            CameraFollow.transform.SetParent(null, true);
            yield return StartCoroutine(HeroMoveFail(heroCurrent));
            StartCoroutine(MoveDown(heroCurrent));
            yield return StartCoroutine(MoveDown(strapCurrent));
            GameOver();
        }
    }


    IEnumerator StrapRotate(GameObject obj)
    {
        for (int i = 0; i < 45; i++)
        {
            obj.transform.Rotate(0, 0, -2f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator HeroMoveOk(GameObject hero)
    {
        float tempDeltaX = (platformCurrent.transform.position.x
            - platformPrev.transform.position.x) / 50;
        for (int i = 0; i < 50; i++)
        {
            hero.transform.Translate(tempDeltaX, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator HeroMoveFail(GameObject hero)
    {

        for (int i = 0; i < 10; i++)
        {
            hero.transform.Translate(5f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator MoveDown(GameObject obj)
    {

        for (int i = 0; i < 50; i++)
        {
            obj.transform.Translate(0, -22f, 0, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator TapActivate()
    {
        yield return new WaitForSeconds(0.1f);
        isTapAvailable = true;
    }


    IEnumerator CameraStart()
    {
        for (int i = 0; i < 35; i++)
        {
            CameraFollow.transform.Translate(10f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }


    void GameOver()
    {
        SoundGameOver.Play();
        if (score > PlayerPrefs.GetInt("Best"))
        {
            PlayerPrefs.SetInt("Best", score);
        }
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameOverScoreText.text = score.ToString();
        HighScoreText.text = score.ToString();
    }
}