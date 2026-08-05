using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private HeroSpawner    heroSpawner;

    private Camera      mainCamera;
    private Vector2     screenPoint;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        screenPoint = context.ReadValue<Vector2>();     // 현재 마우스 포인터의 위치 정보 저장
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // 블록을 클릭(터치) 했을 때만 동작하도록 처리
        if ( !context.performed )
            return;
        if (!context.ReadValueAsButton())
            return;

        // 지금 클릭한 위치가 UI(팝업 버튼 등) 위라면, 아래의 블록 클릭 로직은 실행하지 않음
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // raycast 를 통해 클릭(터치)한 블록이 타워 설치 가능 블록이면 영웅 소환
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);

        if ( Physics.Raycast(ray, out var hit, Mathf.Infinity) )
        {
            if ( hit.transform.gameObject.CompareTag("HeroBlock") )
            {
                heroSpawner.TrySpawnHero(hit.transform);
            }
        }
    }
}
