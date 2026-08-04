using UnityEngine;

namespace PowerCellEscape.Items
{
    using PowerCellEscape.Core;
    using PowerCellEscape.UI;

    /// <summary>
    /// A collectable power cell. On pickup it hides itself, notifies the
    /// GameManager and plays a sound. Carries a text label (B1/B2/B3) so status
    /// is never colour-only (accessibility requirement).
    /// </summary>
    public class Battery : MonoBehaviour
    {
        public string Label = "B";
        public bool Collected { get; private set; } = false;

        private SpriteRenderer sr;
        private WorldLabel label;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            label = gameObject.AddComponent<WorldLabel>();
            label.text = Label;
            label.yOffset = 0.55f;
            label.size = 22;
            label.color = Color.black;
        }

        public void Collect()
        {
            if (Collected) return;
            Collected = true;

            if (sr != null) sr.enabled = false;
            if (label != null) label.enabled = false;

            if (GameManager.Instance != null) GameManager.Instance.CollectCell();
            Audio.AudioFeedback.Instance?.PlayCollect();
        }
    }
}
