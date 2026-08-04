using UnityEngine;

namespace PowerCellEscape.Audio
{
    /// <summary>
    /// All sound effects are synthesised at runtime with short sine tones, so the
    /// project ships with no audio files. Volume / mute are applied globally via
    /// AudioListener (see SettingsManager).
    /// </summary>
    public class AudioFeedback : MonoBehaviour
    {
        public static AudioFeedback Instance { get; private set; }

        private AudioSource source;
        private AudioClip collectClip;
        private AudioClip hitClip;
        private AudioClip winClip;
        private AudioClip loseClip;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;

            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;

            collectClip = MakeTone(660f, 0.12f, 0.30f);
            hitClip = MakeTone(140f, 0.25f, 0.35f);
            winClip = MakeTone(880f, 0.45f, 0.30f);
            loseClip = MakeTone(180f, 0.60f, 0.30f);
        }

        private AudioClip MakeTone(float freq, float duration, float gain)
        {
            int sampleRate = 44100;
            int samples = (int)(sampleRate * duration);
            AudioClip clip = AudioClip.Create("tone", samples, 1, sampleRate, false);
            float[] data = new float[samples];
            for (int i = 0; i < samples; i++)
            {
                float t = i / (float)sampleRate;
                // Simple linear decay envelope to avoid clicks.
                data[i] = gain * Mathf.Sin(2f * Mathf.PI * freq * t) * (1f - t / duration);
            }
            clip.SetData(data, 0);
            return clip;
        }

        public void PlayCollect() { if (source != null && collectClip != null) source.PlayOneShot(collectClip); }
        public void PlayHit() { if (source != null && hitClip != null) source.PlayOneShot(hitClip); }
        public void PlayWin() { if (source != null && winClip != null) source.PlayOneShot(winClip); }
        public void PlayLose() { if (source != null && loseClip != null) source.PlayOneShot(loseClip); }
    }
}
