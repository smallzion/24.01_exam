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
        float minX = 0.0f;
        float maxX = 35.5f;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

}
//0 부터 35.5까지