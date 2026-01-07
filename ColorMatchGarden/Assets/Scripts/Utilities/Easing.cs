using UnityEngine;

namespace ColorMatchGarden.Utilities
{
    /// <summary>
    /// Utility class for smooth value transitions.
    /// Used throughout the game for calming animations.
    /// </summary>
    public static class Easing
    {
        public static float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }

        public static float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }

        public static float EaseInOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
        }

        public static float EaseOutElastic(float t)
        {
            if (t == 0f) return 0f;
            if (t == 1f) return 1f;
            
            float c4 = (2f * Mathf.PI) / 3f;
            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
        }

        public static float EaseOutBounce(float t)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (t < 1f / d1)
                return n1 * t * t;
            else if (t < 2f / d1)
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            else if (t < 2.5f / d1)
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            else
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }

        public static Color LerpColor(Color a, Color b, float t)
        {
            return Color.Lerp(a, b, EaseInOutSine(t));
        }
    }

    /// <summary>
    /// Extension methods for common operations.
    /// </summary>
    public static class Extensions
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color WithBrightness(this Color color, float brightness)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return Color.HSVToRGB(h, s, brightness);
        }

        public static void SetEmission(this Material material, Color color)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color);
        }
    }
}
