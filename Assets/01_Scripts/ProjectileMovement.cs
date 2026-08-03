using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float      moveSpeed;          // 발사체 이동 속도
    [SerializeField] private Vector3    moveDirection;      // 발사체 이동 방향

    // 외부 참조용 프로퍼티
    public float MoveSpeed => moveSpeed;

    private void Update()
    {
        // 발사체 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // 외부에서 발사체의 방향을 변경할 때 호출
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
