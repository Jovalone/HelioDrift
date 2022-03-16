using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveScene : MonoBehaviour
{
    public int newScene;
    Stats stats;
    Engine engine;
    Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void leave()
    {
        //SaveSystem.SaveSlot(0);
        stats = GameObject.Find("Inventory").transform.GetChild(0).gameObject.GetComponent<Stats>();
        stats.SaveAmmo();
        engine = GameObject.Find("Inventory").transform.GetChild(1).gameObject.GetComponent<Engine>();
        if(engine != null)
        {
            engine = GameObject.Find("Inventory").transform.GetChild(1).gameObject.GetComponent<Engine>();
        }
        engine.saveParts();
        inventory.SaveInventory();
        SceneManager.LoadScene(newScene);
    }
}
