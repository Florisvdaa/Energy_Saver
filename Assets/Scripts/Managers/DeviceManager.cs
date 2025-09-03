using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager Instance { get; private set; }

    [SerializeField] private List<Device> devices = new List<Device>();
    private Room[] allRooms;

    [Header("Difficulty")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float rampPerSecond = 0.05f;

    private float turnOffReward = 0.8f;
    private float timer;
    private float elapsed;
    private int score;
    [SerializeField] private bool gameOver;

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

        DiscoverRoomsAndDevices();
    }

    private void DiscoverRoomsAndDevices()
    {
        allRooms = FindObjectsByType<Room>(FindObjectsSortMode.None); // include inactive rooms
        devices.Clear();

        foreach (Room room in allRooms)
        {
            if (room.IsUnlocked)
            {
                Device[] roomDevices = room.GetComponentsInChildren<Device>(true);
                devices.AddRange(roomDevices);
            }
        }

        Debug.Log($"DeviceManager: Found {devices.Count} devices in unlocked rooms.");
    }

    public void OnRoomUnlocked(Room room)
    {
        Device[] newDevices = room.GetComponentsInChildren<Device>(true);
        devices.AddRange(newDevices);
        Debug.Log($"DeviceManager: Added {newDevices.Length} devices from newly unlocked room.");
    }

    void Update()
    {
        if (gameOver) return;

        elapsed += Time.deltaTime;
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - rampPerSecond * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TurnOnRandomDevice();
        }

        float totalDrain = 0f;
        foreach (var d in devices)
            if (d.IsOn) totalDrain += d.DrainPerSeconds;

        GameManager.Instance.AddEnergy(-totalDrain * Time.deltaTime);
    }

    void TurnOnRandomDevice()
    {
        var offList = devices.FindAll(d => !d.IsOn);
        if (offList.Count == 0) return;
        var chosen = offList[Random.Range(0, offList.Count)];
        chosen.TurnOnDevice();
    }

    public void OnDeviceTurnedOff(Device d)
    {
        GameManager.Instance.AddEnergy(turnOffReward);
        UpdateScore(score + 1);
    }

    void UpdateScore(int s)
    {
        score = s;
        Debug.Log($"Score: {score}");
    }
}