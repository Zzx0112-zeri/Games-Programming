using UnityEngine;

namespace PowerCellEscape.Enemy
{
    using PowerCellEscape.Core;
    using PowerCellEscape.UI;

    /// <summary>
    /// A red enemy that wanders randomly around the room. It picks a new random
    /// target point every couple of seconds (or as soon as it reaches the current
    /// one) and moves there. It does NOT chase the player. Touching it costs a
    /// life. Carries a "!" marker so the threat is not colour-only.
    /// </summary>
    public class PatrollingEnemy : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float retargetMin = 1.2f;            // min seconds before a new target
        public float retargetMax = 2.8f;            // max seconds before a new target
        public Vector2 bounds = new Vector2(8f, 4.2f); // interior half-extents to roam within

        private Rigidbody2D rb;
        private WorldLabel marker;
        private Vector2 target;
        private float retargetTimer;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            marker = gameObject.AddComponent<WorldLabel>();
            marker.text = "!";
            marker.yOffset = 0.7f;
            marker.size = 30;
            marker.color = Color.white;
            PickNewTarget();
        }

        void PickNewTarget()
        {
            target = new Vector2(
                Random.Range(-bounds.x, bounds.x),
                Random.Range(-bounds.y, bounds.y));
            retargetTimer = Random.Range(retargetMin, retargetMax);
        }

        void FixedUpdate()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            retargetTimer -= Time.fixedDeltaTime;
            if (retargetTimer <= 0f || Vector2.Distance(transform.position, target) < 0.25f)
            {
                PickNewTarget();
            }

            Vector2 dir = (Vector2)target - (Vector2)transform.position;
            if (dir.magnitude > 0.001f)
                rb.velocity = dir.normalized * moveSpeed;
            else
                rb.velocity = Vector2.zero;

            // Keep the enemy safely inside the room.
            Vector3 p = transform.position;
            p.x = Mathf.Clamp(p.x, -bounds.x, bounds.x);
            p.y = Mathf.Clamp(p.y, -bounds.y, bounds.y);
            transform.position = p;
        }
    }
}
