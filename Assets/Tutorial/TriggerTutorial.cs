using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerTutorial : MonoBehaviour
{
    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("startTutorial", this, SymbolExtensions.GetMethodInfo(() => startTutorial()));
    }

    public void startTutorial()
    {
        Stats.statsInstance.tutorial = true;
    }
}
