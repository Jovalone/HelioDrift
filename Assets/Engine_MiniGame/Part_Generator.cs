using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part
{
    public int Size;
    public int[,] Layout;
    public int edgeSize;
    public int partType;
    public int rotations;
    public bool Fixed;
}

public class Part_Generator : MonoBehaviour
{
    private int edgeSize;
    private int Size;
    public int[,] part;
    public int n, m, l, k;

    public Part generatePart(int size, int edge, int type)
    {
        Part Part_final = new Part();
        edgeSize = edge;
        Part_final.partType = type;
        Part_final.Size = size;
        part = new int[edgeSize, edgeSize];
        part[edgeSize/2, edgeSize/2] = 1;
        n = 0; // counts how many need to be skipped before arriving at the desired part
        m = 0; // counts how many blocks have been placed
        l = 3; // counts how many bordering sides are viable
        k = 0;

        int exit = 0;

        while(size > m + k)//ends if part is stuck in loop
        {
            exit++;
            if(exit > 10000)//      || edgeSize^2 < m + k)
            {
                Part_final.edgeSize = edgeSize;
                Part_final.Layout = part;
                Part_final.Size = size;

                return Part_final;
            }

            n = Random.Range(0,m);// randomise which part will be extended

            for(int i = 0; i < edgeSize; i++)
            {
                for(int j = 0; j < edgeSize; j++)
                {
                    if(part[i, j] == 1)
                    {
                        if(n == 0)
                        {
                            //attempt to spawn a new block from here
                            //check how many of the surrounding blocks are viable

                            l = 3; // counts how many bordering sides are viable

                            //Check to the left
                            if (i == 0)
                            {
                                l--;
                            }
                            else if (part[i - 1,j] != 0)
                            {
                                l--;
                            }

                            //Check Above
                            if (j == 0)
                            {
                                l--;
                            }
                            else if (part[i, j - 1] != 0)
                            {
                                l--;
                            }

                            //Check to the right
                            if (i == edgeSize - 1)
                            {
                                l--;
                            }
                            else if (part[i + 1, j] != 0)
                            {
                                l--;
                            }

                            //Check below
                            if (j == edgeSize - 1)
                            {
                                l--;
                            }
                            else if (part[i, j + 1] != 0)
                            {
                                l--;
                            }


                            if(l == -1)
                            {
                                //no viable edges
                                part[i, j] = 2;// mark so not checked again
                                //n--;
                                m--;
                                k++;
                            }
                            else
                            {
                                //place the new block
                                int k = Random.Range(0, l);//choose a random edge
                                m++;

                                //Check Left
                                if (i != 0 && part[i - 1, j] == 0)
                                {
                                    if(k == 0)
                                    {
                                        part[i - 1, j] = 1;
                                    }
                                    k--;
                                }

                                //Check Up
                                if (j != 0 && part[i, j - 1] == 0)
                                {
                                    if (k == 0)
                                    {
                                        part[i, j - 1] = 1;
                                    }
                                    k--;
                                }

                                //Check Right
                                if (i != edgeSize - 1 && part[i + 1, j] == 0)
                                {
                                    if (k == 0)
                                    {
                                        part[i + 1, j] = 1;
                                    }
                                    k--;
                                }

                                //Check Down
                                if (j != edgeSize - 1 && part[i, j + 1] == 0)
                                {
                                    if (k == 0)
                                    {
                                        part[i, j + 1] = 1;
                                    }
                                    k--;
                                }
                            }
                        }
                        n--;
                    }
                }
            }
        }
        /*
        Debug.Log(size);
        Debug.Log("layout: ");
        for (int row = 0; row < edgeSize; row++)
        {
            Debug.Log(part[0, row] + " " + part[1, row] + " " + part[2, row] + " " + part[3, row] + " " + part[4, row]);
            
        }*/

        createPosNeg();
        Part_final.edgeSize = edgeSize;
        Part_final.Layout = part;
        Part_final.Size = size;

        return Part_final;
    }

    public void createPosNeg()
    {
        bool placedPos = false;
        int exit = 0;

        while (!placedPos)
        {
            exit++;
            if (exit > 10000)
            {
                placedPos = true;
            }

            n = Random.Range(0, m);
            for (int i = 0; i < edgeSize; i++)
            {
                for (int j = 0; j < edgeSize; j++)
                {
                    if (part[i, j] == 1)
                    {
                        if (n == 0)
                        {
                            //check if it has any bordering edges
                            l = 3; // counts how many bordering sides are viable

                            //Check to the left
                            if (i != 0 && part[i - 1, j] != 0)
                            {
                                l--;
                            }

                            //Check Above
                            if (j != 0 && part[i, j - 1] != 0)
                            {
                                l--;
                            }

                            //Check to the right
                            if (i != edgeSize - 1 && part[i + 1, j] != 0)
                            {
                                l--;
                            }

                            //Check below
                            if (j != edgeSize - 1 && part[i, j + 1] != 0)
                            {
                                l--;
                            }


                            if (l == -1)
                            {
                                //no viable edges
                                part[i, j] = 2;
                                m--;
                                k++;
                            }
                            else
                            {
                                part[i, j] = 3;
                                m--;
                                k++;

                                placedPos = true;
                            }
                        }
                        n--;
                    }
                }
            }
        }

        bool placedNeg = false;
        exit = 0;

        while (!placedNeg)
        {
            exit++;
            if (exit > 10000)
            {
                placedNeg = true;
            }

            n = Random.Range(0, m);
            for (int i = 0; i < edgeSize; i++)
            {
                for (int j = 0; j < edgeSize; j++)
                {
                    if (part[i, j] == 1)
                    {
                        if (n == 0)
                        {
                            //check if it has any bordering edges
                            l = 3; // counts how many bordering sides are viable

                            //Check to the left
                            if (i != 0 && part[i - 1, j] != 0)
                            {
                                l--;
                            }

                            //Check Above
                            if (j != 0 && part[i, j - 1] != 0)
                            {
                                l--;
                            }

                            //Check to the right
                            if (i != edgeSize - 1 && part[i + 1, j] != 0)
                            {
                                l--;
                            }

                            //Check below
                            if (j != edgeSize - 1 && part[i, j + 1] != 0)
                            {
                                l--;
                            }


                            if (l == -1)
                            {
                                //no viable edges
                                part[i, j] = 2;
                                m--;
                                k++;
                            }
                            else
                            {
                                part[i, j] = 4;
                                m--;
                                k++;

                                placedNeg = true;
                            }
                        }
                        n--;
                    }
                }
            }
        }
    }
}
