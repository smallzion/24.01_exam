using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float raycastDistance = 1f;
    public LayerMask groundLayer;
    Vector2 direction = Vector2.zero;
    private int moveDirection = 1; // 1이면 오른쪽, -1이면 왼쪽

    public float detectionRange = 5f;
    public float movementSpeed = 2f;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(Flip), Random.Range(0f, 5f), Random.Range(5f, 10f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!DetectPlayer())
        {
            Move();
            CheckCliff();
        }
        else// if (DetectPlayer())
        {
            ChasePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection *= -1;
            Flip(moveDirection);
        }
    }

    bool DetectPlayer()
    {
        if(player != null)
        {
            direction = player.position - transform.position;
        }
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, layerMask);
        Debug.DrawRay(transform.position, direction.normalized * detectionRange, Color.green);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            detectionRange = 10.0f;
            Debug.Log("찾음!");
            return true;
        }

        Debug.Log("못찾음!");
        detectionRange = 3.0f;
        return false;
    }

    void Move()
    {
        Vector2 movement = new(moveDirection * movementSpeed * Time.deltaTime, 0f);
        transform.Translate(movement);
    }

    void CheckCliff()
    {
        // 몬스터의 현재 스케일을 기반으로 레이캐스트 발사 위치 조정
        float raycastDirection = transform.localScale.x > 0 ? 1.0f : -1.0f;
        Vector2 raycastOrigin = new(transform.position.x + raycastDirection, transform.position.y);

        // Raycast를 통해 낭떠러지 감지
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawRay(raycastOrigin, Vector2.down.normalized, Color.red);
        Debug.Log("몬스터 낭떠러지 체크");

        // 낭떠러지가 감지되면 방향을 반대로 변경
        if (hit.collider == null)
        {
            moveDirection *= -1;
            Flip(moveDirection);
        }
    }

    void Flip(int moveDirection)
    {
        Debug.Log("플립");

        // 스케일을 조절하여 방향 뒤집기
        Vector3 scale = transform.localScale;
        scale.x = moveDirection; // 방향에 따라 스케일 값 설정
        transform.localScale = scale;
    }
    void Flip()
    {
        moveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // 방향에 따라 스케일 값 설정
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
        
        Flip(moveDirection);

    }

}
