using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsScroller : MonoBehaviour
{
    
    public Transform transform;

    private Vector2 initialPosition;

    private Vector2 mousePosition;

    private float deltaX, deltaY;

    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        Debug.Log("click");
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        //deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Mathf.Abs(initialPosition.x) - Mathf.Abs(mousePosition.x - deltaX) < 4.5f && Mathf.Abs(initialPosition.x) - Mathf.Abs(mousePosition.x - deltaX) > 0)
        {
            transform.position = new Vector2(mousePosition.x - deltaX, initialPosition.y);
        }
    }

    private void OnMouseUp()
    {
        if (Mathf.Abs(transform.position.x - transform.position.x) <= 0.5)
        {
            transform.position = new Vector2(transform.position.x, initialPosition.y);
            //locked = true;
        }
        else
        {
            transform.position = new Vector2(initialPosition.x, initialPosition.y);
        }
    }

}
