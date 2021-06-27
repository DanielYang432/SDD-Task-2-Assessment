using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTrackerBehaviour : MonoBehaviour
{
    [SerializeField] private LineRenderer LineRenderer = null;
    [SerializeField] private Transform fromTransform = null;
    [SerializeField] private Transform toTransform = null;

    public GameObject otherGameOject;
    public Turret turret;


    void Awake()
    {
        turret = otherGameOject.GetComponent<Turret>();
    }

    void SetTarget(Transform transform)
    {
        toTransform = transform;
        LineRenderer.positionCount = 2;
    }
    public void ClearTarget()
    {
        LineRenderer.positionCount = 0;
    }

    private void LateUpdate()
    {
        if (toTransform == null || fromTransform == null) { return; }

        if (turret.Targettable)
        {        
            LineRenderer.positionCount = 2;
            LineRenderer.SetPosition(0, fromTransform.position);
            LineRenderer.SetPosition(1, toTransform.position);

        }
        else if (!turret.Targettable)
        {
            ClearTarget();
        }
    }
}   
