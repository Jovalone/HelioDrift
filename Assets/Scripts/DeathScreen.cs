using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public Stats stats;
    public Inventory inventory;
    public SolarSystem solarSystem;
    public GameObject Player;

    void Start()
    {
        stats = GameObject.Find("Stats").GetComponent<Stats>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void MainMenu()
	{
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Retry()
	{
        //stats.Ammo = stats.lastSaveAmmo;
        //inventory.inventory = inventory.lastSaveInventory;
       // Player.transform.position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //stats.SetUp();
       // Debug.Log("test");
        Time.timeScale = 1f;
    }
}
