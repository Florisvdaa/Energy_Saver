using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("UI Feedbacks")]
    [SerializeField] private MMF_Player roomNameFeedback;
    [SerializeField] private MMF_Player lockedUIFeedback;

    [Header("Camera Feedbacks")]
    [SerializeField] private MMF_Player noRoomsLeftFeedback;
    [SerializeField] private MMF_Player noRoomsRightFeedback;

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

    // References
    public MMF_Player GetRoomNameFeedback() => roomNameFeedback;
    public MMF_Player GetLockedUIFeedback() => lockedUIFeedback;
    public MMF_Player GetNoRoomsLeftFeedback() => noRoomsLeftFeedback;
    public MMF_Player GetNoRoomsRightFeedback() => noRoomsRightFeedback;
}
