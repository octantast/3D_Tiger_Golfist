using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private float initialLaunch;

    public int howManyLevelsDone;
    public float chosenLevel;
    public int levelMax;

    public Color32 notenableButton;
    public Color32 enableButton;
    public List<Image> buttons; // levels
    public List<ButtonScript> buttonscripts;
    public GameObject leftarrow;
    public GameObject righarrow;

    public GameObject startButtons;
    public GameObject levelButtons;
    public GameObject loadingScreen;
    public GameObject fakeloadingScreen;
    public GameObject settings;
    public Image loading;

    // music
    private float volume;
    public AudioSource ambient;
    public AudioSource tapSound;
    public GameObject volumeOn;
    public GameObject volumeOff;

    // currency
    public TMP_Text currencyCount;
    private int gold;

    public Sprite unlockedLevel;
    public Sprite lockedLevel;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    void Start()
    {
        Time.timeScale = 1;
        initialLaunch = PlayerPrefs.GetFloat("initialLaunch");
        if (initialLaunch == 0)
        {
            PlayerPrefs.SetFloat("initialLaunch", 1);
            volume = 1;
            PlayerPrefs.SetFloat("volume", volume);
            PlayerPrefs.Save();
        }
        else
        {
            volume = PlayerPrefs.GetFloat("volume");
        }

        ambient.Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

      
        gold = PlayerPrefs.GetInt("gold");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");
        for (int i = 0; i <= howManyLevelsDone + 1; i++)
        {
            if (i < buttons.Count)
            {
                buttonscripts[i].thisImage.sprite = unlockedLevel;
                buttonscripts[i].thisLevelNumberText.gameObject.SetActive(true);
            }
        }
       

        currencyCount.text = gold.ToString("0");

       

        settings.SetActive(false);
        loadingScreen.SetActive(false);
        fakeloadingScreen.SetActive(true);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);

        asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        asyncOperation.allowSceneActivation = false;


    }

    private void Update()
    {

        if (loadingScreen.activeSelf == true)
        {
            ambient.volume -= 0.1f;
            tapSound.volume -= 0.1f;
            if (loading.fillAmount < 0.9f)
            {
                loading.fillAmount = Mathf.Lerp(loading.fillAmount, 1, Time.deltaTime * 2);
            }
            else
            {
                asyncOperation.allowSceneActivation = true;
            }
        }

    }
    public void StartGame(float mode)
    {
        // cycled
        playSound(tapSound);
        if (mode <= howManyLevelsDone + 1)
        {
            PlayerPrefs.SetInt("levelMax", levelMax);

            PlayerPrefs.SetFloat("chosenLevel", mode);

            // cycled
            for (int j = 1; j <= 10; j++)
            {
                for (int i = j; i <= levelMax; i += 10) // if 11, j = 1
                {
                    if (mode == i)
                    {
                        mode = j;
                    }
                    if (mode == 0)
                    {
                        mode = 1;
                    }
                }
            }

            // unique levels

            loading.fillAmount = 0;
            loadingScreen.SetActive(true);
            startButtons.SetActive(false);
            levelButtons.SetActive(false);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
        }
    }

    public void ShowLevels()
    {
        playSound(tapSound);
        loadingScreen.SetActive(false);
        startButtons.SetActive(false);
        levelButtons.SetActive(true);

        leftarrow.SetActive(false);
        righarrow.SetActive(true);

        foreach (ButtonScript button in buttonscripts)
        {
            button.thisLevelNumber = buttonscripts.IndexOf(button) + 1;
            buttonCheck(button);
        }
    }
    public void HideLevels()
    {
        playSound(tapSound);
        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);
    }
    public void Settings()
    {
        playSound(tapSound);
        if (!settings.activeSelf)
        {
            Time.timeScale = 0;
            HideLevels();
            startButtons.SetActive(false);
            settings.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            HideLevels();
            settings.SetActive(false);
        }
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
        ambient.volume = volume;
        tapSound.volume = volume;

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void playSound(AudioSource sound)
    {
        sound.Play();
    }



    public void leftArrow()
    {
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber -= 9;
            buttonCheck(button);
        }
        if (buttonscripts[0].thisLevelNumber <= 1)
        {
            leftarrow.SetActive(false);
        }
        righarrow.SetActive(true);

    }

    public void rightArrow()
    {
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber += 9;
            buttonCheck(button);
            if (button.thisLevelNumber > levelMax)
            {
                button.gameObject.SetActive(false);
                button.shadow.SetActive(false);
                righarrow.SetActive(false);
            }

        }
        leftarrow.SetActive(true);

    }

    public void buttonCheck(ButtonScript button)
    {
        button.thisLevelNumberText.text = button.thisLevelNumber.ToString("0");        
        button.gameObject.SetActive(true);
        button.shadow.SetActive(true);
        //if (howManyLevelsDone < buttonscripts[buttonscripts.Count - 1].thisLevelNumber)
        //{
        //    righarrow.SetActive(false);
        //}
        //else
        //{

        //    righarrow.SetActive(true);
        //}
        if (button.thisLevelNumber <= howManyLevelsDone + 1)
        {
            button.thisImage.sprite = unlockedLevel;
            button.thisLevelNumberText.gameObject.SetActive(true);
        }
        else
        {
            button.thisImage.sprite = lockedLevel;
            button.thisLevelNumberText.gameObject.SetActive(false);
        }
    }
}
