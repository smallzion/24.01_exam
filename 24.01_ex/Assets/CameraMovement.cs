using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // 카메라가 따라갈 대상
    public float moveSpeed; // 카메라가 따라갈 속도
    public float maxDistance = 5.0f; // 카메라가 움직이지 않을 최대 범위

    private float originalMoveSpeed; // 초기 이동 속도 저장

    // Start is called before the first frame update
    void Start()
    {
        originalMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // 대상이 있는지 체크
        if (target != null)
        {
            // 플레이어와 카메라 간의 거리 계산
            float distance = Vector3.Distance(target.transform.position, transform.position);

            // 일정 범위 안에 있는지 확인
            if (distance > maxDistance)
            {
                // this는 카메라를 의미 (z값은 카메라값을 그대로 유지)
                Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

                // vectorA -> B까지 T의 속도로 이동
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            // 플레이어와 카메라 간의 거리가 maxDistance를 초과하지 않도록 제한
            ClampCameraPosition();
        }
    }

    void ClampCameraPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, 0, 80);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // 충돌한 오브젝트가 Wall 태그를 가진 경우
            Vector3 wallDirection = (other.transform.position - transform.position).normalized;

            // 벽과의 상대 위치를 계산
            float relativePosition = Vector3.Dot(wallDirection, Vector3.right);

            // 충돌 방향이 오른쪽이면 카메라를 왼쪽으로 이동 방지
            if (relativePosition < 0)
            {
                moveSpeed = 0.0f;
                Debug.Log("Cannot move to the right");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // 충돌이 끝난 경우, 이동 속도를 복원
            moveSpeed = originalMoveSpeed;
            Debug.Log("Wall exited");
        }
    }
}
