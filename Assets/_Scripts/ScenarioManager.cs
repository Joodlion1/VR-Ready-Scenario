using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private Transform xrCamera;

    [Header("Settings")]
    [SerializeField] private float menuDistance = 3.0f;

    private InputAction menuAction;
    private bool isMenuOpen = false;

    void Start()
    {
        menuAction = new InputAction(binding: "<QuestTouchPlusController>{LeftHand}/menu");
        menuAction.Enable();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ShowMenu();
        }
        else
        {
            HideMenu();
        }
    }

    void Update()
    {
        if (menuAction.WasPressedThisFrame())
        {
            Debug.Log("Menu button pressed!");
            if (SceneManager.GetActiveScene().name == "MainMenu") return;
            ToggleMenu();
        }
        
        if (menuAction.IsPressed())
        {
            Debug.Log("Menu action value: " + menuAction.ReadValue<float>());
        }
    }

    void OnDestroy()
    {
        menuAction.Disable();
    }

    private void ToggleMenu()
    {
        if (isMenuOpen) HideMenu();
        else
        {
            PositionMenuInFrontOfPlayer();
            ShowMenu();
        }
    }

    private void ShowMenu()
    {
        isMenuOpen = true;
        menuCanvas.SetActive(true);
    }

    private void HideMenu()
    {
        isMenuOpen = false;
        menuCanvas.SetActive(false);
    }

    private void PositionMenuInFrontOfPlayer()
    {
        Vector3 forward = xrCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        menuCanvas.transform.position = xrCamera.position + forward * menuDistance;
        menuCanvas.transform.rotation = Quaternion.LookRotation(forward);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void LoadSmall()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SelectedDifficulty = "Small";
        SceneManager.LoadScene("Small_Classroom");
    }

    public void LoadMedium()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SelectedDifficulty = "Medium";
        SceneManager.LoadScene("Medium_Classroom");
    }

    public void LoadLarge()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SelectedDifficulty = "Large";
        SceneManager.LoadScene("Large_Classroom");
    }
    
    public void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    //public void QuitGame()     => Application.Quit();
}