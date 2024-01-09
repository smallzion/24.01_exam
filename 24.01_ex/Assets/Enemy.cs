using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float moveRange = 5.0f;
    bool movingRight = true;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    float firstPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        firstPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }

    void OnMove() 
    {
        Vector3 newPosition = transform.position;

        if (movingRight)
        {
            newPosition.x += moveSpeed * Time.deltaTime;
        }
        else
        {
            newPosition.x -= moveSpeed * Time.deltaTime;
        }

        // 계산된 위치로 이동
        transform.position = newPosition;

        // 이동 범위를 벗어나면 방향을 바꿈
        if (Mathf.Abs(firstPosition - transform.position.x) >= moveRange)
        {
            movingRight = !movingRight;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
