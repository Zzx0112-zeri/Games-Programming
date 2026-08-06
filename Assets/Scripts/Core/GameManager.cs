using UnityEngine;

namespace PowerCellEscape.Core
{
    public enum GameState { Menu, Playing, Won, Lost }

    /// <summary>
    /// Central game state and rules. A single instance lives for the whole
    /// session. The flow is:
    ///   Menu  -> start page (Start / Exit buttons)
    ///   Playing -> the round is active
    ///   Won / Lost -> end screen (Restart / Exit Game buttons)
    /// Returning to the menu rebuilds the level so every round starts fresh.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static readonly int TotalCells = 3;
        public const float RoundTime = 180f;

        public GameState State { get; private set; } = GameState.Menu;
        public bool IsPaused { get; private set; } = false;
        public int CellsCollected { get; private set; } = 0;
        public float TimeRemaining { get; private set; } = RoundTime;

        // Accessibility / audio settings (mirrored by SettingsManager).
        public bool HighContrast { get; set; } = false;
        public float Volume { get; set; } = 0.8f;
        public bool Muted { get; set; } = false;

        public bool AllCellsCollected => CellsCollected >= TotalCells;
        public bool DidWin => State == GameState.Won;

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

        /// <summary>Freeze the round (physics + countdown). Drawn as a pause
        /// overlay by MenuManager. Toggled with P / Esc.</summary>
        public void Pause()
        {
            if (State != GameState.Playing || IsPaused) return;
            IsPaused = true;
            Time.timeScale = 0f;
        }

        /// <summary>Resume from the pause overlay.</summary>
        public void Resume()
        {
            if (!IsPaused) return;
            IsPaused = false;
            Time.timeScale = 1f;
        }

        /// <summary>Toggle the pause overlay (only meaningful while Playing).</summary>
        public void TogglePause()
        {
            if (State != GameState.Playing) return;
            if (IsPaused) Resume(); else Pause();
        }

        /// <summary>Begin a fresh round from the start page.</summary>
        public void StartGame()
        {
            LevelBuilder.Build();      // rebuild a clean level
            CellsCollected = 0;
            TimeRemaining = RoundTime;
            IsPaused = false;
            Time.timeScale = 1f;
            State = GameState.Playing;
        }

        /// <summary>Return to the start page. Used by the end-screen Restart
        /// button and the R key. Rebuilds the level so the backdrop is fresh.</summary>
        public void ReturnToMenu()
        {
            LevelBuilder.Build();
            CellsCollected = 0;
            TimeRemaining = RoundTime;
            IsPaused = false;
            Time.timeScale = 1f;
            State = GameState.Menu;
        }
    }
}
