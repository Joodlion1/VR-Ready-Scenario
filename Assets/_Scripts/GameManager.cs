using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Session stats
    public float FinalConfidence;
    public float LookPercentage;
    public int LongPauseCount;

    public float TotalTime;
    public string SelectedDifficulty;
    public bool HasResults = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public string GetGrade()
    {
        if (FinalConfidence >= 0.75f) return "A";
        if (FinalConfidence >= 0.60f) return "B";
        if (FinalConfidence >= 0.45f) return "C";
        if (FinalConfidence >= 0.30f) return "D";
        if (FinalConfidence >= 0.15f) return "E";
        return "F";
    }
}