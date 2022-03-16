using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class LeaveStation : MonoBehaviour
{
    public float minDist;
    public Text leaveShop;
    private Transform player;
    private Inventory inventory;
    Stats stats;
    public int sceneToLoad;
    public string s;
    //public SaveSystemMethods SaveSystem;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void Update()
    {
        if (Vector2.Distance((Vector2)player.position, (Vector2)this.gameObject.transform.position) < minDist)
        {
            leaveShop.enabled = true;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadScene();
            }
        }
        else
        {
            leaveShop.enabled = false;
        }
    }

    public void LoadScene()
    {
        //Statistics info
        RecordKeeper.Record.saveInfo();

        //SaveSystem.SaveSlot(0);
        stats = GameObject.Find("Inventory").transform.GetChild(0).gameObject.GetComponent<Stats>();
        stats.SaveAmmo();
        inventory.SaveInventory();
        SceneManager.LoadScene(sceneToLoad);
        //PixelCrushers.SaveSystem.LoadScene(s);
    }
}
