using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ColorMatchGarden.UI
{
    /// <summary>
    /// TEXT-FREE Visual Guide for neurodiverse children.
    /// Uses ONLY colors, icons, and visual feedback - NO TEXT!
    /// 3 Sensors control RGB color mixing.
    /// </summary>
    public class VisualOnlyGuideUI : MonoBehaviour
    {
        [Header("Main Display")]
        private Canvas mainCanvas;
        
        [Header("Target Color (What to match)")]
        private Image targetColorBox;
        private Image targetColorGlow;
        
        [Header("Mixed Color (What player creates)")]
        private Image mixedColorBox;
        private Image mixedColorGlow;
        
        [Header("RGB Sensor Indicators")]
        private Image redSensorBar;
        private Image greenSensorBar;
        private Image blueSensorBar;
        private Image redSensorIcon;
        private Image greenSensorIcon;
        private Image blueSensorIcon;
        
        [Header("Match Indicator")]
        private Image matchArrow;
        private Image checkmarkIcon;
        
        [Header("Celebration")]
        private GameObject celebrationPanel;
        private Image[] celebrationStars;
        
        [Header("Settings")]
        [SerializeField] private float sensorBarMaxHeight = 150f;
        
        // Current color values from 3 sensors
        private float redValue = 0f;
        private float greenValue = 0f;
        private float blueValue = 0f;
        
        private Color targetColor = Color.magenta;
        private Color mixedColor = Color.black;
        
        private bool isMatched = false;
        
        private void Start()
        {
            CreateVisualOnlyUI();
            StartCoroutine(ShowWelcomeAnimation());
        }
        
        private void CreateVisualOnlyUI()
        {
            // Create Canvas
            GameObject canvasObj = new GameObject("VisualGuideCanvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 100;
            
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Create clean, styled UI elements
            CreateStyledTargetColorDisplay();
            CreateStyledMixedColorDisplay();
            CreateStyledMatchIndicator();
            CreateStyledSensorBars();
            CreateCelebrationEffects();
        }
        
        private void CreateStyledTargetColorDisplay()
        {
            // Container with dark rounded background
            GameObject container = CreateUIPanel("TargetPanel", mainCanvas.transform, 
                new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                new Vector2(40, 0), new Vector2(140, 180));
            
            // Semi-transparent dark background
            Image containerBg = container.AddComponent<Image>();
            containerBg.color = new Color(0.1f, 0.1f, 0.15f, 0.85f);
            
            // Title icon (eye symbol - "look here")
            GameObject iconObj = new GameObject("TargetIcon");
            iconObj.transform.SetParent(container.transform, false);
            Image icon = iconObj.AddComponent<Image>();
            icon.color = new Color(0.9f, 0.9f, 1f, 0.8f);
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(0, 65);
            iconRect.sizeDelta = new Vector2(30, 30);
            
            // Color display box with border
            GameObject borderObj = new GameObject("TargetBorder");
            borderObj.transform.SetParent(container.transform, false);
            Image border = borderObj.AddComponent<Image>();
            border.color = new Color(0.9f, 0.9f, 1f, 0.9f);
            RectTransform borderRect = borderObj.GetComponent<RectTransform>();
            borderRect.anchoredPosition = new Vector2(0, -10);
            borderRect.sizeDelta = new Vector2(110, 110);
            
            // Main color box
            GameObject colorObj = new GameObject("TargetColorBox");
            colorObj.transform.SetParent(borderObj.transform, false);
            targetColorBox = colorObj.AddComponent<Image>();
            targetColorBox.color = Color.magenta;
            RectTransform colorRect = colorObj.GetComponent<RectTransform>();
            colorRect.anchorMin = Vector2.zero;
            colorRect.anchorMax = Vector2.one;
            colorRect.offsetMin = new Vector2(4, 4);
            colorRect.offsetMax = new Vector2(-4, -4);
            
            targetColorGlow = border; // Use border as glow reference
        }
        
        private void CreateStyledMixedColorDisplay()
        {
            // Container with dark rounded background
            GameObject container = CreateUIPanel("MixedPanel", mainCanvas.transform,
                new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0.5f),
                new Vector2(-40, 0), new Vector2(140, 180));
            
            // Semi-transparent dark background
            Image containerBg = container.AddComponent<Image>();
            containerBg.color = new Color(0.1f, 0.1f, 0.15f, 0.85f);
            
            // Hand icon (your creation)
            GameObject iconObj = new GameObject("MixedIcon");
            iconObj.transform.SetParent(container.transform, false);
            Image icon = iconObj.AddComponent<Image>();
            icon.color = new Color(1f, 0.9f, 0.7f, 0.8f);
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(0, 65);
            iconRect.sizeDelta = new Vector2(35, 35);
            
            // Color display box with border
            GameObject borderObj = new GameObject("MixedBorder");
            borderObj.transform.SetParent(container.transform, false);
            Image border = borderObj.AddComponent<Image>();
            border.color = new Color(0.6f, 0.6f, 0.7f, 0.9f);
            RectTransform borderRect = borderObj.GetComponent<RectTransform>();
            borderRect.anchoredPosition = new Vector2(0, -10);
            borderRect.sizeDelta = new Vector2(110, 110);
            
            // Main color box
            GameObject colorObj = new GameObject("MixedColorBox");
            colorObj.transform.SetParent(borderObj.transform, false);
            mixedColorBox = colorObj.AddComponent<Image>();
            mixedColorBox.color = Color.black;
            RectTransform colorRect = colorObj.GetComponent<RectTransform>();
            colorRect.anchorMin = Vector2.zero;
            colorRect.anchorMax = Vector2.one;
            colorRect.offsetMin = new Vector2(4, 4);
            colorRect.offsetMax = new Vector2(-4, -4);
            
            mixedColorGlow = border;
        }
        
        private void CreateStyledMatchIndicator()
        {
            // Simple arrow/indicator in the middle
            GameObject arrowObj = new GameObject("MatchArrow");
            arrowObj.transform.SetParent(mainCanvas.transform, false);
            
            matchArrow = arrowObj.AddComponent<Image>();
            matchArrow.color = new Color(1f, 0.85f, 0.3f, 0.9f); // Golden yellow
            
            RectTransform arrowRect = arrowObj.GetComponent<RectTransform>();
            arrowRect.anchorMin = new Vector2(0.5f, 0.5f);
            arrowRect.anchorMax = new Vector2(0.5f, 0.5f);
            arrowRect.anchoredPosition = new Vector2(0, 0);
            arrowRect.sizeDelta = new Vector2(120, 8);
            
            // Checkmark (hidden until matched)
            GameObject checkObj = new GameObject("Checkmark");
            checkObj.transform.SetParent(mainCanvas.transform, false);
            checkmarkIcon = checkObj.AddComponent<Image>();
            checkmarkIcon.color = new Color(0.3f, 1f, 0.4f);
            
            RectTransform checkRect = checkObj.GetComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkRect.anchoredPosition = new Vector2(0, 0);
            checkRect.sizeDelta = new Vector2(60, 60);
            checkmarkIcon.gameObject.SetActive(false);
        }
        
        private void CreateStyledSensorBars()
        {
            // Container for sensor bars at bottom center
            GameObject sensorPanel = CreateUIPanel("SensorPanel", mainCanvas.transform,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 25), new Vector2(360, 160));
            
            // Semi-transparent dark background
            Image panelBg = sensorPanel.AddComponent<Image>();
            panelBg.color = new Color(0.1f, 0.1f, 0.15f, 0.8f);
            
            // Create 3 sensor bars: R, G, B with proper spacing
            redSensorBar = CreateStyledSensorBar(sensorPanel.transform, -100, Color.red, "Red");
            greenSensorBar = CreateStyledSensorBar(sensorPanel.transform, 0, new Color(0.2f, 0.9f, 0.3f), "Green");
            blueSensorBar = CreateStyledSensorBar(sensorPanel.transform, 100, new Color(0.3f, 0.5f, 1f), "Blue");
        }
        
        private Image CreateStyledSensorBar(Transform parent, float xPos, Color color, string name)
        {
            // Bar container
            GameObject barContainer = new GameObject(name + "Container");
            barContainer.transform.SetParent(parent, false);
            RectTransform containerRect = barContainer.AddComponent<RectTransform>();
            containerRect.anchoredPosition = new Vector2(xPos, 10);
            containerRect.sizeDelta = new Vector2(50, 120);
            
            // Background bar (dark)
            GameObject bgObj = new GameObject(name + "Bg");
            bgObj.transform.SetParent(barContainer.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.25f, 0.9f);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            
            // Fill bar
            GameObject fillObj = new GameObject(name + "Fill");
            fillObj.transform.SetParent(barContainer.transform, false);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = color;
            
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0.1f, 0.05f);
            fillRect.anchorMax = new Vector2(0.9f, 0.05f);
            fillRect.pivot = new Vector2(0.5f, 0);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fillRect.sizeDelta = new Vector2(0, 5); // Start small
            
            // Color indicator circle at bottom
            GameObject indicatorObj = new GameObject(name + "Indicator");
            indicatorObj.transform.SetParent(barContainer.transform, false);
            Image indicator = indicatorObj.AddComponent<Image>();
            indicator.color = color;
            RectTransform indRect = indicatorObj.GetComponent<RectTransform>();
            indRect.anchoredPosition = new Vector2(0, -50);
            indRect.sizeDelta = new Vector2(25, 25);
            
            return fillImage;
        }
        
        private GameObject CreateUIPanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 position, Vector2 size)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return panel;
        }
        
        private void CreateTargetColorDisplay()
        {
            // Large target color box with glow
            GameObject targetPanel = new GameObject("TargetColorPanel");
            targetPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = targetPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0.5f);
            panelRect.anchorMax = new Vector2(0, 0.5f);
            panelRect.pivot = new Vector2(0, 0.5f);
            panelRect.anchoredPosition = new Vector2(50, 50);
            panelRect.sizeDelta = new Vector2(180, 180);
            
            // Outer glow
            GameObject glowObj = new GameObject("TargetGlow");
            glowObj.transform.SetParent(targetPanel.transform, false);
            targetColorGlow = glowObj.AddComponent<Image>();
            targetColorGlow.color = new Color(1f, 1f, 1f, 0.5f);
            RectTransform glowRect = glowObj.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero;
            glowRect.anchorMax = Vector2.one;
            glowRect.offsetMin = new Vector2(-15, -15);
            glowRect.offsetMax = new Vector2(15, 15);
            
            // Main color box
            GameObject boxObj = new GameObject("TargetColorBox");
            boxObj.transform.SetParent(targetPanel.transform, false);
            targetColorBox = boxObj.AddComponent<Image>();
            targetColorBox.color = Color.magenta;
            RectTransform boxRect = boxObj.GetComponent<RectTransform>();
            boxRect.anchorMin = Vector2.zero;
            boxRect.anchorMax = Vector2.one;
            boxRect.offsetMin = Vector2.zero;
            boxRect.offsetMax = Vector2.zero;
            
            // "Eye" icon to indicate "look at this" (visual only)
            CreateEyeIcon(targetPanel.transform);
        }
        
        private void CreateEyeIcon(Transform parent)
        {
            // Simple circle to represent "look here"
            GameObject eyeObj = new GameObject("LookIcon");
            eyeObj.transform.SetParent(parent, false);
            
            Image eyeIcon = eyeObj.AddComponent<Image>();
            eyeIcon.color = Color.white;
            
            RectTransform eyeRect = eyeObj.GetComponent<RectTransform>();
            eyeRect.anchoredPosition = new Vector2(0, -110);
            eyeRect.sizeDelta = new Vector2(40, 40);
            
            // Pupil
            GameObject pupilObj = new GameObject("Pupil");
            pupilObj.transform.SetParent(eyeObj.transform, false);
            Image pupil = pupilObj.AddComponent<Image>();
            pupil.color = Color.black;
            RectTransform pupilRect = pupilObj.GetComponent<RectTransform>();
            pupilRect.sizeDelta = new Vector2(20, 20);
        }
        
        private void CreateMixedColorDisplay()
        {
            // Large mixed color box (what player creates)
            GameObject mixedPanel = new GameObject("MixedColorPanel");
            mixedPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = mixedPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1, 0.5f);
            panelRect.anchorMax = new Vector2(1, 0.5f);
            panelRect.pivot = new Vector2(1, 0.5f);
            panelRect.anchoredPosition = new Vector2(-50, 50);
            panelRect.sizeDelta = new Vector2(180, 180);
            
            // Outer glow
            GameObject glowObj = new GameObject("MixedGlow");
            glowObj.transform.SetParent(mixedPanel.transform, false);
            mixedColorGlow = glowObj.AddComponent<Image>();
            mixedColorGlow.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            RectTransform glowRect = glowObj.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero;
            glowRect.anchorMax = Vector2.one;
            glowRect.offsetMin = new Vector2(-15, -15);
            glowRect.offsetMax = new Vector2(15, 15);
            
            // Main color box
            GameObject boxObj = new GameObject("MixedColorBox");
            boxObj.transform.SetParent(mixedPanel.transform, false);
            mixedColorBox = boxObj.AddComponent<Image>();
            mixedColorBox.color = Color.black;
            RectTransform boxRect = boxObj.GetComponent<RectTransform>();
            boxRect.anchorMin = Vector2.zero;
            boxRect.anchorMax = Vector2.one;
            boxRect.offsetMin = Vector2.zero;
            boxRect.offsetMax = Vector2.zero;
            
            // Hand icon to show "your creation"
            CreateHandIcon(mixedPanel.transform);
        }
        
        private void CreateHandIcon(Transform parent)
        {
            GameObject handObj = new GameObject("HandIcon");
            handObj.transform.SetParent(parent, false);
            
            Image handIcon = handObj.AddComponent<Image>();
            handIcon.color = new Color(1f, 0.9f, 0.7f); // Skin tone
            
            RectTransform handRect = handObj.GetComponent<RectTransform>();
            handRect.anchoredPosition = new Vector2(0, -110);
            handRect.sizeDelta = new Vector2(50, 50);
        }
        
        private void CreateMatchArrow()
        {
            // Arrow pointing from left to right (Target → Mixed)
            GameObject arrowObj = new GameObject("MatchArrow");
            arrowObj.transform.SetParent(mainCanvas.transform, false);
            
            matchArrow = arrowObj.AddComponent<Image>();
            matchArrow.color = Color.yellow;
            
            RectTransform arrowRect = arrowObj.GetComponent<RectTransform>();
            arrowRect.anchorMin = new Vector2(0.5f, 0.5f);
            arrowRect.anchorMax = new Vector2(0.5f, 0.5f);
            arrowRect.anchoredPosition = new Vector2(0, 50);
            arrowRect.sizeDelta = new Vector2(200, 40);
            
            // Checkmark (hidden until matched)
            GameObject checkObj = new GameObject("Checkmark");
            checkObj.transform.SetParent(mainCanvas.transform, false);
            checkmarkIcon = checkObj.AddComponent<Image>();
            checkmarkIcon.color = new Color(0.2f, 1f, 0.3f); // Green
            
            RectTransform checkRect = checkObj.GetComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkRect.anchoredPosition = new Vector2(0, 50);
            checkRect.sizeDelta = new Vector2(80, 80);
            checkmarkIcon.gameObject.SetActive(false);
        }
        
        private void CreateSensorBars()
        {
            // Container for sensor bars at bottom
            GameObject sensorPanel = new GameObject("SensorPanel");
            sensorPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = sensorPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.anchoredPosition = new Vector2(0, 30);
            panelRect.sizeDelta = new Vector2(500, 200);
            
            // Panel background
            Image panelBg = sensorPanel.AddComponent<Image>();
            panelBg.color = new Color(0, 0, 0, 0.4f);
            
            // Create 3 sensor bars: R, G, B
            redSensorBar = CreateSensorBar(sensorPanel.transform, -150, Color.red, "RedBar");
            greenSensorBar = CreateSensorBar(sensorPanel.transform, 0, Color.green, "GreenBar");
            blueSensorBar = CreateSensorBar(sensorPanel.transform, 150, Color.blue, "BlueBar");
            
            // Create sensor icons (colored circles)
            redSensorIcon = CreateSensorIcon(sensorPanel.transform, -150, Color.red);
            greenSensorIcon = CreateSensorIcon(sensorPanel.transform, 0, Color.green);
            blueSensorIcon = CreateSensorIcon(sensorPanel.transform, 150, Color.blue);
        }
        
        private Image CreateSensorBar(Transform parent, float xPos, Color color, string name)
        {
            // Background
            GameObject bgObj = new GameObject(name + "Bg");
            bgObj.transform.SetParent(parent, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(color.r * 0.3f, color.g * 0.3f, color.b * 0.3f, 0.8f);
            
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchoredPosition = new Vector2(xPos, 40);
            bgRect.sizeDelta = new Vector2(60, sensorBarMaxHeight);
            
            // Fill bar
            GameObject fillObj = new GameObject(name + "Fill");
            fillObj.transform.SetParent(bgObj.transform, false);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = color;
            
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(1, 0);
            fillRect.pivot = new Vector2(0.5f, 0);
            fillRect.anchoredPosition = Vector2.zero;
            fillRect.sizeDelta = new Vector2(0, 10); // Start at bottom
            
            return fillImage;
        }
        
        private Image CreateSensorIcon(Transform parent, float xPos, Color color)
        {
            GameObject iconObj = new GameObject("SensorIcon");
            iconObj.transform.SetParent(parent, false);
            
            Image icon = iconObj.AddComponent<Image>();
            icon.color = color;
            
            RectTransform rect = iconObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(xPos, -50);
            rect.sizeDelta = new Vector2(50, 50);
            
            return icon;
        }
        
        private void CreateCelebrationEffects()
        {
            celebrationPanel = new GameObject("CelebrationPanel");
            celebrationPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = celebrationPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Create stars around the screen
            celebrationStars = new Image[8];
            for (int i = 0; i < 8; i++)
            {
                GameObject starObj = new GameObject($"Star{i}");
                starObj.transform.SetParent(celebrationPanel.transform, false);
                
                celebrationStars[i] = starObj.AddComponent<Image>();
                celebrationStars[i].color = new Color(1f, 0.9f, 0.2f); // Golden
                
                float angle = (360f / 8) * i;
                float radius = 300f;
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                
                RectTransform starRect = starObj.GetComponent<RectTransform>();
                starRect.anchoredPosition = new Vector2(x, y);
                starRect.sizeDelta = new Vector2(60, 60);
            }
            
            celebrationPanel.SetActive(false);
        }
        
        // ============ PUBLIC METHODS ============
        
        /// <summary>
        /// Set the target color to match (visual only - no text!)
        /// </summary>
        public void SetTargetColor(Color color)
        {
            targetColor = color;
            if (targetColorBox != null)
            {
                targetColorBox.color = color;
            }
            if (targetColorGlow != null)
            {
                targetColorGlow.color = new Color(color.r, color.g, color.b, 0.5f);
            }
            
            isMatched = false;
            if (checkmarkIcon != null) checkmarkIcon.gameObject.SetActive(false);
            if (matchArrow != null) matchArrow.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Update RED sensor value (Sensor 1)
        /// </summary>
        public void SetRedSensor(float value)
        {
            redValue = Mathf.Clamp01(value);
            UpdateSensorBar(redSensorBar, redValue);
            UpdateMixedColor();
        }
        
        /// <summary>
        /// Update GREEN sensor value (Sensor 2)
        /// </summary>
        public void SetGreenSensor(float value)
        {
            greenValue = Mathf.Clamp01(value);
            UpdateSensorBar(greenSensorBar, greenValue);
            UpdateMixedColor();
        }
        
        /// <summary>
        /// Update BLUE sensor value (Sensor 3)
        /// </summary>
        public void SetBlueSensor(float value)
        {
            blueValue = Mathf.Clamp01(value);
            UpdateSensorBar(blueSensorBar, blueValue);
            UpdateMixedColor();
        }
        
        /// <summary>
        /// Update all 3 sensors at once
        /// </summary>
        public void SetAllSensors(float red, float green, float blue)
        {
            SetRedSensor(red);
            SetGreenSensor(green);
            SetBlueSensor(blue);
        }
        
        private void UpdateSensorBar(Image bar, float value)
        {
            if (bar == null) return;
            
            RectTransform rect = bar.GetComponent<RectTransform>();
            // Use 100 as the max height for the fill bar inside the container
            float maxBarHeight = 100f;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, maxBarHeight * value);
        }
        
        private void UpdateMixedColor()
        {
            mixedColor = new Color(redValue, greenValue, blueValue);
            
            if (mixedColorBox != null)
            {
                mixedColorBox.color = mixedColor;
            }
            if (mixedColorGlow != null)
            {
                mixedColorGlow.color = new Color(mixedColor.r, mixedColor.g, mixedColor.b, 0.5f);
            }
            
            // Check if colors match (within tolerance)
            CheckColorMatch();
        }
        
        [Header("Difficulty")]
        [SerializeField] private float matchTolerance = 0.4f; // 0.0 = Exact match, 1.0 = Any color
        
        private void CheckColorMatch()
        {
            // Calculate distance between mixed color and target color
            // Using simple Euclidean distance in RGB space
            float distance = Vector4.Distance(
                new Vector4(mixedColor.r, mixedColor.g, mixedColor.b, 0),
                new Vector4(targetColor.r, targetColor.g, targetColor.b, 0)
            );
            
            // Allow match if distance is within tolerance
            // AND ensure the user actually has some color (avoid matching black to black if both are off)
            bool hasColor = mixedColor.r > 0.1f || mixedColor.g > 0.1f || mixedColor.b > 0.1f;
            isMatched = (distance <= matchTolerance) && hasColor;
            
            // Visual feedback
            if (matchArrow != null)
            {
                if (isMatched)
                {
                    matchArrow.color = new Color(0.2f, 1f, 0.3f); // GREEN = Ready! Press SPACE!
                }
                else
                {
                    matchArrow.color = new Color(1f, 0.3f, 0.3f); // Red = Not close enough
                }
            }
        }
        
        // Track successful matches
        private int successCount = 0;
        
        /// <summary>
        /// Press SPACE to confirm your color match!
        /// </summary>
        public void OnConfirm()
        {
            // Re-check just in case
            CheckColorMatch();
            
            if (isMatched)
            {
                successCount++;
                Debug.Log($"🎉 SUCCESS! Total: {successCount}");
                ShowCelebration();
            }
            else
            {
                float distance = Vector4.Distance(
                   new Vector4(mixedColor.r, mixedColor.g, mixedColor.b, 0),
                   new Vector4(targetColor.r, targetColor.g, targetColor.b, 0)
                );
                Debug.Log($"❌ Not matched yet! Distance: {distance:F2} (Needs < {matchTolerance:F2})");
                ShowTryAgainFeedback();
            }
        }
        
        /// <summary>
        /// Shows a gentle "try again" visual feedback
        /// </summary>
        public void ShowTryAgainFeedback()
        {
            StartCoroutine(ShakeEffect());
        }
        
        private void ShowCelebration()
        {
            if (checkmarkIcon != null)
            {
                checkmarkIcon.gameObject.SetActive(true);
            }
            if (matchArrow != null)
            {
                matchArrow.gameObject.SetActive(false);
            }
            if (celebrationPanel != null)
            {
                celebrationPanel.SetActive(true);
                StartCoroutine(AnimateCelebrationAndNextRound());
            }
        }
        
        private IEnumerator AnimateCelebrationAndNextRound()
        {
            float duration = 2.5f; // Celebrate for 2.5 seconds
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                // Rotate and scale stars
                for (int i = 0; i < celebrationStars.Length; i++)
                {
                    if (celebrationStars[i] != null)
                    {
                        float scale = 1f + Mathf.Sin((elapsed * 3f) + i) * 0.3f;
                        celebrationStars[i].transform.localScale = Vector3.one * scale;
                        celebrationStars[i].transform.Rotate(0, 0, Time.deltaTime * 100f);
                    }
                }
                
                // Pulse the checkmark
                if (checkmarkIcon != null)
                {
                    float pulse = 1f + Mathf.Sin(elapsed * 5f) * 0.2f;
                    checkmarkIcon.transform.localScale = Vector3.one * pulse;
                }
                
                yield return null;
            }
            
            // Hide celebration
            celebrationPanel.SetActive(false);
            if (checkmarkIcon != null) checkmarkIcon.gameObject.SetActive(false);
            if (matchArrow != null) matchArrow.gameObject.SetActive(true);
            
            // START NEXT ROUND!
            StartNextRound();
        }
        
        /// <summary>
        /// Starts a new round with a random target color
        /// </summary>
        private void StartNextRound()
        {
            // Generate a new random target color
            Color newTargetColor = GenerateRandomTargetColor();
            SetTargetColor(newTargetColor);
            
            // Reset the player's mixed color
            ResetMixedColor();
            
            Debug.Log($"🌈 NEW ROUND! Match this color: R={newTargetColor.r:F2}, G={newTargetColor.g:F2}, B={newTargetColor.b:F2}");
            Debug.Log("🎮 Use A/S/D to add colors, R to reset, SPACE to confirm!");
        }
        
        /// <summary>
        /// Generates a random pastel-ish color that's easy to match
        /// </summary>
        private Color GenerateRandomTargetColor()
        {
            // SIMPLE colors - easy to match!
            // Pure colors: just press one or two keys
            Color[] presetColors = new Color[]
            {
                new Color(1f, 0f, 0f),    // RED = just A
                new Color(0f, 1f, 0f),    // GREEN = just S
                new Color(0f, 0f, 1f),    // BLUE = just D
                new Color(1f, 1f, 0f),    // YELLOW = A + S
                new Color(1f, 0f, 1f),    // MAGENTA = A + D
                new Color(0f, 1f, 1f),    // CYAN = S + D
            };
            
            // Pick a random color from presets
            int index = UnityEngine.Random.Range(0, presetColors.Length);
            return presetColors[index];
        }
        
        /// <summary>
        /// Resets the mixed color to black and notifies the sensor input
        /// </summary>
        private void ResetMixedColor()
        {
            redValue = 0f;
            greenValue = 0f;
            blueValue = 0f;
            mixedColor = Color.black;
            
            if (mixedColorBox != null)
            {
                mixedColorBox.color = Color.black;
            }
            
            // Also reset the sensor input
            var sensorInput = FindObjectOfType<ColorMatchGarden.Core.ThreeSensorInput>();
            if (sensorInput != null)
            {
                sensorInput.SetSensorValues(0f, 0f, 0f);
            }
            
            // Update UI
            UpdateSensorBar(redSensorBar, 0f);
            UpdateSensorBar(greenSensorBar, 0f);
            UpdateSensorBar(blueSensorBar, 0f);
        }
        
        /// <summary>
        /// Get the current success count
        /// </summary>
        public int GetSuccessCount() => successCount;
        
        private IEnumerator ShakeEffect()
        {
            if (mixedColorBox == null) yield break;
            
            Vector3 originalPos = mixedColorBox.transform.localPosition;
            float shakeDuration = 0.3f;
            float elapsed = 0f;
            
            while (elapsed < shakeDuration)
            {
                elapsed += Time.deltaTime;
                float x = Random.Range(-10f, 10f);
                float y = Random.Range(-10f, 10f);
                mixedColorBox.transform.localPosition = originalPos + new Vector3(x, y, 0);
                yield return null;
            }
            
            mixedColorBox.transform.localPosition = originalPos;
        }
        
        private IEnumerator ShowWelcomeAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            
            // Pulse the target color to draw attention
            if (targetColorBox != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    targetColorBox.transform.localScale = Vector3.one * 1.2f;
                    yield return new WaitForSeconds(0.2f);
                    targetColorBox.transform.localScale = Vector3.one;
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        
        /// <summary>
        /// Get if current color matches target
        /// </summary>
        public bool IsColorMatched() => isMatched;
        
        /// <summary>
        /// Get the current mixed color
        /// </summary>
        public Color GetMixedColor() => mixedColor;
    }
}
