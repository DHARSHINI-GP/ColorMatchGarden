using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGarden.UI
{
    public class AccessibilityPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
        
        [Header("Speed Sliders")]
        [SerializeField] private Slider transitionSpeedSlider;
        [SerializeField] private Slider animationSpeedSlider;
        
        [Header("Visual Sliders")]
        [SerializeField] private Slider brightnessBoostSlider;
        [SerializeField] private Slider particleDensitySlider;
        
        [Header("Input Sliders")]
        [SerializeField] private Slider gestureHoldTimeSlider;
        [SerializeField] private Slider sensorSensitivitySlider;
        [SerializeField] private Slider colorToleranceSlider;
        
        [Header("Audio Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider ambientVolumeSlider;
        [SerializeField] private Slider feedbackVolumeSlider;
        
        [Header("Toggles")]
        [SerializeField] private Toggle highContrastToggle;
        [SerializeField] private Toggle reducedMotionToggle;
        
        [Header("Buttons")]
        [SerializeField] private Button resetButton;
        [SerializeField] private Button closeButton;

        private Core.AccessibilityManager accessibilityManager;

        private void Start()
        {
            accessibilityManager = Core.AccessibilityManager.Instance;
            panelRoot?.SetActive(false);
            
            SetupListeners();
            LoadCurrentValues();
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                TogglePanel();
            }
        }

        private void SetupListeners()
        {
            transitionSpeedSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetTransitionSpeed(v));
            animationSpeedSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetAnimationSpeed(v));
            brightnessBoostSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetBrightnessBoost(v));
            particleDensitySlider?.onValueChanged.AddListener(v => accessibilityManager?.SetParticleDensity(v));
            gestureHoldTimeSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetGestureHoldTime(v));
            sensorSensitivitySlider?.onValueChanged.AddListener(v => accessibilityManager?.SetSensorSensitivity(v));
            colorToleranceSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetColorTolerance(v));
            masterVolumeSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetMasterVolume(v));
            ambientVolumeSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetAmbientVolume(v));
            feedbackVolumeSlider?.onValueChanged.AddListener(v => accessibilityManager?.SetFeedbackVolume(v));
            highContrastToggle?.onValueChanged.AddListener(v => accessibilityManager?.SetHighContrastMode(v));
            reducedMotionToggle?.onValueChanged.AddListener(v => accessibilityManager?.SetReducedMotion(v));
            
            resetButton?.onClick.AddListener(ResetToDefaults);
            closeButton?.onClick.AddListener(() => panelRoot?.SetActive(false));
        }

        private void LoadCurrentValues()
        {
            if (accessibilityManager == null) return;
            var settings = accessibilityManager.GetCurrentSettings();
            
            if (transitionSpeedSlider) transitionSpeedSlider.value = settings.TransitionSpeed;
            if (animationSpeedSlider) animationSpeedSlider.value = settings.AnimationSpeed;
            if (brightnessBoostSlider) brightnessBoostSlider.value = settings.BrightnessBoost;
            if (particleDensitySlider) particleDensitySlider.value = settings.ParticleDensity;
            if (gestureHoldTimeSlider) gestureHoldTimeSlider.value = settings.GestureHoldTime;
            if (sensorSensitivitySlider) sensorSensitivitySlider.value = settings.SensorSensitivity;
            if (colorToleranceSlider) colorToleranceSlider.value = settings.ColorTolerance;
            if (masterVolumeSlider) masterVolumeSlider.value = settings.MasterVolume;
            if (ambientVolumeSlider) ambientVolumeSlider.value = settings.AmbientVolume;
            if (feedbackVolumeSlider) feedbackVolumeSlider.value = settings.FeedbackVolume;
            if (highContrastToggle) highContrastToggle.isOn = settings.HighContrastMode;
            if (reducedMotionToggle) reducedMotionToggle.isOn = settings.ReducedMotion;
        }

        private void TogglePanel()
        {
            if (panelRoot != null)
                panelRoot.SetActive(!panelRoot.activeSelf);
        }

        private void ResetToDefaults()
        {
            accessibilityManager?.ResetToDefaults();
            LoadCurrentValues();
        }
    }
}
