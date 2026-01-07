using UnityEngine;
using System.Collections;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Controls the flower's color with smooth, calming transitions.
    /// Supports 5-sensor mixing:
    /// - RGB (Thumb, Index, Middle)
    /// - Brightness (Ring)
    /// - Magic/Sparkle (Pinky)
    /// </summary>
    public class ColorController : MonoBehaviour
    {
        [Header("Flower Reference")]
        [SerializeField] private Renderer flowerRenderer;
        [SerializeField] private Material flowerMaterial;
        
        [Header("Color Settings")]
        [SerializeField] private Color neutralColor = new Color(0.9f, 0.9f, 0.9f);
        [SerializeField] private float minBrightness = 0.3f;
        [SerializeField] private float maxBrightness = 1.0f;
        
        [Header("Transition Settings")]
        [SerializeField] private float colorTransitionSpeed = 2f;
        [SerializeField] private float brightnessLerpSpeed = 3f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Visual Feedback")]
        [SerializeField] private ParticleSystem colorChangeParticles;
        [SerializeField] private ParticleSystem magicSparkles; // For Pinky finger!
        [SerializeField] private Light flowerGlow;
        [SerializeField] private float glowIntensityMultiplier = 0.5f;

        private Color currentBaseColor;
        private Color targetColor;
        private float currentBrightness = 0.5f;
        private float targetBrightness = 0.5f;
        private float currentMagic = 0f;
        private bool isTransitioning = false;
        
        // Input reference
        private FiveSensorInput fiveSensorInput;

        private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");
        private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");

        private void Start()
        {
            if (flowerRenderer != null && flowerMaterial == null)
            {
                flowerMaterial = flowerRenderer.material;
            }
            
            // Try to find input system
            fiveSensorInput = FindObjectOfType<FiveSensorInput>();
            
            // Auto-find sparkles if not assigned
            if (magicSparkles == null)
            {
                var sparklesObj = transform.Find("MagicSparkles");
                if (sparklesObj != null) magicSparkles = sparklesObj.GetComponent<ParticleSystem>();
            }
            
            currentBaseColor = neutralColor;
            ApplyColor();
            
            if (magicSparkles != null) magicSparkles.Stop();
        }

        private void Update()
        {
            // If we have 5-sensor input, use it directly
            if (fiveSensorInput != null)
            {
                UpdateFromFiveSensors();
            }
            
            // Smooth brightness transitions
            if (Mathf.Abs(currentBrightness - targetBrightness) > 0.001f)
            {
                currentBrightness = Mathf.Lerp(currentBrightness, targetBrightness, 
                    Time.deltaTime * brightnessLerpSpeed);
                ApplyColor();
            }
        }
        
        /// <summary>
        /// Updates color based on 5-finger input
        /// </summary>
        private void UpdateFromFiveSensors()
        {
            // 1. Get RGB from Thumb, Index, Middle
            float r = fiveSensorInput.ThumbValue;
            float g = fiveSensorInput.IndexValue;
            float b = fiveSensorInput.MiddleValue;
            
            // Mix base color
            if (r + g + b > 0.1f)
            {
                // Normalize so it's not too dark
                float max = Mathf.Max(r, Mathf.Max(g, b));
                Color inputColor = new Color(r, g, b); // Standard mixing
                currentBaseColor = Color.Lerp(currentBaseColor, inputColor, Time.deltaTime * 5f);
            }
            else
            {
                // Go back to neutral if no fingers bent
                currentBaseColor = Color.Lerp(currentBaseColor, neutralColor, Time.deltaTime * 2f);
            }
            
            // 2. Ring finger controls BRIGHTNESS
            // Base brightness + Ring finger boost
            float ringBoost = fiveSensorInput.RingValue * 0.5f; // Add up to 50% more brightness
            targetBrightness = Mathf.Clamp01(0.5f + ringBoost);
            
            // 3. Pinky finger controls MAGIC sparkles
            float pinkyMagic = fiveSensorInput.PinkyValue;
            if (pinkyMagic > 0.3f && magicSparkles != null && !magicSparkles.isPlaying)
            {
                magicSparkles.Play();
            }
            else if (pinkyMagic < 0.2f && magicSparkles != null && magicSparkles.isPlaying)
            {
                magicSparkles.Stop();
            }
            
            // Update magic intensity
            currentMagic = pinkyMagic;
            
            ApplyColor();
        }

        /// <summary>
        /// Sets the target color that the child should match.
        /// </summary>
        public void SetTargetColor(Color color)
        {
            targetColor = color;
            
            if (colorChangeParticles != null)
            {
                var main = colorChangeParticles.main;
                main.startColor = color;
                colorChangeParticles.Play();
            }
        }

        private void ApplyColor()
        {
            if (flowerMaterial == null) return;

            // Base color * brightness
            Color finalColor = currentBaseColor * currentBrightness;
            
            // Add MAGIC shimmering if Pinky is active
            if (currentMagic > 0.1f)
            {
                float shimmer = Mathf.Sin(Time.time * 10f) * 0.2f * currentMagic;
                finalColor += new Color(shimmer, shimmer, shimmer);
            }
            
            // Apply to material
            flowerMaterial.SetColor(BaseColorProperty, finalColor);
            
            // Emission handles the glow
            Color emissionColor = finalColor * (0.3f + currentMagic * 0.5f); // Magic makes it glow more!
            flowerMaterial.SetColor(EmissionColorProperty, emissionColor);
            
            // Update light
            if (flowerGlow != null)
            {
                flowerGlow.color = finalColor;
                flowerGlow.intensity = (1f + currentMagic) * glowIntensityMultiplier * 2f;
            }
        }

        /// <summary>
        /// Update using normalized value (Legacy/Fallback)
        /// </summary>
        public void UpdateBrightness(float normalizedValue, BrightnessLevel level)
        {
            targetBrightness = Mathf.Lerp(minBrightness, maxBrightness, normalizedValue);
            ApplyColor();
        }

        public void PlayCelebrationPulse()
        {
            StartCoroutine(CelebrationPulseRoutine());
        }

        private IEnumerator CelebrationPulseRoutine()
        {
            float duration = 2f;
            float elapsed = 0f;
            Color celebrationColor = currentBaseColor;
            
            if (magicSparkles != null) magicSparkles.Play();

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float pulse = Mathf.Sin(elapsed * Mathf.PI * 4) * 0.2f + 1f;
                
                Color pulsedColor = celebrationColor * pulse;
                if (flowerMaterial != null)
                {
                    flowerMaterial.SetColor(BaseColorProperty, pulsedColor);
                    flowerMaterial.SetColor(EmissionColorProperty, pulsedColor * 0.6f);
                }
                
                yield return null;
            }
            
            if (magicSparkles != null && fiveSensorInput.PinkyValue < 0.2f) magicSparkles.Stop();
            
            ApplyColor();
        }

        public Color GetCurrentColor() => currentBaseColor;

        // Legacy/Compatibility methods
        public void SetColor(Color color) => SetTargetColor(color);
        public void SetBrightness(float brightness)
        {
            targetBrightness = Mathf.Clamp01(brightness);
            ApplyColor();
        }

        public void ResetToNeutral()
        {
            currentBaseColor = neutralColor;
            currentBrightness = 0.5f;
            targetBrightness = 0.5f;
            currentMagic = 0f;
            if (magicSparkles != null) magicSparkles.Stop();
            ApplyColor();
        }
    }

}
