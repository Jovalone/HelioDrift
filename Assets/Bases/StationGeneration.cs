using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationGeneration : MonoBehaviour
{
    public Transform transform;

    public GameObject[] Tiles;
    public GameObject[] Walls;

    public int stationSize;
    public int roomSize;
    public int[,] floorPlan;
    public int[,] TilePlan;
    public int[,] WallPlan;

    public float DoubleRoomChance;

    public List<WallBlock> wallsLink;

    void Start()
    {
        floorPlan = new int[stationSize, stationSize];
        TilePlan = new int[stationSize * (roomSize + 1), stationSize * (roomSize + 1)];
        WallPlan = new int[stationSize * (roomSize + 1) + 1, stationSize * (roomSize + 1)];

        //generate a random room placement. with 0 meaning no room present
        for (int i = 0; i < stationSize; i++)
        {
            for (int j = 0; j < stationSize; j++)
            {
                floorPlan[i, j] = Random.Range(0, 2);
            }
        }

        //create a minimum station size
        floorPlan[0, 1] = 1;
        floorPlan[1, 1] = 1;

        //delete rooms without borders and select potential double rooms
        for (int i = 0; i < stationSize; i++)
        {
            for (int j = 0; j < stationSize; j++)
            {
                if (floorPlan[i, j] > 0)
                {
                    floorPlan[i, j] = checkBorders(i, j);
                }
            }
        }

        //create rooms and join double rooms
        for (int i = 0; i < stationSize; i++)
        {
            for (int j = 0; j < stationSize; j++)
            {
                if (floorPlan[i, j] == 2)
                {
                    CreateDoubleRooms(i, j);
                }

                if (floorPlan[i, j] > 0)
                {
                    for (int k = 0; k < roomSize; k++)
                    {
                        for (int l = 0; l < roomSize; l++)
                        {
                            TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 1;
                            if(l == (roomSize + 1)/ 2 - 1)
                            {
                                TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 3;
                                if (k == (roomSize + 1) / 2 - 1)
                                {
                                    TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 4;
                                }
                            }
                            else if (l == ((roomSize + 1) / 2 - 1) / 2 || l == (roomSize + 1) / 2 - 1 + ((roomSize + 1) / 2 - 1) / 2)
                            {
                                TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 2;
                            }
                            else if (k == ((roomSize + 1) / 2 - 1) / 2 || k == (roomSize + 1) / 2 - 1 + ((roomSize + 1) / 2 - 1) / 2)
                            {
                                TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 2;
                            }
                            else if(k == (roomSize + 1) / 2 - 1)
                            {
                                TilePlan[j * (roomSize + 1) + l, i * (roomSize + 1) + k] = 3;
                            }
                        }
                    }
                }
            }
        }

        //create paths between rooms
        for (int i = 0; i < stationSize; i++)
        {
            for (int j = 0; j < stationSize; j++)
            {
                if (floorPlan[i, j] > 0)
                {
                    CreatePaths(i, j);
                }
            }
        }
        CreateWalls();

        instantiateTiles();
    }

    //check borders function
    int checkBorders(int i, int j)
    {
        int Count = 0;
        if (i != 0)
        {
            if (floorPlan[i - 1, j] > 0)
            {
                Count++;
            }
        }
        if (i != stationSize - 1)
        {
            if (floorPlan[i + 1, j] > 0)
            {
                Count++;
            }
        }
        if (j != 0)
        {
            if (floorPlan[i, j - 1] > 0)
            {
                Count++;
            }
        }
        if (j != stationSize - 1)
        {
            if (floorPlan[i, j + 1] > 0)
            {
                Count++;
            }
        }

        if (Count == 0)
        {
            return 0;
        }
        else if (Count > 1)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    //create double room function
    void CreateDoubleRooms(int i, int j)
    {
        bool doubled = false;
        if (i != 0)
        {
            if(DoubleRoomChance < Random.Range(0f, 100f))
            {
                if (floorPlan[i - 1, j] > 0 && floorPlan[i - 1, j] < 3)
                {
                    floorPlan[i, j] = 4;
                    floorPlan[i - 1, j] = 3;
                    doubled = true;
                    for (int x = 0; x < roomSize; x++)
                    {
                        TilePlan[j * (roomSize + 1) + x, i * (roomSize + 1) - 1] = 1;
                    }
                }
            }
        }

        if (i != stationSize - 1 && !doubled)
        {
            if (DoubleRoomChance < Random.Range(0f, 100f))
            {
                if (floorPlan[i + 1, j] > 0 && floorPlan[i + 1, j] < 3)
                {
                    floorPlan[i, j] = 4;
                    floorPlan[i + 1, j] = 3;
                    doubled = true;
                    for (int x = 0; x < roomSize; x++)
                    {
                        TilePlan[j * (roomSize + 1) + x, (i + 1) * (roomSize + 1) - 1] = 1;
                    }
                }
            }
        }

        if (j != 0 && !doubled)//creates no double links for some reason
        {
            if (DoubleRoomChance < Random.Range(0f, 100f))
            {
                if (floorPlan[i, j - 1] > 0 && floorPlan[i, j - 1] < 3)
                {
                    floorPlan[i, j] = 4;
                    floorPlan[i, j - 1] = 3;
                    doubled = true;
                    for (int x = 0; x < roomSize; x++)
                    {
                        TilePlan[j * (roomSize + 1) - 1, i * (roomSize + 1) + x] = 1;
                    }
                }
            }
        }

        if (j != stationSize - 1 && !doubled)
        {
            if (DoubleRoomChance < Random.Range(0f, 100f))
            {
                if (floorPlan[i, j + 1] > 0 && floorPlan[i, j + 1] < 3)
                {
                    floorPlan[i, j] = 4;
                    floorPlan[i, j + 1] = 3;
                    doubled = true;
                    for (int x = 0; x < roomSize; x++)
                    {
                        TilePlan[(j + 1) * (roomSize + 1) - 1, i * (roomSize + 1) + x] = 1;
                    }
                }
            }
        }
    }

    //create paths function
    void CreatePaths(int i, int j)
    {
        bool mainDouble = false;
        if (floorPlan[i, j] == 4)
        {
            mainDouble = true;
        }

        if (i != 0)
        {
            if (floorPlan[i - 1, j] > 0 && floorPlan[i - 1, j] < 4)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) + roomSize / 2, i * (roomSize + 1) - 1] = 1;
            }
            else if (floorPlan[i - 1, j] == 4 && mainDouble)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) + roomSize / 2, i * (roomSize + 1) - 1] = 1;
            }
        }

        if (i != stationSize - 1)
        {
            if (floorPlan[i + 1, j] > 0 && floorPlan[i + 1, j] < 4)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) + roomSize / 2, (i + 1) * (roomSize + 1) - 1] = 1;
            }
            else if (floorPlan[i + 1, j] == 4 && mainDouble)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) + roomSize / 2, (i + 1) * (roomSize + 1) - 1] = 1;
            }
        }

        if (j != 0)
        {
            if (floorPlan[i, j - 1] > 0 && floorPlan[i, j - 1] < 4)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) - 1, i * (roomSize + 1) + roomSize / 2] = 1;
            }
            else if (floorPlan[i, j - 1] == 4 && mainDouble)
            {
                floorPlan[i, j] = 0;
                TilePlan[j * (roomSize + 1) - 1, i * (roomSize + 1) + roomSize / 2] = 1;
            }
        }

        if (j != stationSize - 1)
        {
            if (floorPlan[i, j + 1] > 0 && floorPlan[i, j + 1] < 4)
            {
                floorPlan[i, j] = 0;
                TilePlan[(j + 1) * (roomSize + 1) - 1, i * (roomSize + 1) + roomSize / 2] = 1;
            }
            else if (floorPlan[i, j + 1] == 4 && mainDouble)
            {
                floorPlan[i, j] = 0;
                TilePlan[(j + 1) * (roomSize + 1) - 1, i * (roomSize + 1) + roomSize / 2] = 1;
            }
        }
    }

    void CreateWalls()
    {
        for (int i = 0; i < stationSize * (roomSize + 1); i++)
        {
            for (int j = 0; j < stationSize * (roomSize + 1); j++)
            {

                //Place Double walls
                if (i != 0 && j != 0 && j != stationSize * (roomSize + 1) - 1)
                {
                    if (TilePlan[j, i] == 0 && TilePlan[j - 1, i] != 0 && TilePlan[j - 1, i - 1] != 0 && TilePlan[j + 1, i] != 0 && TilePlan[j + 1, i - 1] != 0)
                    {
                        WallPlan[j, i] = 18;
                    }
                }

                //Spawn wall corner joints
                if (j != 0 && j < stationSize * (roomSize + 1) - 1 && i != 0)//right side
                {
                    if (TilePlan[j, i] == 0 && TilePlan[j + 1, i] == 0 && TilePlan[j + 1, i - 1] != 0 && TilePlan[j - 1, i] != 0)
                    {
                        WallPlan[j, i] = 19;
                    }

                    if (TilePlan[j, i] == 0 && TilePlan[j - 1, i] == 0 && TilePlan[j - 1, i - 1] != 0 && TilePlan[j + 1, i] != 0)
                    {
                        WallPlan[j, i] = 20;
                    }
                }

                //Place inner walls
                if (i != 0)
                {
                    if (TilePlan[j, i] == 0 && TilePlan[j, i - 1] != 0)
                    {
                        WallPlan[j, i] = 1;
                    }
                }

                //Double Corner
                if (j != 0 && j != stationSize * (roomSize + 1) - 1 && i != 0)
                {
                    if (TilePlan[j - 1, i] == 0 && TilePlan[j - 1, i - 1] != 0 && TilePlan[j + 1, i] == 0 && TilePlan[j + 1, i - 1] != 0)
                    {
                        if (TilePlan[j, i] == 0 && TilePlan[j, i - 1] == 0)
                        {
                            WallPlan[j, i] = 21;
                        }
                    }
                }

                //Spawn top left corners
                if (j != stationSize * (roomSize + 1) - 1 && i != 0 && WallPlan[j, i] == 0)//stationSize * (roomSize + 1) - 1)
                {
                    if (TilePlan[j, i] == 0 && TilePlan[j, i - 1] == 0 && TilePlan[j + 1, i] == 0 && TilePlan[j + 1, i - 1] != 0)
                    {
                        WallPlan[j, i] = 16;
                    }
                }

                //Spawn top right corners
                if (j != 0 && i != 0 && WallPlan[j, i] == 0)
                {
                    if (TilePlan[j, i] == 0 && TilePlan[j, i - 1] == 0 && TilePlan[j - 1, i] == 0 && TilePlan[j - 1, i - 1] != 0)
                    {
                        WallPlan[j, i] = 17;
                    }
                }

                //Spawn right walls
                if (j != 0 && i != 0 && TilePlan[j, i - 1] == 0 && TilePlan[j - 1, i - 1] != 0 && WallPlan[j, i] == 0)
                {
                    WallPlan[j, i] = 14;
                }

                //Spawn left walls
                if (i != 0 && j != stationSize * (roomSize + 1) - 1 && WallPlan[j, i] == 0 && TilePlan[j, i - 1] == 0 && TilePlan[j + 1, i - 1] != 0)
                {
                    WallPlan[j, i] = 15;
                }

                //Place outerWalls
                if (i == 0)
                {
                    if (TilePlan[j, i] != 0)
                    {
                        WallPlan[j, i] = 7;
                    }
                }
                else
                {
                    if (TilePlan[j, i] != 0 && TilePlan[j, i - 1] == 0)
                    {
                        if (j != stationSize * (roomSize + 1) - 1 && TilePlan[j + 1, i] != 0 && TilePlan[j + 1, i - 1] != 0)//inverse left
                        {
                            WallPlan[j, i] = 12;//this one is wrong but might just ignore
                            if (j != 0 && TilePlan[j - 1, i] != 0 && TilePlan[j - 1, i - 1] != 0)//inverse right
                            {
                                WallPlan[j, i] = 13;
                            }
                        }
                        else if (j != 0 && TilePlan[j - 1, i] != 0 && TilePlan[j - 1, i - 1] != 0)//inverse right
                        {
                            WallPlan[j, i] = 11;
                        }
                        else
                        {
                            WallPlan[j, i] = 7;
                        }
                    }
                }
            }
        }

        //Bottom wall corner business
        for (int i = 0; i < stationSize * (roomSize + 1); i++)
        {
            for (int j = 0; j < stationSize * (roomSize + 1); j++)
            {
                if (j != stationSize * (roomSize + 1) - 1)
                {
                    if(WallPlan[j + 1, i] > 6 && WallPlan[j + 1, i] < 11 && TilePlan[j, i] == 0)
                    {
                        if(WallPlan[j, i] != 0)
                        {
                            WallPlan[j, i] = 24;
                        }
                        else
                        {
                            WallPlan[j, i] = 22;
                        }
                    }
                }
            }

            for (int j = 0; j < stationSize * (roomSize + 1); j++)
            {
                if (j != 0)
                {
                    if (WallPlan[j - 1, i] > 6 && WallPlan[j - 1, i] < 11 && TilePlan[j, i] == 0)
                    {
                        if (WallPlan[j, i] != 0)
                        {
                            if(WallPlan[j, i] == 22)
                            {
                                WallPlan[j, i] = 26;
                            }
                            else
                            {
                                Debug.Log(WallPlan[j, i]);
                                WallPlan[j, i] = 25;
                            }
                        }
                        else
                        {
                            WallPlan[j, i] = 23;
                        }
                    }
                }
            }
        }
    }

    void instantiateTiles()
    {
        bool wall = false;
        int k = 0;
        int l = 0;

        for (int i = 0; i < stationSize * (roomSize + 1); i++)
        {
            for (int j = 0; j < stationSize * (roomSize + 1); j++)
            {
                //Spawn floors
                if (TilePlan[j, i] != 0)
                {
                    Instantiate(Tiles[TilePlan[j, i] - 1], transform.position + new Vector3(j, i, i), Quaternion.identity, transform);
                }

                //Spawn Walls
                if (WallPlan[j, i] != 0)
                {
                    if(WallPlan[j, i] > 6 && WallPlan[j, i] < 14)
                    {
                        if (!wall)
                        {
                            wall = true;
                            wallsLink = new List<WallBlock>();
                            wallsLink.Clear();
                            k = i;
                            l = j;
                        }

                        if (k != i || l + 1 != j)
                        {
                            //link walls
                            foreach (WallBlock wall_0 in wallsLink)
                            {
                                wall_0.blocks = new List<WallBlock>();

                                foreach (WallBlock wall_1 in wallsLink)
                                {
                                    wall_0.blocks.Add(wall_1);
                                }
                            }
                            wallsLink = new List<WallBlock>();
                            wallsLink.Clear();
                            k = i;
                            l = j;
                        }
                        else
                        {
                            l++;
                        }

                        GameObject temp = (GameObject)Instantiate(Walls[WallPlan[j, i] - 1], transform.position + new Vector3(j, i, 0), Quaternion.identity, transform);
                        wallsLink.Add(temp.GetComponent<WallBlock>());

                        //far left corners
                        if (j == 0)
                        {
                            Debug.Log("left corner");
                            Instantiate(Walls[21], transform.position + new Vector3(j - 1, i, 0), Quaternion.identity, transform);
                        }
                    }
                    else
                    {
                        if (wall = true)
                        {
                            //link walls
                            foreach (WallBlock wall_0 in wallsLink)
                            {
                                wall_0.blocks = new List<WallBlock>();

                                foreach (WallBlock wall_1 in wallsLink)
                                {
                                    wall_0.blocks.Add(wall_1);
                                }
                            }
                            wallsLink = new List<WallBlock>();
                            wallsLink.Clear();
                        }
                        wall = false;

                        Instantiate(Walls[WallPlan[j, i] - 1], transform.position + new Vector3(j, i, 0), Quaternion.identity, transform);

                        //far top left corner
                        if (WallPlan[j, i] > 0 && WallPlan[j, i] < 7 && j == 0)
                        {
                            Instantiate(Walls[15], transform.position + new Vector3(j - 1, i, 0), Quaternion.identity, transform);
                        }
                    }
                }
                else if (j == 0)
                {
                    //far left wall edges
                    if(TilePlan[j, i] != 0)
                    {
                        Instantiate(Walls[14], transform.position + new Vector3(j - 1, i, 0), Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}

