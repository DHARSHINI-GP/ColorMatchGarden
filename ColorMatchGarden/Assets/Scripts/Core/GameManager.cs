using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using ColorMatchGarden.UI;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Main game manager for Color Match Garden.
    /// Orchestrates game flow without any failure states or pressure.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.Idle;
        
        [Header("Color Settings")]
        [SerializeField] private Color[] gardenColors = new Color[]
        {
            new Color(1f, 0.6f, 0.8f),      // Soft Pink
            new Color(0.6f, 0.8f, 1f),      // Gentle Blue
            new Color(1f, 0.95f, 0.6f),     // Warm Yellow
            new Color(0.6f, 1f, 0.7f),      // Calming Green
            new Color(0.9f, 0.7f, 1f),      // Lavender
            new Color(1f, 0.8f, 0.6f)       // Peach
        };
        
        [Header("Timing (Calming Pace)")]
        [SerializeField] private float introDelay = 2f;
        [SerializeField] private float celebrationDuration = 3f;
        [SerializeField] private float transitionDelay = 1.5f;
        
        [Header("References")]
        [SerializeField] private ColorController colorController;
        [SerializeField] private GuideCharacter guideCharacter;
        [SerializeField] private ParticleController particleController;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private FiveSensorInput fiveSensorInput;
        [SerializeField] private GestureRecognizer gestureRecognizer;
        
        [Header("UI Guide System")]
        [SerializeField] private GameGuideUI gameGuideUI;

        [Header("Difficulty")]
        [SerializeField] private float matchTolerance = 0.3f; // How close the color needs to be (0-1)

        [Header("Events")]
        public UnityEvent OnGameStart;
        public UnityEvent OnColorPresented;
        public UnityEvent OnColorMatched;
        public UnityEvent OnCelebration;

        private Color currentTargetColor;
        private int colorIndex = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Auto-find references if missing
            if (colorController == null) colorController = FindObjectOfType<ColorController>();
            if (fiveSensorInput == null) fiveSensorInput = FindObjectOfType<FiveSensorInput>();
            if (guideCharacter == null) guideCharacter = FindObjectOfType<GuideCharacter>();
            
            // Subscribe to inputs
            if (fiveSensorInput != null)
            {
                // fiveSensorInput.OnConfirmGesture += OnConfirmGesture; // DISABLED: UI handles logic now!
                // Color changes are handled automatically by ColorController
            }

            StartCoroutine(BeginGardenExperience());
        }



        private IEnumerator BeginGardenExperience()
        {
            currentState = GameState.Intro;
            
            // Play ambient garden sounds
            soundManager?.PlayAmbient();
            
            // Wait for child to settle
            yield return new WaitForSeconds(introDelay);
            
            // Guide waves hello
            guideCharacter?.PlayWaveAnimation();
            soundManager?.PlaySoftChime();
            
            yield return new WaitForSeconds(1.5f);
            
            OnGameStart?.Invoke();
            
            // Start the color matching experience
            StartCoroutine(PresentNewColor());
        }

        private IEnumerator PresentNewColor()
        {
            currentState = GameState.Presenting;
            
            // Get next color (cycle through peacefully)
            currentTargetColor = gardenColors[colorIndex % gardenColors.Length];
            colorIndex++;
            
            // Guide presents the color
            guideCharacter?.PlayPresentAnimation(currentTargetColor);
            colorController?.SetTargetColor(currentTargetColor);
            
            // Update the UI Guide with target color
            int displayColorIndex = (colorIndex - 1) % gardenColors.Length;
            gameGuideUI?.SetTargetColor(currentTargetColor, displayColorIndex);
            
            soundManager?.PlaySoftNote();
            
            yield return new WaitForSeconds(1f);
            
            OnColorPresented?.Invoke();
            
            currentState = GameState.Matching;
        }

        /// <summary>
        /// Called when child confirms with open hand gesture or converts spacebar.
        /// Now verifies the match!
        /// </summary>
        public void OnConfirmGesture()
        {
            if (currentState != GameState.Matching) return;
            
            // 1. Get current flower color
            Color currentColor = colorController.GetCurrentColor();
            
            // 2. Compare with target (using simple RGB distance)
            float distance = ColorDistance(currentColor, currentTargetColor);
            
            Debug.Log($"Color Check: Current {currentColor} vs Target {currentTargetColor} (Dist: {distance:F2})");
            
            if (distance <= matchTolerance)
            {
                StartCoroutine(CelebrateSuccess());
            }
            else
            {
                // Gentle hint that it's not quite right yet
                guideCharacter?.PlayGentleNod(); 
                soundManager?.PlaySoftWhoosh(); // "Try again" sound
                
                // Show feedback in UI
                gameGuideUI?.ShowMessage("Not quite yet... keep trying! 🌸");
                
                // Notify visual guide to show failure feedback (shake)
                var visualOnly = FindObjectOfType<VisualOnlyGuideUI>();
                if (visualOnly != null)
                {
                    // Force it to show its "failure" feedback regardless of its internal tolerance
                    visualOnly.ShowTryAgainFeedback();
                }
            }
        }
        
        private float ColorDistance(Color c1, Color c2)
        {
            // Simple Euclidean distance in RGB space
            float r = c1.r - c2.r;
            float g = c1.g - c2.g;
            float b = c1.b - c2.b;
            return Mathf.Sqrt(r*r + g*g + b*b);
        }

        /// <summary>
        /// Called when child wants to reset with closed fist.
        /// Gentle reset, no negative feedback.
        /// </summary>
        public void OnResetGesture()
        {
            if (currentState != GameState.Matching) return;
            
            colorController?.ResetToNeutral();
            guideCharacter?.PlayGentleNod();
            soundManager?.PlaySoftWhoosh();
        }

        private IEnumerator CelebrateSuccess()
        {
            currentState = GameState.Celebrating;
            
            // Always celebrate - every attempt is wonderful!
            guideCharacter?.PlayCelebrateAnimation();
            particleController?.PlayCelebrationParticles(currentTargetColor);
            soundManager?.PlayHappyChime();
            
            // Show celebration in UI!
            gameGuideUI?.ShowCelebration("🎉 WONDERFUL! 🎉\nYou matched the color!");
            gameGuideUI?.OnConfirmGesture();
            
            OnCelebration?.Invoke();
            
            yield return new WaitForSeconds(celebrationDuration);
            
            // Gentle transition to next color
            particleController?.FadeOutParticles();
            
            yield return new WaitForSeconds(transitionDelay);
            
            // Present next color
            StartCoroutine(PresentNewColor());
        }

        /// <summary>
        /// Update flower brightness based on flex sensor input.
        /// Smooth, calming transitions.
        /// </summary>
        public void UpdateBrightness(float normalizedValue)
        {
            if (currentState != GameState.Matching) return;
            
            // ColorController handles its own brightness from sensors now.
            // We just update the UI here.
            
            // Update the guide UI with current color/progress
            gameGuideUI?.OnFlexSensorInput(normalizedValue);
            
            // Update the current color display based on brightness
            Color adjustedColor = currentTargetColor * (0.5f + normalizedValue * 0.5f);
            gameGuideUI?.UpdateCurrentColor(adjustedColor);
        }



        public GameState GetCurrentState() => currentState;
        public Color GetCurrentTargetColor() => currentTargetColor;
    }

    public enum GameState
    {
        Idle,
        Intro,
        Presenting,
        Matching,
        Celebrating
    }

    public enum BrightnessLevel
    {
        Light,
        Medium,
        Bright
    }
}
