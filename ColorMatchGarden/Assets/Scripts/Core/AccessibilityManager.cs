using UnityEngine;
using UnityEngine.Events;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Manages accessibility settings to accommodate different needs.
    /// Makes the game comfortable for all children.
    /// </summary>
    public class AccessibilityManager : MonoBehaviour
    {
        public static AccessibilityManager Instance { get; private set; }

        [Header("Speed Settings")]
        [SerializeField, Range(0.5f, 3f)] 
        private float transitionSpeed = 1f;
        
        [SerializeField, Range(0.5f, 3f)] 
        private float animationSpeed = 1f;

        [Header("Visual Settings")]
        [SerializeField, Range(0f, 0.5f)] 
        private float brightnessBoost = 0f;
        
        [SerializeField, Range(0f, 1f)] 
        private float particleDensity = 0.75f;
        
        [SerializeField] 
        private bool highContrastMode = false;
        
        [SerializeField] 
        private bool reducedMotion = false;

        [Header("Input Settings")]
        [SerializeField, Range(0.1f, 0.5f)] 
        private float colorTolerance = 0.3f;
        
        [SerializeField, Range(0.5f, 3f)] 
        private float gestureHoldTime = 1.5f;
        
        [SerializeField, Range(0.1f, 1f)] 
        private float sensorSensitivity = 0.5f;

        [Header("Audio Settings")]
        [SerializeField, Range(0f, 1f)] 
        private float masterVolume = 0.7f;
        
        [SerializeField, Range(0f, 1f)] 
        private float ambientVolume = 0.5f;
        
        [SerializeField, Range(0f, 1f)] 
        private float feedbackVolume = 0.8f;

        [Header("Events")]
        public UnityEvent<AccessibilitySettings> OnSettingsChanged;

        private AccessibilitySettings currentSettings;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            LoadSettings();
            ApplySettings();
        }

        private void LoadSettings()
        {
            // Load from PlayerPrefs or use defaults
            transitionSpeed = PlayerPrefs.GetFloat("A11y_TransitionSpeed", transitionSpeed);
            animationSpeed = PlayerPrefs.GetFloat("A11y_AnimationSpeed", animationSpeed);
            brightnessBoost = PlayerPrefs.GetFloat("A11y_BrightnessBoost", brightnessBoost);
            particleDensity = PlayerPrefs.GetFloat("A11y_ParticleDensity", particleDensity);
            highContrastMode = PlayerPrefs.GetInt("A11y_HighContrast", 0) == 1;
            reducedMotion = PlayerPrefs.GetInt("A11y_ReducedMotion", 0) == 1;
            colorTolerance = PlayerPrefs.GetFloat("A11y_ColorTolerance", colorTolerance);
            gestureHoldTime = PlayerPrefs.GetFloat("A11y_GestureHoldTime", gestureHoldTime);
            sensorSensitivity = PlayerPrefs.GetFloat("A11y_SensorSensitivity", sensorSensitivity);
            masterVolume = PlayerPrefs.GetFloat("A11y_MasterVolume", masterVolume);
            ambientVolume = PlayerPrefs.GetFloat("A11y_AmbientVolume", ambientVolume);
            feedbackVolume = PlayerPrefs.GetFloat("A11y_FeedbackVolume", feedbackVolume);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("A11y_TransitionSpeed", transitionSpeed);
            PlayerPrefs.SetFloat("A11y_AnimationSpeed", animationSpeed);
            PlayerPrefs.SetFloat("A11y_BrightnessBoost", brightnessBoost);
            PlayerPrefs.SetFloat("A11y_ParticleDensity", particleDensity);
            PlayerPrefs.SetInt("A11y_HighContrast", highContrastMode ? 1 : 0);
            PlayerPrefs.SetInt("A11y_ReducedMotion", reducedMotion ? 1 : 0);
            PlayerPrefs.SetFloat("A11y_ColorTolerance", colorTolerance);
            PlayerPrefs.SetFloat("A11y_GestureHoldTime", gestureHoldTime);
            PlayerPrefs.SetFloat("A11y_SensorSensitivity", sensorSensitivity);
            PlayerPrefs.SetFloat("A11y_MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("A11y_AmbientVolume", ambientVolume);
            PlayerPrefs.SetFloat("A11y_FeedbackVolume", feedbackVolume);
            PlayerPrefs.Save();
        }

        private void ApplySettings()
        {
            currentSettings = new AccessibilitySettings
            {
                TransitionSpeed = transitionSpeed,
                AnimationSpeed = animationSpeed,
                BrightnessBoost = brightnessBoost,
                ParticleDensity = particleDensity,
                HighContrastMode = highContrastMode,
                ReducedMotion = reducedMotion,
                ColorTolerance = colorTolerance,
                GestureHoldTime = gestureHoldTime,
                SensorSensitivity = sensorSensitivity,
                MasterVolume = masterVolume,
                AmbientVolume = ambientVolume,
                FeedbackVolume = feedbackVolume
            };

            // Apply animation speed globally
            Time.timeScale = reducedMotion ? 0.8f : 1f;
            
            // Apply audio
            AudioListener.volume = masterVolume;

            OnSettingsChanged?.Invoke(currentSettings);
        }

        // Public setters with auto-apply
        public void SetTransitionSpeed(float value)
        {
            transitionSpeed = Mathf.Clamp(value, 0.5f, 3f);
            ApplySettings();
        }

        public void SetAnimationSpeed(float value)
        {
            animationSpeed = Mathf.Clamp(value, 0.5f, 3f);
            ApplySettings();
        }

        public void SetBrightnessBoost(float value)
        {
            brightnessBoost = Mathf.Clamp(value, 0f, 0.5f);
            ApplySettings();
        }

        public void SetParticleDensity(float value)
        {
            particleDensity = Mathf.Clamp01(value);
            ApplySettings();
        }

        public void SetHighContrastMode(bool enabled)
        {
            highContrastMode = enabled;
            ApplySettings();
        }

        public void SetReducedMotion(bool enabled)
        {
            reducedMotion = enabled;
            ApplySettings();
        }

        public void SetColorTolerance(float value)
        {
            colorTolerance = Mathf.Clamp(value, 0.1f, 0.5f);
            ApplySettings();
        }

        public void SetGestureHoldTime(float value)
        {
            gestureHoldTime = Mathf.Clamp(value, 0.5f, 3f);
            ApplySettings();
        }

        public void SetSensorSensitivity(float value)
        {
            sensorSensitivity = Mathf.Clamp(value, 0.1f, 1f);
            ApplySettings();
        }

        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp01(value);
            ApplySettings();
        }

        public void SetAmbientVolume(float value)
        {
            ambientVolume = Mathf.Clamp01(value);
            ApplySettings();
        }

        public void SetFeedbackVolume(float value)
        {
            feedbackVolume = Mathf.Clamp01(value);
            ApplySettings();
        }

        public void ResetToDefaults()
        {
            transitionSpeed = 1f;
            animationSpeed = 1f;
            brightnessBoost = 0f;
            particleDensity = 0.75f;
            highContrastMode = false;
            reducedMotion = false;
            colorTolerance = 0.3f;
            gestureHoldTime = 1.5f;
            sensorSensitivity = 0.5f;
            masterVolume = 0.7f;
            ambientVolume = 0.5f;
            feedbackVolume = 0.8f;
            
            ApplySettings();
            SaveSettings();
        }

        public AccessibilitySettings GetCurrentSettings() => currentSettings;
    }

    [System.Serializable]
    public struct AccessibilitySettings
    {
        public float TransitionSpeed;
        public float AnimationSpeed;
        public float BrightnessBoost;
        public float ParticleDensity;
        public bool HighContrastMode;
        public bool ReducedMotion;
        public float ColorTolerance;
        public float GestureHoldTime;
        public float SensorSensitivity;
        public float MasterVolume;
        public float AmbientVolume;
        public float FeedbackVolume;
    }
}
