using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDP2024;
using Unity.VisualScripting;

public class TriggerEvent : MonoBehaviour
{

    GameEventProcessor gameEventProcessor;

    // Start is called before the first frame update
    void Start()
    {
        gameEventProcessor = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEventProcessor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameEventProcessor.SendPowerUps();
        Destroy(gameObject);
    }
}
