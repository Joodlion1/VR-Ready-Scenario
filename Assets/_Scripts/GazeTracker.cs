using UnityEngine;

public class GazeTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform xrCamera;
    [SerializeField] private Transform[] audiencePoints; // multiple points

    [Header("Settings")]
    [SerializeField] private float lookThreshold = 0.5f; // wider angle

    public bool IsLookingAtAudience { get; private set; }
    public float LookTime { get; private set; }
    public float TotalTime { get; private set; }

    void Update()
    {
        if (xrCamera == null || audiencePoints.Length == 0) return;

        TotalTime += Time.deltaTime;
        IsLookingAtAudience = CheckLooking();

        if (IsLookingAtAudience)
            LookTime += Time.deltaTime;
    }

    private bool CheckLooking()
    {
        // llooking at audience if looking at ANY of thes defined points
        foreach (Transform point in audiencePoints)
        {
            Vector3 toPoint = (point.position - xrCamera.position).normalized;
            float dot = Vector3.Dot(xrCamera.forward, toPoint);
            if (dot > lookThreshold)
                return true;
        }
        return false;
    }

    public float GetLookPercentage()
    {
        if (TotalTime == 0f) return 0f;
        return LookTime / TotalTime;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 70, 300, 20), $"Looking at audience: {IsLookingAtAudience}");
        GUI.Label(new Rect(10, 90, 300, 20), $"Look %: {GetLookPercentage():P0}");
    }
}