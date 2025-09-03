using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private GameObject mainLighting;
    [SerializeField] private bool isUnlocked = false;

    [Header("Materials")]
    [SerializeField] private Material blackMaterial;

    private Material[][] originalMaterials;
    private Renderer[] roomRenderers;
    public bool IsUnlocked => isUnlocked;


    // Untiy registery
    private void Awake()
    {
        roomRenderers = GetComponentsInChildren<MeshRenderer>(true);

        CacheOriginalMaterials();
        ApplyRoomState();
    }

    private void CacheOriginalMaterials()
    {
        originalMaterials = new Material[roomRenderers.Length][];
        for (int i = 0; i < roomRenderers.Length; i++)
        {
            originalMaterials[i] = roomRenderers[i].materials;
        }
    }

    private void ApplyRoomState()
    {
        if (isUnlocked)
            UnlockRoom();
        else
            LockRoom();
    }

    public void UnlockRoom()
    {
        isUnlocked = true;
        mainLighting.SetActive(true);

        for (int i = 0; i < roomRenderers.Length; i++)
        {
            roomRenderers[i].materials = originalMaterials[i];
        }
    }

    public void LockRoom()
    {
        isUnlocked = false;
        mainLighting.SetActive(false);

        for (int i = 0; i < roomRenderers.Length; i++)
        {
            Material[] blackMats = new Material[roomRenderers[i].materials.Length];
            for (int j = 0; j < blackMats.Length; j++)
            {
                blackMats[j] = blackMaterial;
            }
            roomRenderers[i].materials = blackMats;
        }
    }

}
