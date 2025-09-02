using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Rooms to move (their Y/Z can be equal)")]
    public Transform[] rooms;            // e.g. at x = -35, 0, 35

    [Header("Navigation")]
    public int startIndex = 1;           // 0 = left, 1 = middle, 2 = right
    public float moveDuration = 0.6f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3[] basePos;                   // original positions
    float currentOffsetX;                // how far we’ve shifted everything
    int index;
    bool isMoving;

    void Awake()
    {
        if (rooms == null || rooms.Length == 0)
            return;

        basePos = new Vector3[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
            basePos[i] = rooms[i].position;

        index = Mathf.Clamp(startIndex, 0, rooms.Length - 1);

        // Center selected room at camera X (assume camera is at X = 0; change if needed)
        currentOffsetX = -basePos[index].x;
        ApplyOffset(currentOffsetX);
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
}
