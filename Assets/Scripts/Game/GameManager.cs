using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Won,
    Lost
}

/// <summary>
/// Centraliza o estado da partida, a contagem de moedas e as condicoes
/// de vitoria, derrota e reinicio.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias")]
    public GameUI gameUI;

    public int Coins { get; private set; }
    public GameState State { get; private set; } = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (gameUI != null)
        {
            gameUI.UpdateCoins(Coins);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void AddCoin()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        Coins++;

        if (gameUI != null)
        {
            gameUI.UpdateCoins(Coins);
        }
    }

    public void Win()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        State = GameState.Won;

        if (gameUI != null)
        {
            gameUI.ShowVictory(Coins);
        }
    }

    public void Lose()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        State = GameState.Lost;

        if (gameUI != null)
        {
            gameUI.ShowDefeat(Coins);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
