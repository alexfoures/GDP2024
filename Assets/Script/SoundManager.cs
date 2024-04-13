using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayRandomSoundWithDelay());
    }

    IEnumerator PlayRandomSoundWithDelay()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, sounds.Length);
            float delay = sounds[randomIndex].length;
            audioSource.PlayOneShot(sounds[randomIndex]);

            yield return new WaitForSeconds(delay);
        }
    }
}
