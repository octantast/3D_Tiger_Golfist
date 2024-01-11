using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    //private AsyncOperation asyncOperation;
    public Image loading;
    void Start()
    {
        //asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        //asyncOperation.allowSceneActivation = false;
    }

    private void Update()
    {

        if (loading.fillAmount < 0.9f)
        {
            loading.fillAmount = Mathf.Lerp(loading.fillAmount, 1, Time.deltaTime * 2);
        }
        else
        {
            // asyncOperation.allowSceneActivation = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
