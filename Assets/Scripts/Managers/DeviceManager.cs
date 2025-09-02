using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager Instance { get; private set; }

    [SerializeField] private List<Device> devices = new List<Device>();

    [Header("Difficulty")]
    [SerializeField] private float spawnInterval = 1.5f;   // how often a device turns on
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float rampPerSecond = 0.05f;  // how fast interval decreases

    private float turnOffReward = 0.8f;

    private float timer;
    private float elapsed;
    private int score;
    private bool gameOver;

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
    void Update()
    {
        if (gameOver) return;

        elapsed += Time.deltaTime;

        // Ramp up difficulty
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - rampPerSecond * Time.deltaTime);

        // Periodically turn a random OFF device ON
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TurnOnRandomDevice();
        }

        // Drain energy for all ON devices
        float totalDrain = 0f;
        foreach (var d in devices)
            if (d.IsOn) totalDrain += d.DrainPerSeconds;

        GameManager.Instance.AddEnergy(-totalDrain * Time.deltaTime);

        //energyBar.AddEnergy(-totalDrain * Time.deltaTime);

        //if (energyBar.IsEmpty())
            //EndGame();
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
        //if (scoreText) scoreText.text = $"Score: {score}";
        Debug.Log($"Score: {score}");
    }
}
