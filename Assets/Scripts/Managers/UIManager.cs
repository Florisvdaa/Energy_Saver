using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider energySlider;

    [Header("Locked UI")]
    [SerializeField] private GameObject lockedSegment;

    [Header("Room name")]
    [SerializeField] private TextMeshProUGUI roomName;
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

        CheckIfRoomIsLocked();
    }

    private void Update()
    {
        if (scoreText) scoreText.text = $"Score: {GameManager.Instance.Score}";
        energySlider.value = GameManager.Instance.CurrentEnergy;

        // Debug 
        //DisplayRoomName();
    }

    public void DisplayRoomName()
    {
        if (RoomManager.Instance == null) return;

        Room currentRoom = RoomManager.Instance.AvailableRooms[RoomManager.Instance.CurrentIndex];
        if (currentRoom != null)
        {
            roomName.text = currentRoom.RoomName;
        }

        FeedbackManager.Instance.GetRoomNameFeedback().PlayFeedbacks();
    }

    public void CheckIfRoomIsLocked()
    {
        if (RoomManager.Instance == null || lockedSegment == null) return;

        Room currentRoom = RoomManager.Instance.AvailableRooms[RoomManager.Instance.CurrentIndex];

        if (currentRoom != null && !currentRoom.IsUnlocked)
        {
            lockedSegment.SetActive(true);
        }
        else
        {
            lockedSegment.SetActive(false);
        }
    }

}
