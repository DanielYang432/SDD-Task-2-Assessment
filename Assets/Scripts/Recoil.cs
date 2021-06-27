using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public Vector3 upRecoil;
    Vector3 originalRotation;

    private void Start()
    {
        originalRotation = transform.localEulerAngles.normalized;

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AddRecoil();
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            stopRecoil();
        }
    }


    private void AddRecoil()
    {
        transform.localEulerAngles += upRecoil;
    }


    private void stopRecoil()
    {
        transform.localEulerAngles = originalRotation;

    }
}
