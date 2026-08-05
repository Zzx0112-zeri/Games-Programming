using UnityEngine;

namespace PowerCellEscape.Core
{
    using PowerCellEscape.Enemy;
    using PowerCellEscape.Items;
    using PowerCellEscape.Player;
    using PowerCellEscape.Utils;

    /// <summary>
    /// Builds the entire level in code: camera, room walls, player, the three
    /// power cells, the exit door and the wandering enemy. Called by
    /// GameBootstrap once the scene has loaded, and again whenever a fresh round
    /// starts or the player returns to the menu (so the level is rebuilt clean).
    /// All level objects live under a "LevelRoot" GameObject that is destroyed
    /// and recreated on each Build().
    /// </summary>
    public class LevelBuilder
    {
        public static void Build()
        {
            // Remove any previously built level so we start from a clean slate.
            var prev = GameObject.Find("LevelRoot");
            if (prev != null) Object.Destroy(prev);

            var root = new GameObject("LevelRoot");
            Transform parent = root.transform;

            // ---- Camera (reuse the existing MainCamera) -----------------
            Camera cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("MainCamera");
                camGo.tag = "MainCamera";
                cam = camGo.AddComponent<Camera>();
            }
            cam.orthographic = true;
            cam.orthographicSize = 6.5f;
            cam.transform.position = new Vector3(0f, 0f, -10f);
            cam.transform.rotation = Quaternion.identity;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.white;

            // ---- Room dimensions ----------------------------------------
            const float halfX = 9f;
            const float halfY = 5f;
            const float wall = 0.6f;

            // A single rectangular frame that encloses the play area. The four
            // edges are parented under one "Boundary" GameObject so they read as
            // one continuous box rather than four separate walls.
            var boundary = new GameObject("Boundary");
            boundary.transform.SetParent(parent);
            CreateWall(boundary.transform, "WallTop", new Vector3(0f, halfY + wall / 2f, 0f), new Vector2((halfX + wall) * 2f, wall));
            CreateWall(boundary.transform, "WallBottom", new Vector3(0f, -halfY - wall / 2f, 0f), new Vector2((halfX + wall) * 2f, wall));
            CreateWall(boundary.transform, "WallLeft", new Vector3(-halfX - wall / 2f, 0f), new Vector2(wall, (halfY + wall) * 2f));
            CreateWall(boundary.transform, "WallRight", new Vector3(halfX + wall / 2f, 0f), new Vector2(wall, (halfY + wall) * 2f));

            // ---- Player -------------------------------------------------
            var player = new GameObject("Player");
            player.tag = "Player";
            var pSr = player.AddComponent<SpriteRenderer>();
            pSr.sprite = SpriteAssets.Player;
            pSr.sortingOrder = 2;
            // Shrink the icon so its on-screen size matches the collider (0.9 x 0.9 world units).
            // The collider's local size is set to the sprite's native world size so that, after
            // the object is scaled, the collider's world size stays exactly 0.9.
            float pSw = SpriteAssets.Player != null ? SpriteAssets.Player.bounds.size.x : 0.9f;
            if (pSw > 0f) player.transform.localScale = new Vector3(0.9f / pSw, 0.9f / pSw, 1f);
            var pRb = player.AddComponent<Rigidbody2D>();
            pRb.gravityScale = 0f;
            pRb.freezeRotation = true;
            pRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            var pCol = player.AddComponent<BoxCollider2D>();
            pCol.size = new Vector2(pSw, pSw);
            player.transform.position = new Vector3(0f, -3.5f, 0f);
            player.transform.SetParent(parent);
            player.AddComponent<PlayerController>();

            // ---- Power cells --------------------------------------------
            SpawnBattery(parent, "B1", new Vector3(-6f, 0f, 0f));
            SpawnBattery(parent, "B2", new Vector3(6f, 0f, 0f));
            SpawnBattery(parent, "B3", new Vector3(0f, 3f, 0f));

