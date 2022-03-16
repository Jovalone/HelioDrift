using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class Store : MonoBehaviour
{

    private Transform player;
    public Canvas canvas, StoreCanvas;
    public float minDist;
    public Text enterShop;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StoreCanvas.enabled = false;
        canvas.enabled = true;
    }

    public void Leave()
    {
        Time.timeScale = 1f;
        StoreCanvas.enabled = false;
        canvas.enabled = true;
    }

    public void OpenStore()
    {
        Debug.Log(this.gameObject.name);
        StoreCanvas.enabled = true;
        canvas.enabled = false;
        Time.timeScale = 0f;
    }



    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("OpenStore", this, SymbolExtensions.GetMethodInfo(() => OpenStore()));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("OpenStore");
    }
}
