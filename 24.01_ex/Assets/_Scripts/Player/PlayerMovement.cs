using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    PlayerController inputActions;
    Vector3 inputDir;
    public float moveSpeed = 3.0f;
    public float jump = 1.0f;
    private bool isJump = true;
    Rigidbody2D rigid;
    Animator anim;
    public float playerHp = 10;
    public string failScene;
    public string successScene;
    SpriteRenderer spriteRenderer;
    public float coolDown = 1;
    private void Awake()
    {
        inputActions = new PlayerController();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Movement.performed += OnMove;
        inputActions.Player.Movement.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.canceled -= OnJump;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Movement.canceled -= OnMove;
        inputActions.Player.Movement.performed -= OnMove;
        inputActions.Player.Disable();
    }
    private void Update()
    {
        Move();
        if(playerHp <= 0)
        {
            Dead();
        }
    }

    private void Move()
    {
        transform.Translate(moveSpeed * Time.fixedDeltaTime * inputDir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        Debug.Log("좌표값: " + inputDir);
        if(context.ReadValue<Vector2>().x == 1) 
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (!anim.GetBool("isJump"))
                anim.SetBool("isMove", true);
        }
        else if(context.ReadValue<Vector2>().x == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (!anim.GetBool("isJump"))
                anim.SetBool("isMove", true);
        }
        else if(context.canceled)
        {
            anim.SetBool("isMove", false);
        }
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if(!anim.GetBool("isJump") && isJump == true && context.performed)
        {
            rigid.AddForce(Vector3.up * jump, ForceMode2D.Impulse);
            isJump = false;
            anim.SetBool("isJump", true);
        }
        else if(context.canceled)
        {

        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Map"))
        {
            isJump = true;
            anim.SetBool("isJump", false);
        }
        if(collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene(successScene);
            Debug.Log("도착");
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            OnDamage();
            Invoke(nameof(OffDamage), coolDown);
        }
    }

    private void OnDamage()
    {
        float count = 0;
        gameObject.layer = 9;
        Debug.Log(gameObject.layer);
        while (count <= 10)
        {
            if (count % 2 == 0)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            count++;
        }

        playerHp -= 1.0f;
        Debug.Log("현재 남은체력: " + playerHp);
        if (playerHp <= 0.0f)
        {
            Dead();
        }
    }
    void OffDamage()
    {
        Debug.Log("off데미지 호출");
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void Dead()
    {
        SceneManager.LoadScene(failScene);
        Destroy(gameObject);
    }

}

