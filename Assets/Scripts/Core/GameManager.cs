using UnityEngine;
using UnityEngine.SceneManagement;

namespace PowerCellEscape.Core
{
    public enum GameState { Playing, Won, Lost }

    /// <summary>
    /// Central game state and rules. A single instance lives for the duration of
    /// a level; on restart the scene is reloaded so a fresh instance is created.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static readonly int TotalCells = 3;
        public const float RoundTime = 180f;

        public GameState State { get; private set; } = GameState.Playing;
        public int CellsCollected { get; private set; } = 0;
        public float TimeRemaining { get; private set; } = RoundTime;

        // Accessibility / audio settings (mirrored by SettingsManager).
        public bool HighContrast { get; set; } = false;
        public float Volume { get; set; } = 0.8f;
        public bool Muted { get; set; } = false;

        public bool AllCellsCollected => CellsCollected >= TotalCells;

        public event System.Action OnCellCollected;
        public event System.Action OnWin;
        public event System.Action OnLose;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Update()
        {
            if (State != GameState.Playing) return;
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0f)
            {
                TimeRemaining = 0f;
                Lose();
            }
        }

        public void CollectCell()
        {
            if (State != GameState.Playing) return;
            CellsCollected++;
            OnCellCollected?.Invoke();
        }

        public void Win()
        {
            if (State != GameState.Playing) return;
            State = GameState.Won;
            OnWin?.Invoke();
        }

        public void Lose()
        {
            if (State != GameState.Playing) return;
            State = GameState.Lost;
            OnLose?.Invoke();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
