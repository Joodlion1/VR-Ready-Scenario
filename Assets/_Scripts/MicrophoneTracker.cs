using UnityEngine;

public class MicrophoneTracker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int sampleWindow = 128;
    [SerializeField] private float speakingThreshold = 0.01f;

    private AudioClip micClip;
    private string micName;

    public bool IsSpeaking { get; private set; }
    public float CurrentVolume { get; private set; }
    public float SilenceTimer { get; private set; }
    public float TotalSpeakingTime { get; private set; }
    public float TotalTime { get; private set; }

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone detected!");
            return;
        }
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 20, 44100);
        Debug.Log($"Microphone started: {micName}");
    }

    public int LongPauseCount { get; private set; }
private bool wasInLongPause = false;

    void Update()
    {
        CurrentVolume = GetMicVolume();
        IsSpeaking = CurrentVolume > speakingThreshold;
        TotalTime += Time.deltaTime;

        if (IsSpeaking)
        {
            TotalSpeakingTime += Time.deltaTime;
            wasInLongPause = false;
            SilenceTimer = 0f;
        }
        else
        {
            SilenceTimer += Time.deltaTime;

            if (SilenceTimer > longPauseThreshold && !wasInLongPause)
            {
                LongPauseCount++;
                wasInLongPause = true;
                Debug.Log($"Long pause detected! Total: {LongPauseCount}");
            }
        }
    }



    [SerializeField] private float longPauseThreshold = 3f;

    private float GetMicVolume()
    {
        if (micClip == null) return 0f;

        float[] samples = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micName) - sampleWindow;
        if (micPosition < 0) return 0f;

        micClip.GetData(samples, micPosition);

        float levelMax = 0f;
        foreach (var sample in samples)
        {
            float wavePeak = Mathf.Abs(sample);
            if (wavePeak > levelMax)
                levelMax = wavePeak;
        }
        return levelMax;
    }


    public float GetNormalizedVolume()
    {
        return Mathf.Clamp01(CurrentVolume / 0.1f);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Speaking: {IsSpeaking}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Volume: {CurrentVolume:F4}");
        GUI.Label(new Rect(10, 50, 300, 20), $"Silence: {SilenceTimer:F1}s");
    }

    void OnDestroy()
    {
        if (Microphone.IsRecording(micName))
            Microphone.End(micName);
    }
}