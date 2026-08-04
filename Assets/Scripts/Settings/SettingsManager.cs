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
    ///   R  - restart the level
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        void Awake()
        {
            ApplyAudio();
            ApplyContrast();
        }

        void Update()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;

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
                gm.Restart();
            }
        }

        void ApplyAudio()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            AudioListener.volume = gm.Muted ? 0f : gm.Volume;
        }

        void ApplyContrast()
        {
            var gm = Core.GameManager.Instance;
            if (gm == null) return;
            Camera cam = Camera.main;
            if (cam == null) return;
            cam.backgroundColor = gm.HighContrast
                ? new Color(0f, 0f, 0f, 1f)
                : new Color(0.10f, 0.10f, 0.15f, 1f);
        }
    }
}
