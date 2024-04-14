using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Voices {
        Spawn,
        Death,
        Win,
        Idle,
        Fly,
        Forage,
        Upscale,
        Downscale

    }

    [SerializeField] public bool IsOwnRecord { get; set;} = true;

    [SerializeField] AudioClip[] ambiantSounds;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource voiceSource;
    //Own
    [SerializeField] AudioClip[] voiceSpawn;
    [SerializeField] AudioClip[] voiceDeath;
    [SerializeField] AudioClip[] voiceWin;
    [SerializeField] AudioClip[] voiceIdle;
    [SerializeField] AudioClip[] voiceFly;
    [SerializeField] AudioClip[] voiceForage;
    [SerializeField] AudioClip[] voiceUpscale;
    [SerializeField] AudioClip[] voiceDownscale;
    //Portal
    [SerializeField] AudioClip[] voiceSpawnPortal;
    [SerializeField] AudioClip[] voiceDeathPortal;
    [SerializeField] AudioClip[] voiceWinPortal;
    [SerializeField] AudioClip[] voiceIdlePortal;
    [SerializeField] AudioClip[] voiceFlyPortal;
    [SerializeField] AudioClip[] voiceForagePortal;
    [SerializeField] AudioClip[] voiceUpscalePortal;
    [SerializeField] AudioClip[] voiceDownscalePortal;

    private float lastInputTime;

    void Start()
    {
        StartCoroutine(PlayRandomSoundWithDelay());
        lastInputTime = Time.time;
    }

    void Update()
    {
        // Vérifier si le joueur n'a pas bougé pendant 20 secondes
        if (Time.time - lastInputTime >= 10.0f)
        {
            PlayVoice(Voices.Idle,0);
            lastInputTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsOwnRecord = !IsOwnRecord;
        }
    }

    void FixedUpdate()
    {
        // Mettre à jour le dernier temps d'entrée à chaque frame fixe où il y a un mouvement
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            lastInputTime = Time.time;
        }
    }

    public IEnumerator PlayRandomSoundWithDelay()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, ambiantSounds.Length);
            float delay = ambiantSounds[randomIndex].length;
            audioSource.PlayOneShot(ambiantSounds[randomIndex]);

            yield return new WaitForSeconds(delay);
        }
    }

    public void PlayVoice(Voices voice, int probablilityMax = 4){
        if(!voiceSource.isPlaying && Random.Range(0, probablilityMax) == 0){
            AudioClip clip = null;
            switch (voice)
            {
                case Voices.Spawn :
                    if(IsOwnRecord)
                        clip = voiceSpawn[Random.Range(0, voiceSpawn.Length)];
                    else
                        clip = voiceSpawnPortal[Random.Range(0, voiceSpawnPortal.Length)];
                break;
                case Voices.Death :
                    if(IsOwnRecord)
                        clip = voiceDeath[Random.Range(0, voiceDeath.Length)];
                    else
                        clip = voiceDeathPortal[Random.Range(0, voiceDeathPortal.Length)];
                break;
                case Voices.Win :
                    if(IsOwnRecord)
                        clip = voiceWin[Random.Range(0, voiceWin.Length)];
                    else
                        clip = voiceWinPortal[Random.Range(0, voiceWinPortal.Length)];
                break;
                case Voices.Idle :
                    if(IsOwnRecord)
                        clip = voiceIdle[Random.Range(0, voiceIdle.Length)];
                    else
                        clip = voiceIdlePortal[Random.Range(0, voiceIdlePortal.Length)];
                break;
                case Voices.Fly :
                    if(IsOwnRecord)
                        clip = voiceFly[Random.Range(0, voiceFly.Length)];
                    else
                        clip = voiceFlyPortal[Random.Range(0, voiceFlyPortal.Length)];
                break;
                case Voices.Forage :
                    if(IsOwnRecord)
                        clip = voiceForage[Random.Range(0, voiceForage.Length)];
                    else
                        clip = voiceForagePortal[Random.Range(0, voiceForagePortal.Length)];
                break;
                case Voices.Upscale :
                    if(IsOwnRecord)
                        clip = voiceUpscale[Random.Range(0, voiceUpscale.Length)];
                    else
                        clip = voiceUpscalePortal[Random.Range(0, voiceUpscalePortal.Length)];
                break;
                case Voices.Downscale :
                    if(IsOwnRecord)
                        clip = voiceDownscale[Random.Range(0, voiceDownscale.Length)];
                    else
                        clip = voiceDownscalePortal[Random.Range(0, voiceDownscalePortal.Length)];
                break;
            }
            if(clip != null){
                if(IsOwnRecord)
                    voiceSource.volume = 1.0f;
                else
                    voiceSource.volume = 0.02f;
                voiceSource.PlayOneShot(clip);
            }
        }
    }
}
