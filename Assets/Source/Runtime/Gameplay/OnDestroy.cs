using GDP2024;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class OnDestroy : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] AudioSource audioSource;
    private float _lifetime = 1f;
    private SpriteRenderer spriteR;
    private PowerUpManagement _powerManagement = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        _powerManagement = collision.gameObject.GetComponent<PowerUpManagement>();
        if (_powerManagement.IsDrill)
        {
            _lifetime -= (Time.deltaTime / 4);
            if (_lifetime < 0f)
            {
                _lifetime = 0f;
            }
            var index = (int)Math.Floor(_lifetime * sprites.Length);
            Debug.Log(index);
            if (_lifetime <= 0)
            {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>().PlayVoice(SoundManager.Voices.Forage);
                Destroy(gameObject);
            }
            if (gameObject)
            {
                if(!audioSource.isPlaying)
                    audioSource.Play();
                spriteR.sprite = sprites[index];
                Destroy(GetComponent<PolygonCollider2D>());
                gameObject.AddComponent<PolygonCollider2D>();
            }
        } 
    }
}
