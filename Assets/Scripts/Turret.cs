using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private Transform Target;

    [Header("Atrributes" )]
    public float range = 10f;
    public float fireDelay = 2f;
    float timeSinceFire = 10f;
    

    [Header("Unity Setup Fields")]
    public string enemyTag = "Player";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    public bool Targettable = false;
    public GameObject m_shotPrefab;


    //public ParticleSystem muzzleFlash;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject neartestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                neartestEnemy = enemy;
            }
        }
        
        if(neartestEnemy != null && shortestDistance <= range)
        {
            Target = neartestEnemy.transform;
            Targettable = true;
            
        }
        else
        {
            Target = null;
            Targettable = false;
        }
    }

    void Update()
    {
        if (Target == null)
            return;


        Vector3 dir = Target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 roation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = lookRotation;
        
        partToRotate.forward = Target.position - transform.position;

        if (timeSinceFire > fireDelay && Targettable)
        {
            Shoot();
        }

        timeSinceFire += Time.deltaTime;
    }

    void Shoot()
    {
        timeSinceFire = 0f;
        Instantiate(m_shotPrefab, firePoint.position, partToRotate.rotation).transform.up = partToRotate.forward;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
