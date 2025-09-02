using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider energySlider;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (energySlider)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = GameManager.Instance.MaxEnergy;
            energySlider.value = GameManager.Instance.CurrentEnergy;
        }
    }

    private void Update()
    {
        if (scoreText) scoreText.text = $"Score: {GameManager.Instance.Score}";
        energySlider.value = GameManager.Instance.CurrentEnergy;
    }
}
