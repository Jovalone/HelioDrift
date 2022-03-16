using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasBlock : MonoBehaviour
{
    public CanvasPart canvasPart;
    public void Setup()
    {
        canvasPart = transform.parent.GetComponent<CanvasPart>();

        PartPurchase.partPurchaseInstance.SetUp(canvasPart);
    }
}
