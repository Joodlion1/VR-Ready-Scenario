using UnityEngine;

public class ConfidenceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MicrophoneTracker micTracker;
    [SerializeField] private GazeTracker gazeTracker;

    [Header("Weights")]
    [SerializeField] private float volumeWeight = 0.4f;
    [SerializeField] private float gazeWeight = 0.3f;

    public float ConfidenceScore { get; private set; }

    void Update()
    {
        ConfidenceScore = CalculateConfidence();
    }

    private float CalculateConfidence()
    {
        float volumeScore = micTracker.GetNormalizedVolume();
        float gazeScore = gazeTracker.GetLookPercentage();

        // Penalty per long pause, max penalty of 0.3
        float pausePenalty = Mathf.Min(micTracker.LongPauseCount * 0.05f, 0.3f);

        float score =
            volumeWeight * volumeScore +
            gazeWeight * gazeScore -
            pausePenalty;

        return Mathf.Clamp01(score);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 110, 300, 20), $"Confidence: {ConfidenceScore:F2}");
    }
}