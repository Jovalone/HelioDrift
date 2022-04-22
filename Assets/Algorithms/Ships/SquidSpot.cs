using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidSpot : MonoBehaviour
{
    public Transform transform;
    public bool searching, full;
    public List<SquidSpawn> squidSpawns;
    public float dist;
    public GameObject follower;
    void Update()
    {
        if(squidSpawns.Count == 0)
        {
            squidSpawns.AddRange(FindObjectsOfType<SquidSpawn>());
        }

        if (!full)
        {
            Search();
        }
        {
            if(follower == null)
            {
                full = false;
            }
        }
    }

    //IEnumerator Search()
    void Search()
    {
        searching = true;

        foreach(SquidSpawn spawn in squidSpawns)
        {
            if (Vector3.Distance(transform.position, spawn.transform.position) < dist && spawn.members.Count != 0)
            {
                int i = 0;
                while (!full && spawn.members.Count != 0)
                {
                    if(!spawn.members[i].Leader)
                    {
                        spawn.members[i].follow = true;
                        spawn.members[i].Target = transform;
                        spawn.members.Remove(spawn.members[i]);
                        follower = spawn.members[i].gameObject;
                        full = true;
                    }
                    i++;
                }
            }
        }
        //yield return new WaitForSeconds(0.1f);
        searching = false;
    }
}
