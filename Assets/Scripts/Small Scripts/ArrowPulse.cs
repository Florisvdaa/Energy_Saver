using UnityEngine;

public enum Direction { Left, Right }

public class ArrowPulse : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private Vector3 normalScale = Vector3.one;      // Default scale
    [SerializeField] private Vector3 hoverScale = Vector3.one * 1.2f; // How big it should get
    [SerializeField] private Vector3 clickedScale = Vector3.one * 0.8f; // Shrink when clicked
    [SerializeField] private float speed = 5f;                       // How fast it lerps

    public Direction direction = Direction.Left;
    public RoomManager roomManager;

    private Vector3 targetScale;
    private void Start()
    {
        targetScale = normalScale;
        transform.localScale = normalScale;
    }

    private void Update()
    {
        if (targetScale == hoverScale)
        {
            float pulse = 0.1f * Mathf.Sin(Time.time * 5f); // 0.1 = pulse size, 5 = speed
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale + Vector3.one * pulse, Time.deltaTime * speed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
        }
    }

    private void OnMouseEnter()
    {
        targetScale = hoverScale;
    }

    private void OnMouseExit()
    {
        targetScale = normalScale;
    }
    private void OnMouseDown()
    {
        Debug.Log("iscliked");

        targetScale = clickedScale;
        if (!roomManager) return;
        if (direction == Direction.Left) roomManager.GoLeft();
        else roomManager.GoRight();

    }

    private void OnMouseUp()
    {
        targetScale = hoverScale; // Go back to hover scale after releasing
    }
}
