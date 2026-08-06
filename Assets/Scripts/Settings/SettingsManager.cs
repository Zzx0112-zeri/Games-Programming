using UnityEngine;

namespace PowerCellEscape.Settings
{
    /// <summary>
    /// Handles global input for accessibility and audio, and reflects those
    /// settings onto the camera / AudioListener.
    ///   C  - toggle high-contrast mode
    ///   M  - mute / unmute
    ///   [  - decrease volume
    ///   ]  - increase volume
    ///   R  - return to the start page (from play or end screen)
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            ApplyAudio();
            ApplyContrast();
        }

        void Update()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;

            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                gm.TogglePause();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                gm.HighContrast = !gm.HighContrast;
                ApplyContrast();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                gm.Muted = !gm.Muted;
                ApplyAudio();
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                gm.Volume = Mathf.Max(0f, gm.Volume - 0.1f);
                ApplyAudio();
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                gm.Volume = Mathf.Min(1f, gm.Volume + 0.1f);
                ApplyAudio();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                gm.ReturnToMenu();
            }
        }

        void ApplyAudio()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            AudioListener.volume = gm.Muted ? 0f : gm.Volume;
        }

        /// <summary>Toggle mute from a UI button and re-apply the audio setting.</summary>
        public void ToggleMute()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            gm.Muted = !gm.Muted;
            ApplyAudio();
        }

        /// <summary>Toggle high-contrast mode from a UI button and re-apply it.</summary>
        public void ToggleContrast()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            gm.HighContrast = !gm.HighContrast;
            ApplyContrast();
        }

        void ApplyContrast()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            Camera cam = Camera.main;
            if (cam == null) return;
            // Normal mode = white background; high-contrast mode = black background.
            cam.backgroundColor = gm.HighContrast ? Color.black : Color.white;

            // Boundary is solid black on the white background. On the high-contrast
            // (black) background it flips to white so it stays visible.
            Color wallColor = gm.HighContrast ? Color.white : Color.black;
            var walls = FindObjectsOfType<SpriteRenderer>();
            foreach (var w in walls)
            {
                if (w.gameObject.name.StartsWith("Wall"))
                    w.color = wallColor;
            }
        }
    }
}
