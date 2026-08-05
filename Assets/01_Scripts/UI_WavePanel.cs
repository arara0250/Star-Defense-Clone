using TMPro;
using UnityEngine;

public class UI_WavePanel : MonoBehaviour
{
    [SerializeField] private WaveSystem         waveSystem;
    [SerializeField] private TextMeshProUGUI    waveText;

    private void OnEnable()
    {
        // 이벤트 구독
        waveSystem.OnWaveChanged += UpdateWaveText;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        waveSystem.OnWaveChanged -= UpdateWaveText;
    }

    private void UpdateWaveText()
    {
        waveText.text = $"{ waveSystem.CurrentWave } / { waveSystem.MaxWave }";
    }
}
