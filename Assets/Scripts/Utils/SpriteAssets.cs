using UnityEngine;

namespace PowerCellEscape.Utils
{
    /// <summary>
    /// Loads the hand-drawn sprites from Resources/Sprites at runtime.
    /// Falls back to procedurally generated sprites if any asset is missing,
    /// so the game still works even when the PNGs are not yet imported.
    /// </summary>
    public static class SpriteAssets
    {
        private static Sprite _player;
        private static Sprite _enemy;
        private static Sprite _battery;
        private static Sprite _forbidden;
        private static Sprite _exitDoor;

        public static Sprite Player => _player ??= LoadOrFallback("Sprites/Player", () => GameArt.MakeRobotSprite());
        public static Sprite Enemy => _enemy ??= LoadOrFallback("Sprites/Enemy",
            () => GameArt.MakeSquareSprite(new Color(0.90f, 0.10f, 0.10f, 1f)));
        public static Sprite Battery => _battery ??= LoadOrFallback("Sprites/Battery",
            () => GameArt.MakeCircleSprite(Color.yellow));
        public static Sprite Forbidden => _forbidden ??= LoadOrFallback("Sprites/Forbidden",
            () => GameArt.MakeWallSprite(new Color(0.05f, 0.05f, 0.05f, 1f), 8, 8));
        public static Sprite ExitDoor => _exitDoor ??= LoadOrFallback("Sprites/ExitDoor",
            () => GameArt.MakeSquareSprite(Color.white, 64));

        private static Sprite LoadOrFallback(string path, System.Func<Sprite> fallback)
        {
            Sprite s = null;
            try
            {
                s = Resources.Load<Sprite>(path);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[SpriteAssets] Failed to load {path}: {e.Message}. Using fallback.");
            }

            if (s != null) return s;

            // The PNG may have been imported as a Texture2D instead of a Sprite.
            // Try loading it as a texture and creating a sprite on the fly.
            try
            {
                var tex = Resources.Load<Texture2D>(path);
                if (tex != null)
                {
                    s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    if (s != null) return s;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[SpriteAssets] Failed to create sprite from {path} texture: {e.Message}. Using fallback.");
            }

            return fallback();
        }
    }
}
