using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public GameObject otherGameOject;
    public Respawning respawning;
    public Text deathCounter;

    void Awake()
    {
        respawning = otherGameOject.GetComponent<Respawning>();
    }

    private void Update()
    {
        deathCounter.text = respawning.Death.ToString();
    }
}
