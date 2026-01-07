using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ColorMatchGarden.UI
{
    /// <summary>
    /// User-friendly visual guide UI for Color Match Garden.
    /// Shows clear step-by-step instructions with visual icons.
    /// Designed for neurodiverse children - minimal text, maximum visuals!
    /// </summary>
    public class GameGuideUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private Canvas guideCanvas;
        
        [Header("Target Color Display")]
        [SerializeField] private Image targetColorCircle;
        [SerializeField] private Image currentColorCircle;
        [SerializeField] private Text colorNameText;
        
        [Header("Step Indicators")]
        [SerializeField] private Image[] stepIcons;
        [SerializeField] private Color activeStepColor = new Color(1f, 0.9f, 0.2f); // Yellow
        [SerializeField] private Color inactiveStepColor = new Color(0.5f, 0.5f, 0.5f); // Gray
        
        [Header("Guide Message")]
        [SerializeField] private Text guideMessageText;
        [SerializeField] private Image guideCharacterIcon;
        
        [Header("Celebration")]
        [SerializeField] private GameObject celebrationPanel;
        [SerializeField] private Text celebrationText;
        
        [Header("Settings")]
        [SerializeField] private float messageDisplayTime = 3f;
        [SerializeField] private bool autoCreateUI = true;
        
        // Color names for display
        private readonly string[] colorNames = new string[]
        {
            "Pink", "Blue", "Yellow", "Green", "Purple", "Orange"
        };
        
        private int currentStep = 0;
        private const int TOTAL_STEPS = 3;
        
        private void Start()
        {
            if (autoCreateUI)
            {
                CreateGuideUI();
            }
            StartCoroutine(ShowWelcomeSequence());
        }
        
        /// <summary>
        /// Creates all UI elements programmatically
        /// </summary>
        private void CreateGuideUI()
        {
            // Create Canvas if not assigned
            if (guideCanvas == null)
            {
                GameObject canvasObj = new GameObject("GuideCanvas");
                guideCanvas = canvasObj.AddComponent<Canvas>();
                guideCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                guideCanvas.sortingOrder = 100;
                canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            
            // Create Target Color Display (Top Left)
            CreateTargetColorPanel();
            
            // Create Step Indicators (Top Center)
            CreateStepIndicators();
            
            // Create Guide Message (Bottom)
            CreateGuideMessagePanel();
            
            // Create Celebration Panel (Center - Hidden)
            CreateCelebrationPanel();
        }
        
        private void CreateTargetColorPanel()
        {
            // Panel background
            GameObject panel = CreateUIPanel("TargetColorPanel", guideCanvas.transform);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 1);
            panelRect.anchorMax = new Vector2(0, 1);
            panelRect.pivot = new Vector2(0, 1);
            panelRect.anchoredPosition = new Vector2(20, -20);
            panelRect.sizeDelta = new Vector2(200, 120);
            
            Image panelBg = panel.GetComponent<Image>();
            panelBg.color = new Color(0, 0, 0, 0.6f);
            
            // "Match This Color" label
            GameObject labelObj = CreateUIText("Label", panel.transform, "🎨 MATCH THIS COLOR:", 16);
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchoredPosition = new Vector2(0, -15);
            labelRect.sizeDelta = new Vector2(180, 30);
            
            // Target color circle (large)
            GameObject targetObj = new GameObject("TargetColorCircle");
            targetObj.transform.SetParent(panel.transform, false);
            targetColorCircle = targetObj.AddComponent<Image>();
            targetColorCircle.color = Color.magenta;
            
            // Make it circular
            RectTransform targetRect = targetObj.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(-40, -65);
            targetRect.sizeDelta = new Vector2(60, 60);
            
            // "Your Color" label and circle
            GameObject yourLabel = CreateUIText("YourLabel", panel.transform, "YOUR\nCOLOR:", 12);
            RectTransform yourLabelRect = yourLabel.GetComponent<RectTransform>();
            yourLabelRect.anchoredPosition = new Vector2(40, -55);
            yourLabelRect.sizeDelta = new Vector2(50, 40);
            
            GameObject currentObj = new GameObject("CurrentColorCircle");
            currentObj.transform.SetParent(panel.transform, false);
            currentColorCircle = currentObj.AddComponent<Image>();
            currentColorCircle.color = Color.white;
            RectTransform currentRect = currentObj.GetComponent<RectTransform>();
            currentRect.anchoredPosition = new Vector2(80, -65);
            currentRect.sizeDelta = new Vector2(40, 40);
            
            // Color name text
            GameObject nameObj = CreateUIText("ColorName", panel.transform, "Pink", 14);
            colorNameText = nameObj.GetComponent<Text>();
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchoredPosition = new Vector2(0, -100);
            nameRect.sizeDelta = new Vector2(180, 25);
        }
        
        private void CreateStepIndicators()
        {
            // Panel for steps
            GameObject panel = CreateUIPanel("StepsPanel", guideCanvas.transform);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 1);
            panelRect.anchorMax = new Vector2(0.5f, 1);
            panelRect.pivot = new Vector2(0.5f, 1);
            panelRect.anchoredPosition = new Vector2(0, -20);
            panelRect.sizeDelta = new Vector2(400, 80);
            
            Image panelBg = panel.GetComponent<Image>();
            panelBg.color = new Color(0, 0, 0, 0.6f);
            
            // Title
            GameObject title = CreateUIText("Title", panel.transform, "🌸 HOW TO PLAY 🌸", 18);
            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, -15);
            titleRect.sizeDelta = new Vector2(380, 25);
            
            // Step labels
            string[] stepLabels = new string[]
            {
                "1️⃣ LOOK at color",
                "2️⃣ BEND sensor",
                "3️⃣ OPEN hand ✋"
            };
            
            stepIcons = new Image[3];
            
            for (int i = 0; i < 3; i++)
            {
                // Step container
                GameObject stepObj = new GameObject($"Step{i + 1}");
                stepObj.transform.SetParent(panel.transform, false);
                
                RectTransform stepRect = stepObj.AddComponent<RectTransform>();
                stepRect.anchoredPosition = new Vector2(-120 + (i * 120), -50);
                stepRect.sizeDelta = new Vector2(110, 35);
                
                stepIcons[i] = stepObj.AddComponent<Image>();
                stepIcons[i].color = i == 0 ? activeStepColor : inactiveStepColor;
                
                // Step text
                GameObject textObj = CreateUIText($"StepText{i}", stepObj.transform, stepLabels[i], 11);
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.anchoredPosition = Vector2.zero;
                textRect.sizeDelta = new Vector2(100, 30);
                Text txt = textObj.GetComponent<Text>();
                txt.color = Color.black;
            }
        }
        
        private void CreateGuideMessagePanel()
        {
            // Panel at bottom
            GameObject panel = CreateUIPanel("GuideMessagePanel", guideCanvas.transform);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.anchoredPosition = new Vector2(0, 20);
            panelRect.sizeDelta = new Vector2(500, 80);
            
            Image panelBg = panel.GetComponent<Image>();
            panelBg.color = new Color(0.3f, 0.2f, 0.5f, 0.85f); // Purple background
            
            // Guide character icon (left side)
            GameObject iconObj = new GameObject("GuideIcon");
            iconObj.transform.SetParent(panel.transform, false);
            guideCharacterIcon = iconObj.AddComponent<Image>();
            guideCharacterIcon.color = new Color(0.8f, 0.5f, 1f); // Light purple
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(-200, 0);
            iconRect.sizeDelta = new Vector2(60, 60);
            
            // Message text
            GameObject msgObj = CreateUIText("GuideMessage", panel.transform, "Welcome to the Magic Garden! 🌸", 20);
            guideMessageText = msgObj.GetComponent<Text>();
            guideMessageText.alignment = TextAnchor.MiddleCenter;
            RectTransform msgRect = msgObj.GetComponent<RectTransform>();
            msgRect.anchoredPosition = new Vector2(30, 0);
            msgRect.sizeDelta = new Vector2(380, 70);
        }
        
        private void CreateCelebrationPanel()
        {
            celebrationPanel = CreateUIPanel("CelebrationPanel", guideCanvas.transform);
            RectTransform panelRect = celebrationPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(400, 200);
            
            Image panelBg = celebrationPanel.GetComponent<Image>();
            panelBg.color = new Color(1f, 0.8f, 0.2f, 0.95f); // Golden
            
            // Celebration text
            GameObject textObj = CreateUIText("CelebrationText", celebrationPanel.transform, "🎉 AMAZING! 🎉\nYou matched the color!", 28);
            celebrationText = textObj.GetComponent<Text>();
            celebrationText.color = new Color(0.2f, 0.1f, 0.4f);
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(380, 180);
            
            celebrationPanel.SetActive(false);
        }
        
        private GameObject CreateUIPanel(string name, Transform parent)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            panel.AddComponent<RectTransform>();
            panel.AddComponent<Image>();
            return panel;
        }
        
        private GameObject CreateUIText(string name, Transform parent, string text, int fontSize)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
            textObj.AddComponent<RectTransform>();
            
            Text txt = textObj.AddComponent<Text>();
            txt.text = text;
            txt.fontSize = fontSize;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            
            return textObj;
        }
        
        // ============ PUBLIC METHODS ============
        
        private IEnumerator ShowWelcomeSequence()
        {
            yield return new WaitForSeconds(1f);
            ShowMessage("Welcome to the Magic Garden! 🌸");
            
            yield return new WaitForSeconds(2.5f);
            ShowMessage("Let's match some beautiful colors! 🎨");
            
            yield return new WaitForSeconds(2.5f);
            ShowMessage("Watch the guide - she'll show you the color!");
            
            yield return new WaitForSeconds(2.5f);
            SetStep(1);
        }
        
        /// <summary>
        /// Set the target color to match
        /// </summary>
        public void SetTargetColor(Color color, int colorIndex = 0)
        {
            if (targetColorCircle != null)
            {
                targetColorCircle.color = color;
            }
            
            if (colorNameText != null && colorIndex < colorNames.Length)
            {
                colorNameText.text = colorNames[colorIndex];
            }
            
            ShowMessage($"Match this color: {(colorIndex < colorNames.Length ? colorNames[colorIndex] : "Beautiful")}! 🌈");
            SetStep(1);
        }
        
        /// <summary>
        /// Update the current color being created by the player
        /// </summary>
        public void UpdateCurrentColor(Color color)
        {
            if (currentColorCircle != null)
            {
                currentColorCircle.color = color;
            }
        }
        
        /// <summary>
        /// Set the current active step (1, 2, or 3)
        /// </summary>
        public void SetStep(int step)
        {
            currentStep = Mathf.Clamp(step, 1, TOTAL_STEPS);
            
            if (stepIcons != null)
            {
                for (int i = 0; i < stepIcons.Length; i++)
                {
                    if (stepIcons[i] != null)
                    {
                        stepIcons[i].color = (i < currentStep) ? activeStepColor : inactiveStepColor;
                    }
                }
            }
            
            // Show step-specific message
            switch (currentStep)
            {
                case 1:
                    ShowMessage("Step 1: Look at the TARGET color above! 👀");
                    break;
                case 2:
                    ShowMessage("Step 2: Bend the sensor to change brightness! 💪");
                    break;
                case 3:
                    ShowMessage("Step 3: Open your hand when ready! ✋");
                    break;
            }
        }
        
        /// <summary>
        /// Show a guide message at the bottom
        /// </summary>
        public void ShowMessage(string message)
        {
            if (guideMessageText != null)
            {
                guideMessageText.text = message;
            }
        }
        
        /// <summary>
        /// Show celebration when color is matched!
        /// </summary>
        public void ShowCelebration(string message = null)
        {
            if (celebrationPanel != null)
            {
                celebrationPanel.SetActive(true);
                
                if (celebrationText != null && message != null)
                {
                    celebrationText.text = message;
                }
                
                StartCoroutine(HideCelebrationAfterDelay());
            }
            
            ShowMessage("🎉 WONDERFUL! You did it! 🎉");
        }
        
        private IEnumerator HideCelebrationAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            
            if (celebrationPanel != null)
            {
                celebrationPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Called when flex sensor value changes
        /// </summary>
        public void OnFlexSensorInput(float value)
        {
            if (currentStep == 1 && value > 0.1f)
            {
                SetStep(2);
            }
        }
        
        /// <summary>
        /// Called when player confirms with open hand
        /// </summary>
        public void OnConfirmGesture()
        {
            SetStep(3);
            ShowCelebration("🎉 PERFECT MATCH! 🎉\nYou're amazing!");
        }
    }
}
