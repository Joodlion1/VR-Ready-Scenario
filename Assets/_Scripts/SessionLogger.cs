using UnityEngine;
using System.IO;
using System;

public class SessionLogger : MonoBehaviour
{
    public void LogSession()
    {
        if (GameManager.Instance == null || !GameManager.Instance.HasResults) return;

        string filePath = Path.Combine(Application.persistentDataPath, "vr-session_log.csv");

        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "Date,Difficulty,Confidence,EyeContact,LongPauses,Time,Grade\n");

        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string difficulty = GameManager.Instance.SelectedDifficulty;
        float confidence = GameManager.Instance.FinalConfidence;
        float gaze = GameManager.Instance.LookPercentage;
        int pauses = GameManager.Instance.LongPauseCount;
        float time = GameManager.Instance.TotalTime;
        string grade = GameManager.Instance.GetGrade();

        string line = $"{date},{difficulty},{confidence * 100f:F0},{gaze * 100f:F0},{pauses},{time:F0},{grade}\n";

        File.AppendAllText(filePath, line);
        Debug.Log($"Session logged to: {filePath}");
    }
}