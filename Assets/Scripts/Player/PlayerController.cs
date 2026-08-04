using UnityEngine;

namespace PowerCellEscape.Player
{
    using PowerCellEscape.Core;
    using PowerCellEscape.Items;

    /// <summary>
    /// Top-down player movement and collection. Uses a dynamic Rigidbody2D with
    /// velocity-based movement; the collider is NOT a trigger so it physically
    /// stops at the room walls. Battery pickup is detected by proximity every
    /// frame, and any contact with the enemy ends the game immediately (Game Over).
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float pickupRadius = 0.7f;   // how close to a battery counts as "touch"
        public float hitRadius = 0.75f;     // how close to the enemy counts as "hit"

        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private const float BoundX = 8.5f;
        private const float BoundY = 4.5f;

        private Battery[] batteries;
        private Transform enemyTransform;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            batteries = FindObjectsOfType<Battery>();
            var enemy = GameObject.FindWithTag("Enemy");
            enemyTransform = enemy != null ? enemy.transform : null;
        }

        void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;

            // Collect any battery the player is standing on.
            if (batteries != null)
            {
                for (int i = 0; i < batteries.Length; i++)
                {
                    Battery b = batteries[i];
                    if (b != null && !b.Collected &&
                        Vector2.Distance(transform.position, b.transform.position) < pickupRadius)
                    {
                        b.Collect();
                    }
                }
            }

            // Any contact with the enemy ends the round (Game Over).
            if (enemyTransform != null &&
                Vector2.Distance(transform.position, enemyTransform.position) < hitRadius)
            {
                GameManager.Instance.Lose();
                Audio.AudioFeedback.Instance?.PlayLose();
            }
        }

        void FixedUpdate()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 move = new Vector2(h, v).normalized * speed;
            rb.velocity = move;

            Vector3 p = transform.position;
            p.x = Mathf.Clamp(p.x, -BoundX, BoundX);
            p.y = Mathf.Clamp(p.y, -BoundY, BoundY);
            transform.position = p;
        }
    }
}
