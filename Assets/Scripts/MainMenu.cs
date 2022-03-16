using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject menu;
    public GameObject LoadingInterface;
    public Image LoadingProgressBar;
    AsyncOperation operation;
    public Animator animator;
    public float waitTime;
    public Stats stats;
    public int SceneToLoad;
    public CanvasAnimator_Stages stage;

    void Start()
    {
        if(GameObject.Find("Stats") != null)
        {
            stats = GameObject.Find("Stats").GetComponent<Stats>();
            SceneToLoad = stats.SceneToLoad;
        }
        else
        {
            SceneToLoad = 2;
        }
    }


    public void PlayGame()
	{
        HideMenu();
        ShowLoadingScreen();
        StartCoroutine(FadeMusic());
	}

    public void PressedPlay()
    {
        stage.stage++;
    }

    public void QuitGame()
	{
        Application.Quit();
        Debug.Log("Quit");
	}

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        LoadingInterface.SetActive(true);
    }

    IEnumerator FadeMusic()
    {
        LoadingProgressBar.fillAmount = 0;
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitTime);

        //operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //operation = SceneManager.LoadSceneAsync(SceneToLoad);
        //StartCoroutine(LoadingScreen());
        float totalProgress = 0f;
        operation = SceneManager.LoadSceneAsync(SceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            LoadingProgressBar.fillAmount = progress;
            //Debug.Log(progress);
            yield return null;
        }
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0f;
        operation = SceneManager.LoadSceneAsync(SceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            LoadingProgressBar.fillAmount = progress;
            Debug.Log(progress);
            yield return null;
        }
        //return;
    }
}
