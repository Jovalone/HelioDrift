using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowParticle
{
    public Vector3 moveVel;
    public Vector2 position;
}

public class PerlinVectorFlowField : MonoBehaviour
{
    public int size;
    FastNoiseLite fastNoise;
    public int particleNum;
    public float depth;
    public float frequency;
    public float offX, offY;
    public int Lifetime;
    public Vector3[,] Forces;
    public int thickness;
    public int length;


    public GameObject arrow, spot;
    public static PerlinVectorFlowField flowField;

    void Start()
    {

        flowField = this;

        FastNoiseLite noise = new FastNoiseLite();
        noise.SetFrequency(frequency);
        noise.SetRotationType3D(FastNoiseLite.RotationType3D.ImproveXYPlanes);
        int index = 0;

        offX = Random.Range(0, 99999);
        offY = Random.Range(0, 99999);

        //Create array holding forces
        length = size + 2 * Lifetime;
        Forces = new Vector3[length, length];

        //create paths
        for (int i = 0; i < particleNum; i++)
        {
            //spawn in random location
            FlowParticle particle = new FlowParticle();

            particle.position = new Vector2(Random.Range(0, size), Random.Range(0, size));

            for (int j = 0; j < Lifetime; j++)
            {
                //find nearest coordinate and get velocity which is added to current velocity
                float val1 = Mathf.Cos(noise.GetNoise(particle.position.x + offX, particle.position.y + offY, depth) * 2 * Mathf.PI);
                float val2 = Mathf.Sin(noise.GetNoise(particle.position.x + offX, particle.position.y + offY, depth) * 2 * Mathf.PI);
                particle.moveVel += new Vector3(val2, val1, 0);// * (1 - Mathf.Clamp(val1 * val2, 0, 1)) / 2;
                if (particle.moveVel.magnitude > 1)
                {
                    particle.moveVel.Normalize();
                }

                particle.position += (Vector2)particle.moveVel / 2;

                //change values in force array
                Forces[(int)(particle.position.x + Lifetime + size / 2),(int)(particle.position.y + Lifetime + size / 2)] = new Vector3(val2, val1, 0);

                float rot_z = Mathf.Atan2(val1, val2) * Mathf.Rad2Deg;
                if (j % 12 == 0)
                {
                    GameObject Arrow = (GameObject)Instantiate(arrow, transform.position + new Vector3(particle.position.x, particle.position.y, 0), Quaternion.identity, transform);
                    Arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                }


                //Set up side winds
                Vector2 rightVector = (Vector2)(Quaternion.Euler(0f, 0f, rot_z - 180) * Vector2.up);
                float angle = 90f / (thickness - 1);
                for(int k = 0; k < thickness; k++)
                {
                    //Change force values
                    Forces[(int)(particle.position.x + Lifetime + size / 2 + rightVector.x * k / 2), (int)(particle.position.y + Lifetime + size / 2 + rightVector.y * k / 2)] = Quaternion.Euler(0, 0, angle * k) * new Vector3(val2, val1, 0) * 5;// * (10 - k) / 2;
                    Forces[(int)(particle.position.x + Lifetime + size / 2 - rightVector.x * k / 2), (int)(particle.position.y + Lifetime + size / 2 - rightVector.y * k / 2)] = Quaternion.Euler(0, 0, -angle * k) * new Vector3(val2, val1, 0) * 5;// * (10 - k) / 2;
                }
            }
        }
    }
}

