using GDP2024;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class OnDestroy : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    private float _lifetime = 1f;
    private SpriteRenderer spriteR;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        _lifetime -= (Time.deltaTime/4);
        if (_lifetime < 0f)
        { 
            _lifetime = 0f; 
        }
        var index = (int)Math.Floor(_lifetime * sprites.Length);
        Debug.Log(index);
        if (_lifetime <= 0)
        {
            Destroy(gameObject);
        }
        if (gameObject)
        {
            spriteR.sprite = sprites[index];
        }
    }
}
