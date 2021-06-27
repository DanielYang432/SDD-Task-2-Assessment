using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] Transform startPoint, endPoint;

    [SerializeField] float changeDirectionDelay;

    private Transform destinationTarget, departTarget;

    private float startTime;

    //public GameObject Player;

    private float journeyLength;

    bool isWaiting;



    private void Start()
    {
        departTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.deltaTime;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01)
            {
                float distCovered = (Time.time - startTime) * speed;

                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(changeDelay());
            }
        }
    }

    void ChangeDestination()
    {
        if(departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }
    }

    IEnumerator changeDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }


    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Player.transform.parent = transform;
    //        Debug.Log("Hitplayer");
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Player.transform.parent = null;
    //    }
    //}




}
