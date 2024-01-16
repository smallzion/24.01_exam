using UnityEngine;

public class Background : MonoBehaviour
{
    public float parallaxSpeed = 2.0f; // 배경의 상대적인 속도

    void Update()
    {
        float moveX = Time.deltaTime * Input.GetAxis("Horizontal") * parallaxSpeed;
        transform.Translate(new Vector3(moveX, 0, 0));
    }
}
