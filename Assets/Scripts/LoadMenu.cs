using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    //private AsyncOperation asyncOperation;
    public Image loading;

    public float fillDuration = 8f;

    void Start()
    {
        if (loading != null)
        {
            StartCoroutine(FillImage());
        }
    }

    IEnumerator FillImage()
    {
        float timer = 0;
        while (timer < fillDuration)
        {
            loading.fillAmount = timer / fillDuration;
            timer += Time.deltaTime;
            yield return null;
        }

        loading.fillAmount = 1;

        // Function to call after the image is filled
        PerformFunctionAfterFill();
    }

    void PerformFunctionAfterFill()
    {
        SceneManager.LoadScene("MainMenu");
    }
}