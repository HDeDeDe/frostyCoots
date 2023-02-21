using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlope : MonoBehaviour
{
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider.size = sprite.size;
    }
}
