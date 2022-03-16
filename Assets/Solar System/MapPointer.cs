using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPointer : MonoBehaviour
{
    public GameObject mapPointer;
    public GameObject currPointer;
    public miniMapPoint miniPoint;
    public Camera camera;
    Vector3 objectPos;
    Vector3 oldPosition;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(objectPos != null)
            {
                oldPosition = objectPos;
            }

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 110f;       // we want 2m away from the camera position
            objectPos = camera.ScreenToWorldPoint(mousePos);

            if(currPointer != null)//destroy old object
            {
                Destroy(currPointer);
            }

            currPointer = Instantiate(mapPointer, objectPos, Quaternion.identity);
            miniPoint.pointer = currPointer.transform;
        }
    }

    public void Undo()
    {
        if (currPointer != null)//destroy old object
        {
            Destroy(currPointer);

            currPointer = Instantiate(mapPointer, oldPosition, Quaternion.identity);
            miniPoint.pointer = currPointer.transform;
        }
    }
}
