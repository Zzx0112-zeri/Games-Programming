using UnityEngine;

namespace PowerCellEscape.Items
{
using PowerCellEscape.Core;
using PowerCellEscape.Player;
using PowerCellEscape.Utils;

    /// <summary>
    /// The exit. While the three power cells are NOT all collected, this spot shows
    /// the "no entry" (forbidden) sign and cannot be used. Once all three cells are
    /// collected the forbidden sign is replaced by the exit icon, and reaching it wins
    /// the round. The win check runs every frame, so collecting the last cell while
    /// already standing on the door still wins.
    /// </summary>
    public class ExitDoor : MonoBehaviour
    {
        private SpriteRenderer sr;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            // Start locked: show the forbidden sign at the exit location.
            if (sr != null) sr.sprite = SpriteAssets.Forbidden;
        }

        void Update()
        {
            if (GameManager.Instance == null) return;

            bool open = GameManager.Instance.AllCellsCollected;
            if (sr != null)
            {
                sr.sprite = open ? SpriteAssets.ExitDoor : SpriteAssets.Forbidden;
                sr.color = Color.white;
            }

            // Win as soon as the player reaches the open exit.
            if (open && GameManager.Instance.State == GameState.Playing)
            {
                var player = FindObjectOfType<PlayerController>();
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
