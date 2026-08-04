using UnityEngine;

namespace PowerCellEscape.Items
{
    using PowerCellEscape.Core;
    using PowerCellEscape.UI;

    /// <summary>
    /// The exit. It is LOCKED (red, label "LOCKED") until all three power cells
    /// are collected, then it turns green and shows "OPEN". Reaching it while open
    /// wins the round. The win check runs every frame, so collecting the last cell
    /// while already standing on the door still wins.
    /// </summary>
    public class ExitDoor : MonoBehaviour
    {
        private SpriteRenderer sr;
        private WorldLabel status;

        private readonly Color lockedColor = new Color(0.80f, 0.10f, 0.10f, 1f);
        private readonly Color openColor = new Color(0.10f, 0.80f, 0.20f, 1f);

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            status = gameObject.AddComponent<WorldLabel>();
            status.yOffset = 0f;
            status.size = 20;
            status.color = Color.white;
            status.text = "LOCKED";
        }

        void Update()
        {
            if (GameManager.Instance == null) return;

            bool open = GameManager.Instance.AllCellsCollected;
            if (sr != null) sr.color = open ? openColor : lockedColor;
            if (status != null) status.text = open ? "OPEN" : "LOCKED";

            // Win as soon as the player reaches the open exit.
            if (open && GameManager.Instance.State == GameState.Playing)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null &&
                    Vector2.Distance(transform.position, player.transform.position) < 1.0f)
                {
                    GameManager.Instance.Win();
                    Audio.AudioFeedback.Instance?.PlayWin();
                }
            }
        }
    }
}
