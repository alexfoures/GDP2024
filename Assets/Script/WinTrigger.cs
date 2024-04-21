using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] GameObject end;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        end.SetActive(true);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>().PlayVoice(SoundManager.Voices.Win, 0);
        Destroy(gameObject);
    }
}
