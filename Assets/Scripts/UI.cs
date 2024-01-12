using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public PlayerController player;
    public Animator arrowAnimator;

    private bool reloadThis;
    private bool reload;

    private float volume;
    public List<AudioSource> sounds;

    public GameObject bar;
    public GameObject arrows;
    public GameObject arrowmoving;
    public GameObject arrowmovingchild;

    public int gold; // current count
    public int earnedGold; // level bonus
    public int shots; // current count
    public int currentshots; // max shots
    public float timerCurrent; // current count
    public float timerMax; // max

    public TMP_Text shotCount;
    public TMP_Text timer;
    public TMP_Text goldcoins;
    public TMP_Text earnCoinstext;

    public int price1;
    public int price2;
    public TMP_Text price1text;
    public TMP_Text price2text;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject errorScreen;
    public GameObject settingScreen;
    public GameObject menuScreen;
    public GameObject volumeOn;
    public GameObject volumeOff;
    public GameObject loadingScreen;
    public Image loading;

    private float mode; // unique level
    private int howManyLevelsDone; // real number of last level
    private int levelMax; // how many levels total
    private float chosenLevel; // real number of level

    public bool result;

    public int a1bought;
    public Image iconbutton;
    public List<GameObject> reliefs;
    public Color32 activecolor;

    // tutorial
    public float movementTip;
    public float punchTip;
    public float specialsTip;
    public GameObject pointerTutorial;
    public GameObject tutorialText1;
    public GameObject tutorialText2;
    public Animator tutorialanimated;

    // circles
    public int howmanycircles;

    public GameObject light;
    private bool lighton;

    void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("PreloaderScene");
        asyncOperation.allowSceneActivation = false;

        gold = PlayerPrefs.GetInt("gold");
        mode = PlayerPrefs.GetFloat("mode");
        levelMax = PlayerPrefs.GetInt("levelMax");
        volume = PlayerPrefs.GetFloat("volume");
        chosenLevel = PlayerPrefs.GetFloat("chosenLevel");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");
        a1bought = PlayerPrefs.GetInt("a1bought");
        a1check();

        movementTip = PlayerPrefs.GetFloat("movementTip");
        punchTip = PlayerPrefs.GetFloat("punchTip");
        specialsTip = PlayerPrefs.GetFloat("specialsTip");
        tutorialanimated.enabled = false;
        pointerTutorial.SetActive(false);
        tutorialText1.SetActive(false);
        tutorialText2.SetActive(false);

        tutorialText1.SetActive(false);
        tutorialText2.SetActive(false);

        sounds[0].Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

        bar.SetActive(false);
        arrows.SetActive(false);
        arrowmoving.SetActive(false);
        arrowAnimator.enabled = false;
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        errorScreen.SetActive(false);
        settingScreen.SetActive(false);
        menuScreen.SetActive(false);
        loadingScreen.SetActive(false);

        price1text.text = price1.ToString("0");
        price2text.text = price2.ToString("0");

        // levels
        if (mode != 0)
        {
            if(mode <= 3)
            {
                earnedGold = 50;
                shots = 5;
            }
            else if(mode > 3 && mode <= 6)
            {
                earnedGold = 75;
                shots = 10;
            }
            else
            {
                earnedGold = 100;
                shots = 15;
            }

            timerMax = 300;
            foreach(GameObject relief in reliefs)
            {
                relief.SetActive(false);
            }
            reliefs[(int)mode - 1].SetActive(true);

            if (mode == 1 || mode == 2 || mode == 4 || mode == 6 || mode == 10)
            {
                howmanycircles = 1;
            }
            else if(mode == 3 || mode == 7 || mode == 8)
            {
                howmanycircles = 2;
            }
            else if(mode == 5 || mode == 9)
            {
                howmanycircles = 3;
            }
        }

        timerCurrent = timerMax;
        earnCoinstext.text = "+" + earnedGold.ToString("0");
    }

    public void Update()
    {
        if (!lighton)
        {
            lighton = true;
               GameObject lightning = Instantiate(light, transform.position, Quaternion.Euler(50, -30, 0));
        }

        if (timerCurrent > 0)
        {
            timerCurrent -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timerCurrent / 60);
            int seconds = Mathf.FloorToInt(timerCurrent % 60);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            lose();
        }
        shotCount.text = "SHOTS: " + currentshots.ToString("0") + "/" + shots.ToString("0");
        goldcoins.text = gold.ToString("0");

        // reload scene
        if (loadingScreen.activeSelf == true)
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = 0;
            }

            if (loading.fillAmount < 0.9f)
            {
                loading.fillAmount = Mathf.Lerp(loading.fillAmount, 1, Time.deltaTime * 2);
            }
            else
            {
                if (!reload)
                {
                    reload = true;
                    if (reloadThis)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
            }
        }
        else
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = volume;
            }
        }

        // tutorial

        if (movementTip == 0)
        {
            if (!tutorialanimated.enabled)
            {
                tutorialanimated.enabled = true;
                pointerTutorial.SetActive(true);
            }
            tutorialanimated.Play("ShowMovement");
        }
        else if (movementTip == 1 && punchTip == 0 && !arrowmoving.activeSelf)
        {
            if (!tutorialanimated.enabled)
            {
                tutorialanimated.enabled = true;
                pointerTutorial.SetActive(true);
            }
            tutorialanimated.Play("ShowPunch");
        }
        else if (punchTip == 1 && specialsTip == 0 && gold > price1 && !result)
        {
            if (!tutorialanimated.enabled)
            {
                tutorialanimated.enabled = true;
                pointerTutorial.SetActive(true);
            }
            tutorialanimated.Play("ShowBonuses");
            
        }

    }

    public void a1()
    {
        sounds[1].Play();
        player.swipesBlocked = true;
        if(a1bought == 1)
        {
            // already bought
        }
        else if (gold >= price1)
        {
            // text about activation
            gold -= price1;
            a1bought = 1;
            a1check();
            PlayerPrefs.SetInt("gold", gold);
            PlayerPrefs.SetInt("a1bought", a1bought);

            // tutorial check
            if (specialsTip == 0)
            {
                specialsTip = 1;
                tutorialCheck();
                PlayerPrefs.SetFloat("specialsTip", specialsTip);
            }
            PlayerPrefs.Save();
        }
        else
        {
            Time.timeScale = 0;
            errorScreen.SetActive(true);
            settingScreen.SetActive(false);
            menuScreen.SetActive(true);
        }
    }

    public void a1check()
    {
        if (a1bought == 1)
        {
            // trajectory changes
            player.specialPunch = true;
            iconbutton.color = activecolor;

            player.skill.SetActive(true);
        }
        else
        {
            player.specialPunch = false;
            iconbutton.color = Color.white;
        }
    }
    public void a2()
    {
        sounds[1].Play();
        player.swipesBlocked = true;
        if (gold >= price2)
        {
            // bonus shots
            shots += 5;
            gold -= price2;
            PlayerPrefs.SetInt("gold", gold);
            // tutorial check
            if (specialsTip == 0)
            {
                specialsTip = 1;
                tutorialCheck();
                PlayerPrefs.SetFloat("specialsTip", specialsTip);
            }
            PlayerPrefs.Save();
        }
        else
        {
            Time.timeScale = 0;
            errorScreen.SetActive(true);
            settingScreen.SetActive(false);
            menuScreen.SetActive(true);
        }
    }
    public void closeIt()
    {
        sounds[1].Play();
        player.swipesBlocked = true;
        Time.timeScale = 1;
        menuScreen.SetActive(false);
    }

    public void Settings()
    {
        sounds[1].Play();
        player.swipesBlocked = true;
        Time.timeScale = 0;
        errorScreen.SetActive(false);
        settingScreen.SetActive(true);
        menuScreen.SetActive(true);
    }

    public void ExitMenu()
    {
        sounds[1].Play();
        Time.timeScale = 1;
        asyncOperation.allowSceneActivation = true;
        loading.fillAmount = 0;
        loadingScreen.SetActive(true);
        loading.enabled = false;
    }
    public void reloadScene()
    {
        sounds[1].Play();
        Time.timeScale = 1;
        loading.fillAmount = 0;
        reloadThis = true;
        loadingScreen.SetActive(true);
    }
    public void barTap()
    {
        sounds[1].Play();
        player.swipesBlocked = true;
        Debug.Log("bartap");
        if (arrowmoving.activeSelf)
        {
            player.punching();
            arrowAnimator.enabled = false;
        }
        else
        {
            player.targetdot.SetParent(null);
            arrowmoving.SetActive(true);
            arrowAnimator.enabled = true;
            // tutorial check
            if (punchTip == 0)
            {
                punchTip = 1;
                PlayerPrefs.SetFloat("punchTip", punchTip);
                PlayerPrefs.Save();
                tutorialanimated.enabled = false;
                pointerTutorial.SetActive(false);
                tutorialText1.SetActive(false);
                tutorialText2.SetActive(false);
            }
        }

    }

    public void activeShot()
    {
        bar.SetActive(true);
        arrows.SetActive(true);
    }
    public void unactiveShot()
    {
        arrowmoving.SetActive(false);
    }
    public void Sound(bool volumeBool)
    {
        if (volumeBool)
        {
            volumeOn.SetActive(true);
            volumeOff.SetActive(false);
            volume = 1;
        }
        else
        {
            volume = 0;
            volumeOn.SetActive(false);
            volumeOff.SetActive(true);
        }

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void win()
    {
        if (!result)
        {
            result = true;
            //Time.timeScale = 0;
            Debug.Log("win");
            sounds[2].Play();

            gold += earnedGold;
            goldcoins.text = gold.ToString("0");

            winScreen.SetActive(true);
            if (chosenLevel > howManyLevelsDone)
            {
                PlayerPrefs.SetInt("howManyLevelsDone", (int)chosenLevel);
            }
            PlayerPrefs.SetInt("gold", gold);
            PlayerPrefs.Save();
        }
    }

    public void lose()
    {
        if (!result)
        {
            result = true;
            Time.timeScale = 0;
            Debug.Log("lose");
            loseScreen.SetActive(true);
        }
    }

    public void NextLevel()
    {
        sounds[1].Play();
        if (chosenLevel <= howManyLevelsDone + 1 && chosenLevel != levelMax)
        {
            chosenLevel += 1;
            mode += 1;
            if(mode > 10)
            {
                mode = 1;
            }


            PlayerPrefs.SetFloat("chosenLevel", chosenLevel);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
            reloadScene();
        }
    }

    void tutorialCheck()
    {
        if (specialsTip == 1)
        {
            tutorialanimated.enabled = false;
            pointerTutorial.SetActive(false);
            tutorialText1.SetActive(false);
            tutorialText2.SetActive(false);
        }
    }
}