            // ---- Exit door (top centre) --------------------------------
            var exit = new GameObject("Exit");
            var eSr = exit.AddComponent<SpriteRenderer>();
            eSr.sprite = SpriteAssets.Forbidden;
            eSr.sortingOrder = 1;
            // Scale the exit icon to a 1.6 x 1.0 world-unit trigger area; the collider
            // local size is set to the sprite's native world size so scaling keeps the
            // world collider exactly 1.6 x 1.0.
            Vector2 exitTarget = new Vector2(1.6f, 1.0f);
            float exitW = SpriteAssets.ExitDoor != null ? SpriteAssets.ExitDoor.bounds.size.x : exitTarget.x;
            float exitH = SpriteAssets.ExitDoor != null ? SpriteAssets.ExitDoor.bounds.size.y : exitTarget.y;
            if (exitW > 0f && exitH > 0f)
                exit.transform.localScale = new Vector3(exitTarget.x / exitW, exitTarget.y / exitH, 1f);
            exit.transform.position = new Vector3(0f, halfY - 0.3f, 0f);
            var eCol = exit.AddComponent<BoxCollider2D>();
            // Solid (non-trigger) collider: while locked the forbidden sign physically
            // blocks the player; once open, proximity still triggers the win.
            eCol.isTrigger = false;
            eCol.size = new Vector2(exitW, exitH);
            exit.transform.SetParent(parent);
            exit.AddComponent<ExitDoor>();

            // ---- Enemy --------------------------------------------------
            var enemy = new GameObject("Enemy");
            var enSr = enemy.AddComponent<SpriteRenderer>();
            enSr.sprite = SpriteAssets.Enemy;
            enSr.sortingOrder = 2;
            // Shrink the icon to match the collider (0.9 x 0.9 world units).
            float enSw = SpriteAssets.Enemy != null ? SpriteAssets.Enemy.bounds.size.x : 0.9f;
            if (enSw > 0f) enemy.transform.localScale = new Vector3(0.9f / enSw, 0.9f / enSw, 1f);
            var enRb = enemy.AddComponent<Rigidbody2D>();
            enRb.gravityScale = 0f;
            enRb.freezeRotation = true;
            enRb.bodyType = RigidbodyType2D.Kinematic;
            var enCol = enemy.AddComponent<BoxCollider2D>();
            enCol.isTrigger = true;
            enCol.size = new Vector2(enSw, enSw);
            enemy.transform.position = new Vector3(-5f, -2f, 0f);
            enemy.transform.SetParent(parent);
            enemy.AddComponent<PatrollingEnemy>();
        }

        private static Sprite _whiteUnitSprite;
        private static Sprite WhiteUnitSprite()
        {
            if (_whiteUnitSprite == null)
            {
                var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                _whiteUnitSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            }
            return _whiteUnitSprite;
        }

        private static void CreateWall(Transform parent, string name, Vector3 position, Vector2 size)
        {
            var wall = new GameObject(name);
            var sr = wall.AddComponent<SpriteRenderer>();
            // A plain 1x1 white sprite, scaled to the wall dimensions, drawn black so
            // the frame is visible on the white background regardless of when
            // SettingsManager runs its tint pass. SettingsManager still recolours it
            // (black on white, white on high-contrast) when it later toggles.
            sr.sprite = WhiteUnitSprite();
            sr.color = Color.black;
            sr.sortingOrder = 0;
            wall.transform.position = position;
            wall.transform.localScale = new Vector3(size.x, size.y, 1f);
            wall.AddComponent<BoxCollider2D>();
            wall.transform.SetParent(parent);
        }

        private static void SpawnBattery(Transform parent, string label, Vector3 position)
        {
            var b = new GameObject("Battery_" + label);
            var sr = b.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteAssets.Battery;
            sr.sortingOrder = 2;
            // Shrink the icon to match the collider (radius 0.4 -> diameter 0.8 world units).
            float bSw = SpriteAssets.Battery != null ? SpriteAssets.Battery.bounds.size.x : 0.8f;
            if (bSw > 0f) b.transform.localScale = new Vector3(0.8f / bSw, 0.8f / bSw, 1f);
            b.transform.position = position;
            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = bSw / 2f; // local; world radius = (bSw/2) * (0.8/bSw) = 0.4
            b.transform.SetParent(parent);
            var battery = b.AddComponent<Battery>();
            battery.Label = label;
        }
    }
}
