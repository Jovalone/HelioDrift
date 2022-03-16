using UnityEngine;
using System.Collections;

public class TurretFirePoint : MonoBehaviour
{
    public float timeDelay;
    public float overHeat1;
    public float overHeat;
    public float maxDist;
    public int maxShot;
    public int maxShot1;
    public int shotNum;
    public int shotNum1;
    public GameObject Bullet;
    public Transform FirePoint;
    public Transform target;
    public bool loading;
    public Animator anim;
    public Turret turret;

    void Update()
    {
        //Find target
        if(turret.Battle)
        {
            target = turret.target;
        }

        if(target != null)
        {
            if (!loading && Vector3.Distance(FirePoint.position, target.position) < maxDist)
            {
                shotNum++;
                shotNum1++;
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        loading = true;
        anim.SetTrigger("shot");
        Instantiate(Bullet, FirePoint.position, FirePoint.rotation);

        yield return new WaitForSeconds(timeDelay);

        if (shotNum1 > maxShot1)
        {
            shotNum1 = 0;
            yield return new WaitForSeconds(overHeat1);
        }

        if (shotNum > maxShot)
        {
            shotNum = 0;
            yield return new WaitForSeconds(overHeat);
        }

        loading = false;
    }
}
