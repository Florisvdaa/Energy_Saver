using UnityEngine;

[CreateAssetMenu(fileName = "Device", menuName = "ScriptableObjects/DeviceSO")]
public class DeviceSO : ScriptableObject
{
    [Header("Device Settings")]
    public string deviceName;
    public GameObject visualPrefab;
    public float drainPerSecond;
}
