using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBlock : MonoBehaviour
{
    [SerializeField]
    public Transform partPos;
    public Transform partTran;

    private Vector2 initialPosition;

    private Vector2 mousePosition;

    private float deltaX, deltaY;

    public static bool locked;

    public SpriteRenderer sprite;

    private void OnMouseDown()
    {
        Debug.Log("clicked_block");
        initialPosition = transform.position;
        partPos.parent = null;

        if (!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - partTran.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - partTran.position.y;
        }
        PartRotate.partRotateInstance.part = partPos.gameObject;

        partPos.gameObject.GetComponent<EnginePart>().changeLayerOrder(2);
        CircuitGrid.circuitGrid.fixedParts.Remove(partPos.gameObject.GetComponent<EnginePart>());
        CircuitGrid.circuitGrid.updatePartValues(partPos.gameObject.GetComponent<EnginePart>());
    }

    private void OnMouseDrag()
    {
        if (!locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            partTran.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
        }
    }

    private void OnMouseUp()
    {
        if(Mathf.Abs(partTran.position.x - partPos.position.x) <= 0.5 && Mathf.Abs(partTran.position.y - partPos.position.y) <= 0.5)
        {
            partTran.position = new Vector2(Mathf.Round(partPos.position.x), Mathf.Round(partPos.position.y));
            //locked = true;
        }
        else
        {
            partTran.position = new Vector2(initialPosition.x, initialPosition.y);
        }
        CircuitGrid.circuitGrid.CheckPlacement(partPos.gameObject.GetComponent<EnginePart>());
        CircuitGrid.circuitGrid.updatePartValues(partPos.gameObject.GetComponent<EnginePart>());
        partPos.gameObject.GetComponent<EnginePart>().changeLayerOrder(1);
    }
}
