using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score = 0;
    private float maxEnergy = 100f;
    private float currentEnergy;

    public GameObject deviceManager;
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

        currentEnergy = maxEnergy;
    }

    // Debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            deviceManager.SetActive(true);
    }

    public void AddScore()
    {
        score++;
        Debug.Log(score);
    }

    // Usefull for Adding and Subtracting, Adding -10 will subtract it.
    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
    }

    // References
    public int Score => score;
    public float MaxEnergy => maxEnergy;
    public float CurrentEnergy => currentEnergy;

    public bool IsEmpty() => currentEnergy <= 0.0001f;
}
