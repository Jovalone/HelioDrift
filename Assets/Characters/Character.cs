using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Character : MonoBehaviour
{
    public int CharacterNum;//place in character  array

    public int Honour;//likely hood to betray
    public int Intelligence;//Importance of Opinion and Honour when there is a lot to gain
    public int Opinion;//Character opinion of player
    public int OpinionRate;//How quickly opinion changes

    public int[] relationships;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
