using UnityEngine;

namespace PowerCellEscape.Utils
{
    /// <summary>
    /// Generates all sprites procedurally at runtime so the project ships with
    /// zero external image assets. Every method returns a Sprite built from a
    /// Texture2D drawn in code.
    /// </summary>
    public static class GameArt
    {
        public static Sprite MakeSquareSprite(Color color, int size = 64)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        public static Sprite MakeCircleSprite(Color color, int size = 64)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f - 2f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                    pixels[y * size + x] = d <= radius ? color : Color.clear;
                }
            }
            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        /// <summary>Simple friendly robot: a rounded blue body with two white eyes.</summary>
        public static Sprite MakeRobotSprite(int size = 64)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.clear;

            Color body = new Color(0.20f, 0.60f, 0.90f, 1f);
            Color trim = new Color(0.12f, 0.40f, 0.65f, 1f);
            Color eye = Color.white;

            int bw = size * 3 / 4;
            int bh = size * 3 / 4;
            int bx = (size - bw) / 2;
            int by = (size - bh) / 2;

            for (int y = by; y < by + bh; y++)
            {
                for (int x = bx; x < bx + bw; x++)
                {
                    bool corner = (x == bx || x == bx + bw - 1) && (y == by || y == by + bh - 1);
                    if (corner) continue;
                    bool border = (x == bx || x == bx + bw - 1 || y == by || y == by + bh - 1);
                    pixels[y * size + x] = border ? trim : body;
                }
            }

            int ew = Mathf.Max(2, size / 8);
            int ey0 = size * 2 / 5;
            for (int y = ey0; y < ey0 + ew; y++)
            {
                for (int x = size * 2 / 5; x < size * 2 / 5 + ew; x++) pixels[y * size + x] = eye;
                for (int x = size * 3 / 5; x < size * 3 / 5 + ew; x++) pixels[y * size + x] = eye;
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        public static Sprite MakeWallSprite(Color color, int w = 8, int h = 8)
        {
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[w * h];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
        }
    }
}
