using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;

    public Transform Camera;


    // Update is called once per frame
    void Update()
    {
        Camera.position = new Vector3(Player.position.x, Player.position.y, -2);
    }
}
