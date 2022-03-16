using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public static bool paused = false;

	public GameObject pauseMenu, camera, Map;
	public GameObject Player;
	public bool Base;
	public Button MapReturn;

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (paused)
			{
				Resume();
			}
			else
			{
				Pause();
			}

		}

	}

	public void Resume()
	{
		if (Map != null && Map.activeSelf)
		{
			MapReturn.onClick.Invoke();
		}
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		paused = false;
		Player.SetActive(true);
	}

	void Pause()
	{
        if (!Base)
        {
			if (Time.timeScale == 1f)
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
				paused = true;
				Player.SetActive(false);
			}
        }
        else
        {
			if (Time.timeScale == 1f)
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
				paused = true;
			}
		}
	}

    public void LoadMenu()
	{
		Time.timeScale = 1f;
		Stats.statsInstance.SceneToLoad = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(0);
	}

    public void Quit()
	{
		Debug.Log("Quitting Game");
		Application.Quit();
	}
}
