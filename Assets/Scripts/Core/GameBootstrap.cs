using UnityEngine;

namespace PowerCellEscape.Core
{
    /// <summary>
    /// Entry point. The scene file contains no hand-placed objects (only a camera
    /// if present); everything is created in code when the level loads. This keeps
    /// the repo free of fragile serialized references and matches the design note
    /// "the level is built in code at runtime by GameBootstrap + LevelBuilder".
    /// </summary>
    public class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            // Core managers (singletons guard against duplicates via Awake).
            new GameObject("GameManager").AddComponent<GameManager>();
            new GameObject("SettingsManager").AddComponent<Settings.SettingsManager>();
            new GameObject("AudioFeedback").AddComponent<Audio.AudioFeedback>();
            new GameObject("HUDManager").AddComponent<UI.HUDManager>();
            new GameObject("InstructionsPanel").AddComponent<UI.InstructionsPanel>();

            LevelBuilder.Build();
        }
    }
}
