using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text gameOverText;
    
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject player; 

    private bool isGameActive = false;
    private Campfire campfire;

    void Start()
    {
        ShowMenu();
        HideGameObjects(); 

        campfire = FindAnyObjectByType<Campfire>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameActive && campfire != null)
        {
            CheckForPlayerDeath();
        }
    }

    public void StartGame()
    {
        ResetGameState();
        
        ShowGame();
        ShowGameObjects(); 

        if (campfire != null)
        {
            campfire.Health = campfire.MaxHealth;
        }

        if (player != null)
        {
            player.transform.position = Vector3.zero;
            player.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    void ResetGameState()
    {
        isGameActive = false;
        
        CancelInvoke(nameof(ReturnToMenu));
        
        if (campfire == null)
        {
            campfire = FindAnyObjectByType<Campfire>();
        }
    }

    void CheckForPlayerDeath()
    {
        if (campfire != null && campfire.Health <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (!isGameActive) return;
        
        isGameActive = false;
        Time.timeScale = 0.5f; 

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over!";
        }

        Invoke(nameof(ReturnToMenu), 0.5f);
    }

    void ReturnToMenu()
    {
        ShowMenu();
        HideGameObjects();
        Time.timeScale = 0f; 
    }

    void ShowMenu()
    {
        menuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        Time.timeScale = 0f;
        isGameActive = false;
    }

    void ShowGame()
    {
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        Time.timeScale = 1f;
        isGameActive = true;
    }

    void ShowGameObjects()
    {
        if (gridParent != null) gridParent.SetActive(true);
        if (player != null) player.SetActive(true);  
    }

    void HideGameObjects()
    {
        if (gridParent != null) gridParent.SetActive(false);
        if (player != null) player.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}