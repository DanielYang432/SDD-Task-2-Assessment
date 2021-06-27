using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;

    RaycastHit hit;

    public Camera cam;
    public ParticleSystem muzzleFlash;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > m_shootRateTimeStamp)
            {
                Shoot();
                m_shootRateTimeStamp = Time.time + shootRate;
            }
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);

        }

    }

}
