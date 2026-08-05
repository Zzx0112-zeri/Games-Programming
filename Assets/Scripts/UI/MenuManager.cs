using UnityEngine;

namespace PowerCellEscape.UI
{
    using PowerCellEscape.Core;

    /// <summary>
    /// Draws the two full-screen overlay pages with immediate-mode GUI (OnGUI),
    /// so no UI assets are needed:
    ///   - Start page (GameState.Menu): title + "Start" and "Exit" buttons.
    ///   - End screen (GameState.Won / Lost): result text + "Restart" and
    ///     "Exit Game" buttons.
    /// The level keeps running (frozen) behind a dim panel; only the buttons are
    /// interactive. Honours high-contrast mode.
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        private Texture2D dimTex;

        void Awake()
        {
            dimTex = new Texture2D(1, 1);
            dimTex.SetPixel(0, 0, Color.white);
            dimTex.Apply();
        }

        void OnGUI()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            if (gm.State == GameState.Menu)
                DrawStartMenu(gm);
            else if (gm.State == GameState.Won || gm.State == GameState.Lost)
                DrawEndScreen(gm);
        }

        private void DrawStartMenu(GameManager gm)
        {
            bool hc = gm.HighContrast;
            DrawDim(hc ? 0.85f : 0.6f);

            Color fg = hc ? Color.yellow : Color.white;
            float cx = Screen.width / 2f;

            GUI.Label(new Rect(cx - 320, Screen.height * 0.27f, 640, 72),
                "POWER CELL ESCAPE", TitleStyle(fg));

            GUI.Label(new Rect(cx - 340, Screen.height * 0.27f + 80, 680, 40),
                "Collect 3 power cells, reach the exit, and avoid the red enemy.",
                SubStyle(fg));

            var btn = ButtonStyle(fg);
            float bw = 240f, bh = 56f;
            float by = Screen.height * 0.52f;

            if (GUI.Button(new Rect(cx - bw / 2f, by, bw, bh), "Start", btn))
                gm.StartGame();

            if (GUI.Button(new Rect(cx - bw / 2f, by + 72f, bw, bh), "Exit", btn))
                QuitGame();
        }

        private void DrawEndScreen(GameManager gm)
        {
            bool hc = gm.HighContrast;
            DrawDim(hc ? 0.85f : 0.6f);

            Color fg = hc ? Color.yellow : Color.white;
            float cx = Screen.width / 2f;

            string title = gm.DidWin ? "YOU ESCAPED!" : "GAME OVER";
            Color titleColor = gm.DidWin ? Color.green : Color.red;
            GUI.Label(new Rect(cx - 320, Screen.height * 0.29f, 640, 72),
                title, TitleStyle(titleColor));

            var btn = ButtonStyle(fg);
            float bw = 260f, bh = 56f;
            float by = Screen.height * 0.49f;

            if (GUI.Button(new Rect(cx - bw / 2f, by, bw, bh), "Restart", btn))
                gm.ReturnToMenu();

            if (GUI.Button(new Rect(cx - bw / 2f, by + 72f, bw, bh), "Exit Game", btn))
                QuitGame();
        }

        private void DrawDim(float alpha)
        {
            if (dimTex == null) return;
            Color prev = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), dimTex);
            GUI.color = prev;
        }

        private GUIStyle TitleStyle(Color c)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.alignment = TextAnchor.MiddleCenter;
            s.fontSize = 44;
            s.fontStyle = FontStyle.Bold;
            s.normal.textColor = c;
            s.font = GuiFonts.Builtin;
            return s;
        }

        private GUIStyle SubStyle(Color c)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.alignment = TextAnchor.MiddleCenter;
            s.fontSize = 18;
            s.normal.textColor = c;
            s.font = GuiFonts.Builtin;
            return s;
        }

        private GUIStyle ButtonStyle(Color c)
        {
            var s = new GUIStyle(GUI.skin.button);
            s.alignment = TextAnchor.MiddleCenter;
            s.fontSize = 26;
            s.fontStyle = FontStyle.Bold;
            s.normal.textColor = c;
            s.font = GuiFonts.Builtin;
            return s;
        }

        private void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
