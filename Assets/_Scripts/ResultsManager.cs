using UnityEngine;
using TMPro;
using UnityEditor;

public class ResultsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject resultsCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private SessionLogger sessionLogger;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI confidenceText;
    [SerializeField] private TextMeshProUGUI gazeText;
    [SerializeField] private TextMeshProUGUI silenceText;
    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {
        Debug.Log($"HasResults: {GameManager.Instance?.HasResults}");
        Debug.Log($"MenuCanvas active before: {menuCanvas.activeSelf}");
       
        if (GameManager.Instance != null && GameManager.Instance.HasResults)
        {
            menuCanvas.SetActive(false);
            Debug.Log($"MenuCanvas active after SetActive(false): {menuCanvas.activeSelf}");
            ShowResults();
        }
        else
        {
            menuCanvas.SetActive(true);
            resultsCanvas.SetActive(false);
        }
    }

    private void ShowResults()
    {
        sessionLogger.LogSession();
        resultsCanvas.SetActive(true);

        string grade = GameManager.Instance.GetGrade();
        float confidence = GameManager.Instance.FinalConfidence;
        float gaze = GameManager.Instance.LookPercentage;
        float time = GameManager.Instance.TotalTime;

        gradeText.text = grade;
        difficultyText.text = $"Difficulty: {GameManager.Instance.SelectedDifficulty}";
        confidenceText.text = $"Confidence: {confidence * 100f:F0}%";
        gazeText.text = $"Eye Contact: {gaze * 100f:F0}%";
        silenceText.text = $"Long Pauses: {GameManager.Instance.LongPauseCount}";
        timeText.text = $"Time: {Mathf.FloorToInt(time / 60)}m {Mathf.FloorToInt(time % 60)}s";

        // reset results flag
        GameManager.Instance.HasResults = false;
    }

    public void BackToMenu()
    {
        resultsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        GameManager.Instance.HasResults = false;
    }
}