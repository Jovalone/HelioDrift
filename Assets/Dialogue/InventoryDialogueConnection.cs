using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class InventoryDialogueConnection : MonoBehaviour
{
    public Inventory inventory;
    public bool unregisterOnDisable = false;

    public bool hasItem(double item)
    {
        if(inventory.inventory[(int)item] > 0)
        {
            Debug.Log("has item");
            //inventory.inventory[(int)item] -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void takeItem(double item)
    {
        inventory.inventory[(int)item] -= 1;
    }

    public void giveMoney(double amount)
    {
        inventory.inventory[0] += (int)amount;
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("hasItem", this, SymbolExtensions.GetMethodInfo(() => hasItem((double)0)));
        Lua.RegisterFunction("giveMoney", this, SymbolExtensions.GetMethodInfo(() => giveMoney((double)0)));
        Lua.RegisterFunction("takeItem", this, SymbolExtensions.GetMethodInfo(() => takeItem((double)0)));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction("hasItem");
            Lua.UnregisterFunction("takeItem");
            Lua.UnregisterFunction("givemoney");
        }
    }
}
