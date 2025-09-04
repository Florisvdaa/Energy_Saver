using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Rooms to move (their Y/Z can be equal)")]
    [SerializeField] private Transform[] rooms;            // e.g. at x = -35, 0, 35
    [SerializeField] private Room[] availableRooms;

    [Header("Navigation")]
    [SerializeField] private int startIndex = 1;           // 0 = left, 1 = middle, 2 = right
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3[] basePos;                   // original positions
    private float currentOffsetX;                // how far we’ve shifted everything
    private int index;
    private bool isMoving;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (rooms == null || rooms.Length == 0)
            return;

        basePos = new Vector3[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
            basePos[i] = rooms[i].position;

        index = Mathf.Clamp(startIndex, 0, rooms.Length - 1);

        // Center selected room at camera X (assume camera is at X = 0; change if needed)
        currentOffsetX = -basePos[index].x;
        ApplyOffset(currentOffsetX);

        SetupRooms();
    }

    private void SetupRooms()
    {
        availableRooms = new Room[rooms.Length];

        for (int i = 0; i < rooms.Length; i++)
        {
            Room r = rooms[i].GetComponent<Room>();
            if (r != null)
            {
                availableRooms[i] = r;
            }
            else
            {
                Debug.LogWarning($"RoomManager: No Room component found on {rooms[i].name}");
            }
        }

    }

    public void GoLeft() => SetRoom(index - 1);
    public void GoRight() => SetRoom(index + 1);

    public void SetRoom(int newIndex)
    {
        if (rooms == null || rooms.Length == 0) return;
        if (isMoving) return;

        newIndex = Mathf.Clamp(newIndex, 0, rooms.Length - 1);
        if (newIndex == index) return;

        float targetOffsetX = -basePos[newIndex].x;   // shift all so newIndex room ends up at X=0
        StartCoroutine(MoveRoutine(targetOffsetX, newIndex));
    }

    IEnumerator MoveRoutine(float targetOffsetX, int newIndex)
    {
        FeedbackManager.Instance.GetRoomNameFeedback().SkipToTheEnd();

        isMoving = true;

        float start = currentOffsetX;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float k = ease.Evaluate(t);
            float offs = Mathf.LerpUnclamped(start, targetOffsetX, k);
            ApplyOffset(offs);
            yield return null;
        }

        currentOffsetX = targetOffsetX;
        ApplyOffset(currentOffsetX);

        index = newIndex;
        isMoving = false;

        UIManager.Instance.DisplayRoomName();
        UIManager.Instance.CheckIfRoomIsLocked();
    }

    void ApplyOffset(float offsetX)
    {
        // Only move on X; keep original Y/Z (so camera + look target can stay put)
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector3 p = basePos[i];
            rooms[i].position = new Vector3(p.x + offsetX, p.y, p.z);
        }
    }

    public int CurrentIndex => index;
    public int RoomCount => rooms?.Length ?? 0;
    public Room[] AvailableRooms => availableRooms;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            availableRooms[0].UnlockRoom();
            DeviceManager.Instance.OnRoomUnlocked(availableRooms[0]);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            availableRooms[2].UnlockRoom();
            DeviceManager.Instance.OnRoomUnlocked(availableRooms[2]);
        }
    }
}
