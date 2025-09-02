using UnityEngine;

public class Device : MonoBehaviour
{
    [Header("Device Settings")]
    [SerializeField] private string deviceName;
    [SerializeField] private GameObject deviceGameObjectOn;
    [SerializeField] private GameObject deviceGameObjectOff;
    [SerializeField] private float drainPerSeconds;

    private bool isOn = false;
    private int originalLayer;

    public bool IsOn => isOn;
    public float DrainPerSeconds => drainPerSeconds;

    private void Awake()
    {
        if (isOn)
            isOn = false;

        SetupDevice();
    }

    public void TurnOnDevice()
    {
        if (!isOn)
        {
            deviceGameObjectOn.SetActive(true);
            deviceGameObjectOff.SetActive(false);
        }

        isOn = true;
    }


    private void TurnOffDevice()
    {
        if (isOn)
        {
            deviceGameObjectOff.SetActive(true);
            deviceGameObjectOn.SetActive(false);
        }

        isOn = false;
    }
    private void OnMouseDown()
    {
        if (isOn)
        {
            TurnOffDevice();
            GameManager.Instance.AddScore();
            GameManager.Instance.AddEnergy(1);
        }
    }
   
    // Gizmo to see current state in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isOn ? Color.yellow : Color.gray;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, 0.1f);
    }
    private void SetupDevice()
    {
        deviceGameObjectOn.SetActive(false);
        deviceGameObjectOff.SetActive(true);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Tooltip manager
    private void OnMouseOver()
    {
        if (originalLayer == 0)
            originalLayer = gameObject.layer;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Outline"));

        TooltipManager.Instance?.Show(deviceName, drainPerSeconds, isOn);

         // change layer to outline layer.
    }

    private void OnMouseExit()
    {
        SetLayerRecursively(gameObject, originalLayer);

        TooltipManager.Instance?.Hide();

        // Remove outline layer
    }

    private void OnDisable()
    {
        // prevent stuck tooltip if object is disabled while hovered
        if (TooltipManager.Instance) TooltipManager.Instance.Hide();
    }
}
