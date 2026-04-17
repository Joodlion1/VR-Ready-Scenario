using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableDoor : MonoBehaviour
{
    [SerializeField] private ConfidenceManager confidencemanager;
    [SerializeField] private MicrophoneTracker micTracker;
    [SerializeField] private GazeTracker gazeTracker;

    private void Start()
    {
        if (confidencemanager == null) confidencemanager = FindFirstObjectByType<ConfidenceManager>();
        if (micTracker == null) micTracker = FindFirstObjectByType<MicrophoneTracker>();
        if (gazeTracker == null) gazeTracker = FindFirstObjectByType<GazeTracker>();

        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnDoorClicked);
    }

    private void OnDoorClicked(SelectEnterEventArgs args)
    {
        Debug.Log($"[InteractableDoor] Door clicked by {args.interactorObject} — saving stats and loading MainMenu.");
        SaveStats();
        Debug.Log($"[InteractableDoor] SaveStats done. HasResults={GameManager.Instance?.HasResults}. Loading MainMenu...");
        SceneManager.LoadScene("MainMenu");
    }

    private void SaveStats()
    {
        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogWarning("InteractableDoor: GameManager.Instance is null — skipping stat save.");
            return;
        }

        if (confidencemanager != null) gm.FinalConfidence = confidencemanager.ConfidenceScore;
        if (gazeTracker != null) gm.LookPercentage = gazeTracker.GetLookPercentage();
        if (micTracker != null)
        {
            gm.LongPauseCount = micTracker.LongPauseCount;
            gm.TotalTime = micTracker.TotalTime;
        }
        gm.HasResults = true;
    }
}