using UnityEngine;

namespace PowerCellEscape.UI
{
    using PowerCellEscape.Core;

    /// <summary>
    /// Toggleable help overlay (press I). Drawn with OnGUI so no UI assets are
    /// required. Honours high-contrast mode.
    /// </summary>
    public class InstructionsPanel : MonoBehaviour
    {
        private bool show = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) show = !show;
        }

        void OnGUI()
        {
            if (!show) return;

            var gm = GameManager.Instance;
            bool hc = gm != null && gm.HighContrast;
            Color fg = hc ? Color.yellow : Color.white;

            Rect r = new Rect(Screen.width / 2 - 230, Screen.height / 2 - 170, 460, 340);
            GUI.Box(r, "How to play");

            string body =
                "Collect 3 power cells (B1 B2 B3) and reach the exit.\n\n" +
                "WASD / Arrow keys : Move\n" +
                "R : Restart level\n" +
                "C : High-contrast mode\n" +
                "M : Mute / unmute     [ ] : Volume\n" +
                "I : Hide this help\n\n" +
                "Avoid the red enemy (it moves randomly).\n" +
                "Touching it ends the game. The exit stays LOCKED\n" +
                "until all cells are collected, then opens.";

            GUI.Label(new Rect(r.x + 22, r.y + 42, r.width - 44, r.height - 60), body, MakeStyle(fg, 18));
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
