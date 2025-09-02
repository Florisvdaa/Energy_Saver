using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform root;        // TooltipRoot panel
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailText;
    [SerializeField] private TextMeshProUGUI activeText;
    [SerializeField] private Image activeIcon;
    [SerializeField] private Image deactivatedIcon;

    [Header("Behavior")]
    [SerializeField] private Vector2 cursorOffset = new Vector2(16f, -16f);

    RectTransform _canvasRect;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;

        if (!canvas) canvas = GetComponentInChildren<Canvas>();
        _canvasRect = canvas.transform as RectTransform;

        Hide();
    }

    void Update()
    {
        if (!root.gameObject.activeSelf) return;
        UpdatePosition(Input.mousePosition);
    }

    public void Show(string title, float drainPerSecond, bool isActive)
    {
        if (titleText) titleText.text = title;
        if (detailText) detailText.text = $"Drain: {drainPerSecond:0.##}/s";
        if (isActive)
        {
            activeIcon.gameObject.SetActive(true);
            deactivatedIcon.gameObject.SetActive(false);
            if (activeText) activeText.text = "Device is active";
        }
        else
        {
            activeIcon.gameObject.SetActive(false);
            deactivatedIcon.gameObject.SetActive(true);
            if (activeText) activeText.text = "Device is not active";
        }

        root.gameObject.SetActive(true);
        UpdatePosition(Input.mousePosition);
    }

    public void Hide()
    {
        if (root) root.gameObject.SetActive(false);
    }

    void UpdatePosition(Vector3 screenPos)
    {
        // Convert screen to anchored pos in canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            (Vector2)screenPos + cursorOffset,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        // Clamp tooltip inside canvas
        Vector2 halfCanvas = _canvasRect.rect.size * 0.5f;
        Vector2 halfTooltip = root.rect.size * 0.5f;

        float minX = -halfCanvas.x + halfTooltip.x;
        float maxX = halfCanvas.x - halfTooltip.x;
        float minY = -halfCanvas.y + halfTooltip.y;
        float maxY = halfCanvas.y - halfTooltip.y;

        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        root.anchoredPosition = localPoint;
    }
}
