using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Pilot_Follow : MonoBehaviour
{
    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("Follow", this, SymbolExtensions.GetMethodInfo(() => Follow()));
    }

    public void Follow()
    {
        Stats.statsInstance.Followers++;
    }
}
