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
            if (gm == null) return;

            bool hc = gm.HighContrast;
            Color fg = hc ? Color.yellow : Color.white;

            GUI.Label(new Rect(12, 10, 320, 30), "Time: " + Mathf.CeilToInt(gm.TimeRemaining) + "s", MakeStyle(fg, 24));
            GUI.Label(new Rect(12, 44, 320, 30), "Cells: " + gm.CellsCollected + " / " + GameManager.TotalCells, MakeStyle(fg, 24));

            if (gm.State == GameState.Won)
            {
                CenteredBanner("YOU ESCAPED!", Color.green, hc);
                GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 14, 300, 30),
                    "Press R to restart", MakeStyle(fg, 20));
            }
            else if (gm.State == GameState.Lost)
            {
                CenteredBanner("GAME OVER", Color.red, hc);
                GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 14, 300, 30),
                    "Press R to restart", MakeStyle(fg, 20));
            }
        }

        private void CenteredBanner(string text, Color color, bool hc)
        {
            Color c = hc ? color : color;
            GUI.Label(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 50, 360, 60),
                text, MakeStyle(c, 42));
        }

        private GUIStyle MakeStyle(Color c, int size)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = c;
            s.fontSize = size;
            s.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            return s;
        }
    }
}
