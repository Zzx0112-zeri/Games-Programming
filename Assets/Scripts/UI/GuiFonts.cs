using UnityEngine;

namespace PowerCellEscape.UI
{
    /// <summary>
    /// Resolves the built-in GUI font without throwing. Unity 2022.3 renamed the
    /// legacy Arial built-in resource to "LegacyRuntime.ttf"; older Unity versions
    /// used "Arial.ttf". We try both and fall back to the skin default font so the
    /// UI never crashes on a missing built-in font name.
    /// </summary>
    public static class GuiFonts
    {
        private static Font _font;
        private static bool _resolved;

        public static Font Builtin
        {
            get
            {
                if (_resolved) return _font;
                _resolved = true;

                string[] names = { "LegacyRuntime.ttf", "Arial.ttf" };
                foreach (var n in names)
                {
                    try
                    {
                        _font = Resources.GetBuiltinResource<Font>(n);
                    }
                    catch (System.Exception)
                    {
                        _font = null;
                    }
                    if (_font != null) break;
                }
                return _font;
            }
        }
    }
}
