using UnityEngine;
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

        // raycast 를 통해 클릭(터치)한 블록이 타워 설치 가능 블록이면 영웅 소환
        // TODO : 중복 체크 및 소환 재화 소모
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);

        if ( Physics.Raycast(ray, out var hit, Mathf.Infinity) )
        {
            if ( hit.transform.gameObject.CompareTag("TowerBlock") )
            {
                heroSpawner.TrySpawnHero(hit.transform);
            }
        }
    }
}
