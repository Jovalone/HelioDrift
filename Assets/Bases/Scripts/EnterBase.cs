using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterBase : MonoBehaviour
{
    public float dist;
    public Transform transform;
    public Transform player;
    public Text text;
    private bool saving = false;
    public SolarSystems solarSystems;
    public int SceneToEnter;
    public bool current;
    public Stats stats;

    void Start()
    {
        solarSystems = GameObject.Find("SolarSystems").GetComponent<SolarSystems>();
        player = GameObject.Find("Player").transform;
        text = GameObject.Find("EnterBase").GetComponent<Text>();
        stats = GameObject.Find("Stats").GetComponent<Stats>();
        text.enabled = false;
        current = false;
    }

    void Update()
    {
        if(stats == null)
        {
            //stats = GameObject.Find("Stats").GetComponent<Stats>();
            stats = GameObject.Find("Inventory").transform.GetChild(0).GetComponent<Stats>();
        }

        if (Vector3.Distance(transform.position, player.position) < dist)
        {
            current = true;
            text.enabled = true;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                solarSystems.UpdateScene();
                stats.SceneToLoad = SceneToEnter;
                SceneManager.LoadScene(SceneToEnter);
            }
        }
        else if(current)
        {
            current = false;
            text.enabled = false;
        }
    }
}
