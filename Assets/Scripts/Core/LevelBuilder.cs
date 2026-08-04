using UnityEngine;

namespace PowerCellEscape.Core
{
    using PowerCellEscape.Enemy;
    using PowerCellEscape.Items;
    using PowerCellEscape.Player;
    using PowerCellEscape.Utils;

    /// <summary>
    /// Builds the entire level in code: camera, room walls, player, the three
    /// power cells, the exit door and the patrolling enemy. Called by
    /// GameBootstrap once the scene has loaded.
    /// </summary>
    public class LevelBuilder
    {
        public static void Build()
        {
            // ---- Camera --------------------------------------------------
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
            cam.backgroundColor = new Color(0.10f, 0.10f, 0.15f, 1f);

            // ---- Room dimensions ----------------------------------------
            const float halfX = 9f;
            const float halfY = 5f;
            const float wall = 0.5f;

            CreateWall("WallTop", new Vector3(0f, halfY + wall / 2f, 0f), new Vector3((halfX + wall) * 2f, wall, 1f));
            CreateWall("WallBottom", new Vector3(0f, -halfY - wall / 2f, 0f), new Vector3((halfX + wall) * 2f, wall, 1f));
            CreateWall("WallLeft", new Vector3(-halfX - wall / 2f, 0f, 0f), new Vector3(wall, (halfY + wall) * 2f, 1f));
            CreateWall("WallRight", new Vector3(halfX + wall / 2f, 0f, 0f), new Vector3(wall, (halfY + wall) * 2f, 1f));

            // ---- Player -------------------------------------------------
            var player = new GameObject("Player");
            player.tag = "Player";
            var pSr = player.AddComponent<SpriteRenderer>();
            pSr.sprite = GameArt.MakeRobotSprite();
            pSr.sortingOrder = 2;
            var pRb = player.AddComponent<Rigidbody2D>();
            pRb.gravityScale = 0f;
            pRb.freezeRotation = true;
            pRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            var pCol = player.AddComponent<BoxCollider2D>();
            pCol.size = new Vector2(0.9f, 0.9f);
            player.transform.position = new Vector3(0f, -3.5f, 0f);
            player.AddComponent<PlayerController>();

            // ---- Power cells --------------------------------------------
            SpawnBattery("B1", new Vector3(-6f, 0f, 0f));
            SpawnBattery("B2", new Vector3(6f, 0f, 0f));
            SpawnBattery("B3", new Vector3(0f, 3f, 0f));

            // ---- Exit door (top centre) --------------------------------
            var exit = new GameObject("Exit");
            exit.tag = "Exit";
            var eSr = exit.AddComponent<SpriteRenderer>();
            eSr.sprite = GameArt.MakeSquareSprite(Color.white, 64);
            eSr.sortingOrder = 1;
            exit.transform.position = new Vector3(0f, halfY - 0.3f, 0f);
            exit.transform.localScale = new Vector3(2f, 1f, 1f);
            var eCol = exit.AddComponent<BoxCollider2D>();
            eCol.isTrigger = true;
            eCol.size = new Vector2(2f, 1f);
            exit.AddComponent<ExitDoor>();

            // ---- Enemy --------------------------------------------------
            var enemy = new GameObject("Enemy");
            enemy.tag = "Enemy";
            var enSr = enemy.AddComponent<SpriteRenderer>();
            enSr.sprite = GameArt.MakeSquareSprite(new Color(0.90f, 0.10f, 0.10f, 1f), 64);
            enSr.sortingOrder = 2;
            var enRb = enemy.AddComponent<Rigidbody2D>();
            enRb.gravityScale = 0f;
            enRb.freezeRotation = true;
            enRb.bodyType = RigidbodyType2D.Kinematic;
            var enCol = enemy.AddComponent<BoxCollider2D>();
            enCol.isTrigger = true;
            enCol.size = new Vector2(0.9f, 0.9f);
            enemy.transform.position = new Vector3(-5f, -2f, 0f);
            enemy.AddComponent<PatrollingEnemy>();
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            var wall = new GameObject(name);
            var sr = wall.AddComponent<SpriteRenderer>();
            sr.sprite = GameArt.MakeWallSprite(new Color(0.30f, 0.30f, 0.42f, 1f), 8, 8);
            sr.sortingOrder = 0;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.AddComponent<BoxCollider2D>();
        }

        private static void SpawnBattery(string label, Vector3 position)
        {
            var b = new GameObject("Battery_" + label);
            b.tag = "Battery";
            var sr = b.AddComponent<SpriteRenderer>();
            sr.sprite = GameArt.MakeCircleSprite(Color.yellow, 64);
            sr.sortingOrder = 2;
            b.transform.position = position;
            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.4f;
            var battery = b.AddComponent<Battery>();
            battery.Label = label;
        }
    }
}
