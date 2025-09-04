using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player roomNameFeedback;
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
}
