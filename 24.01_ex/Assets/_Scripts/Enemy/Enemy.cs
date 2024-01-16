using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    private int nextMove;
    public float raycastDistance = 1f;
    public LayerMask groundLayer;

    private int moveDirection = 1; // 1이면 오른쪽, -1이면 왼쪽

    public float detectionRange = 5f;
    public float movementSpeed = 2f;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("RandomizeDirection", Random.Range(0f, 5f), Random.Range(2f, 5f));
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Flip(); // 시작할 때 방향에 따라 스케일 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (!DetectPlayer())
        {
            Move();
            CheckCliff();
        }

        if (DetectPlayer())
        {
            ChasePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    bool DetectPlayer()
    {
        Vector2 direction = player.position - transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, layerMask);
        Debug.DrawRay(transform.position, direction.normalized * detectionRange, Color.green);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("찾음!");
            return true;
        }

        Debug.Log("못찾음!");
        return false;
    }

    void Move()
    {
        Vector2 movement = new Vector2(moveDirection * movementSpeed * Time.deltaTime, 0f);
        transform.Translate(movement);
    }

    void CheckCliff()
    {
        // Raycast를 통해 낭떠러지 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);

        // 낭떠러지가 감지되면 방향을 반대로 변경
        if (hit.collider == null)
        {
            Flip();
        }
    }

    void RandomizeDirection()
    {
        // 랜덤으로 방향 설정
        moveDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        Flip(); // 방향 설정에 따라 스케일을 조절하여 뒤집기
    }

    void Flip()
    {
        // 스케일을 조절하여 방향 뒤집기
        Vector3 scale = transform.localScale;
        scale.x = moveDirection; // 방향에 따라 스케일 값 설정
        transform.localScale = scale;
    }

    void ChasePlayer()
    {
        // 플레이어와 적 사이의 방향 벡터 계산
        Vector2 direction = (player.position - transform.position).normalized;

        // 이동할 방향 설정
        moveDirection = direction.x > 0 ? 1 : -1;

        // 이동
        Vector2 movement = new Vector2(moveDirection * movementSpeed * Time.deltaTime, 0f);
        transform.Translate(movement);

        // 방향이 바뀌면 스케일 조절
        Flip();
    }

}
