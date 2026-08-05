using UnityEngine;

namespace PowerCellEscape.UI
{
    /// <summary>
    /// Draws a text label in screen space above a world object using OnGUI, so we
    /// avoid any font/material assets. Used for the battery labels (B1/B2/B3), the
    /// enemy "!" marker, and the exit LOCKED/OPEN status.
    /// </summary>
    public class WorldLabel : MonoBehaviour
    {
        public string text = "";
        public float yOffset = 0.6f;
        public int size = 24;
        public Color color = Color.white;
        // When true, the label colour flips between black (normal white
        // background) and white (high-contrast black background) so it stays
        // readable on either background.
        public bool autoContrast = true;

        void OnGUI()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Vector3 worldPos = transform.position + Vector3.up * yOffset;
            Vector3 screen = cam.WorldToScreenPoint(worldPos);
            if (screen.z < 0f) return; // behind camera

            Color actual = color;
            if (autoContrast)
            {
                bool hc = Core.GameManager.Instance != null && Core.GameManager.Instance.HighContrast;
                actual = hc ? Color.white : Color.black;
            }

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = actual;
            style.fontSize = size;
            style.font = GuiFonts.Builtin;

            GUI.Label(new Rect(screen.x - 50f, Screen.height - screen.y - 15f, 100f, 30f), text, style);
        }
    }
}
