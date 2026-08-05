using UnityEngine;

// UI(HP바) 가 타겟(적) 을 따라다니도록 제어하는 컴포넌트
public class UI_FollowTarget : MonoBehaviour
{
    private Transform       _target;

    private RectTransform   rectTransform;
    private Camera          mainCamera;

    private void Awake()
    {
        rectTransform   = GetComponent<RectTransform>();
        mainCamera      = Camera.main;
    }

    // 외부에서 추적 대상을 설정할 때 호출
    public void SetTarget(Transform target) => _target = target;

    // 타겟의 위치가 갱신된 이후에 UI 가 따라가도록 LateUpdate() 사용
    private void LateUpdate()
    {
        // 추적 대상이 사라지면, UI 도 삭제
        if ( _target == null )
        {
            Destroy(gameObject);
            return;
        }

        rectTransform.position = mainCamera.WorldToScreenPoint( _target.position );
    }
}
