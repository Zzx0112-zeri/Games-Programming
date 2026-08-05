using UnityEngine;

namespace PowerCellEscape.UI
{
    using PowerCellEscape.Core;

    /// <summary>
    /// Persistent heads-up display drawn with immediate-mode GUI (OnGUI): time
    /// left, cells collected, lives, and the win/lose banner. Honours
    /// high-contrast mode by switching to bright text on black.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        void OnGUI()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.State != GameState.Playing) return;

            bool hc = gm.HighContrast;
            Color fg = hc ? Color.yellow : Color.black;

            GUI.Label(new Rect(12, 10, 320, 30), "Time: " + Mathf.CeilToInt(gm.TimeRemaining) + "s", MakeStyle(fg, 24));
            GUI.Label(new Rect(12, 44, 320, 30), "Cells: " + gm.CellsCollected + " / " + GameManager.TotalCells, MakeStyle(fg, 24));
        }

        private GUIStyle MakeStyle(Color c, int size)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = c;
            s.fontSize = size;
            s.font = GuiFonts.Builtin;
            return s;
        }
    }
}
