using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ColorMatchGarden.UI
{
    /// <summary>
    /// Visual-only UI for 5-sensor color mixing game.
    /// NO TEXT - only colors, bars, and visual feedback!
    /// Perfect for neurodiverse children.
    /// </summary>
    public class FiveSensorVisualUI : MonoBehaviour
    {
        [Header("Main Canvas")]
        private Canvas mainCanvas;
        
        [Header("Color Displays")]
        private Image targetColorBox;
        private Image mixedColorBox;
        private Image matchArrow;
        
        [Header("5 Sensor Bars")]
        private Image[] sensorBars = new Image[5];
        private Image[] sensorIcons = new Image[5];
        
        [Header("Celebration")]
        private GameObject celebrationPanel;
        private Image[] celebrationStars;
        private Image bigCheckmark;
        
        [Header("Settings")]
        [SerializeField] private float barMaxHeight = 120f;
        
        // Colors for each sensor indicator
        // Colors for each sensor indicator (MUST MATCH FiveSensorInput.cs logic!)
        private readonly Color[] sensorColors = new Color[]
        {
            Color.red,                    // Thumb
            Color.yellow,                 // Index
            Color.green,                  // Middle
            Color.cyan,                   // Ring
            new Color(0.6f, 0f, 1f)       // Pinky (Purple)
        };
        
        private Color targetColor = new Color(1f, 0.4f, 0.7f);
        private Color currentMixedColor = Color.black;
        private Core.FiveSensorInput sensorInput;
        
        private void Start()
        {
            CreateUI();
            
            // Find and subscribe to sensor input
            sensorInput = FindObjectOfType<Core.FiveSensorInput>();
            if (sensorInput != null)
            {
                sensorInput.OnColorChanged += OnColorChanged;
                sensorInput.OnSensorValuesChanged += OnSensorValuesChanged;
                sensorInput.OnConfirmGesture += OnConfirm;
                Debug.Log("✅ [FiveSensorVisualUI] Connected to FiveSensorInput!");
            }
            
            // Set initial target color
            SetRandomTargetColor();
            
            StartCoroutine(PulseTargetColor());
        }
        
        private void OnDestroy()
        {
            if (sensorInput != null)
            {
                sensorInput.OnColorChanged -= OnColorChanged;
                sensorInput.OnSensorValuesChanged -= OnSensorValuesChanged;
                sensorInput.OnConfirmGesture -= OnConfirm;
            }
        }
        
        private void CreateUI()
        {
            // Create Canvas
            GameObject canvasObj = new GameObject("FiveSensorUI");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 100;
            
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasObj.AddComponent<GraphicRaycaster>();
            
            CreateColorDisplays();
            CreateSensorBars();
            CreateCelebrationPanel();
        }
        
        private void CreateColorDisplays()
        {
            // TARGET COLOR (Left side - BIG)
            GameObject targetPanel = new GameObject("TargetPanel");
            targetPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform targetRect = targetPanel.AddComponent<RectTransform>();
            targetRect.anchorMin = new Vector2(0, 0.5f);
            targetRect.anchorMax = new Vector2(0, 0.5f);
            targetRect.pivot = new Vector2(0, 0.5f);
            targetRect.anchoredPosition = new Vector2(40, 80);
            targetRect.sizeDelta = new Vector2(150, 150);
            
            // Glow behind target
            GameObject targetGlow = new GameObject("TargetGlow");
            targetGlow.transform.SetParent(targetPanel.transform, false);
            Image glowImg = targetGlow.AddComponent<Image>();
            glowImg.color = new Color(1, 1, 1, 0.4f);
            RectTransform glowRect = targetGlow.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero;
            glowRect.anchorMax = Vector2.one;
            glowRect.offsetMin = new Vector2(-15, -15);
            glowRect.offsetMax = new Vector2(15, 15);
            
            // Target color box
            GameObject targetBox = new GameObject("TargetColorBox");
            targetBox.transform.SetParent(targetPanel.transform, false);
            targetColorBox = targetBox.AddComponent<Image>();
            targetColorBox.color = targetColor;
            RectTransform boxRect = targetBox.GetComponent<RectTransform>();
            boxRect.anchorMin = Vector2.zero;
            boxRect.anchorMax = Vector2.one;
            boxRect.offsetMin = Vector2.zero;
            boxRect.offsetMax = Vector2.zero;
            
            // Eye icon (look at this!)
            CreateIconCircle(targetPanel.transform, new Vector2(0, -100), 35, Color.white, Color.black);
            
            // ARROW (Center)
            GameObject arrowObj = new GameObject("MatchArrow");
            arrowObj.transform.SetParent(mainCanvas.transform, false);
            matchArrow = arrowObj.AddComponent<Image>();
            matchArrow.color = Color.yellow;
            RectTransform arrowRect = arrowObj.GetComponent<RectTransform>();
            arrowRect.anchorMin = new Vector2(0.5f, 0.5f);
            arrowRect.anchorMax = new Vector2(0.5f, 0.5f);
            arrowRect.anchoredPosition = new Vector2(0, 80);
            arrowRect.sizeDelta = new Vector2(150, 40);
            
            // MIXED COLOR (Right side - BIG)
            GameObject mixedPanel = new GameObject("MixedPanel");
            mixedPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform mixedRect = mixedPanel.AddComponent<RectTransform>();
            mixedRect.anchorMin = new Vector2(1, 0.5f);
            mixedRect.anchorMax = new Vector2(1, 0.5f);
            mixedRect.pivot = new Vector2(1, 0.5f);
            mixedRect.anchoredPosition = new Vector2(-40, 80);
            mixedRect.sizeDelta = new Vector2(150, 150);
            
            // Glow behind mixed
            GameObject mixedGlow = new GameObject("MixedGlow");
            mixedGlow.transform.SetParent(mixedPanel.transform, false);
            Image mixedGlowImg = mixedGlow.AddComponent<Image>();
            mixedGlowImg.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
            RectTransform mixedGlowRect = mixedGlow.GetComponent<RectTransform>();
            mixedGlowRect.anchorMin = Vector2.zero;
            mixedGlowRect.anchorMax = Vector2.one;
            mixedGlowRect.offsetMin = new Vector2(-15, -15);
            mixedGlowRect.offsetMax = new Vector2(15, 15);
            
            // Mixed color box
            GameObject mixedBox = new GameObject("MixedColorBox");
            mixedBox.transform.SetParent(mixedPanel.transform, false);
            mixedColorBox = mixedBox.AddComponent<Image>();
            mixedColorBox.color = Color.black;
            RectTransform mixedBoxRect = mixedBox.GetComponent<RectTransform>();
            mixedBoxRect.anchorMin = Vector2.zero;
            mixedBoxRect.anchorMax = Vector2.one;
            mixedBoxRect.offsetMin = Vector2.zero;
            mixedBoxRect.offsetMax = Vector2.zero;
            
            // Hand icon (your creation!)
            CreateIconCircle(mixedPanel.transform, new Vector2(0, -100), 35, new Color(1f, 0.9f, 0.7f), new Color(0.8f, 0.6f, 0.4f));
        }
        
        private void CreateIconCircle(Transform parent, Vector2 pos, float size, Color outerColor, Color innerColor)
        {
            GameObject outer = new GameObject("Icon");
            outer.transform.SetParent(parent, false);
            Image outerImg = outer.AddComponent<Image>();
            outerImg.color = outerColor;
            RectTransform outerRect = outer.GetComponent<RectTransform>();
            outerRect.anchoredPosition = pos;
            outerRect.sizeDelta = new Vector2(size, size);
            
            GameObject inner = new GameObject("Inner");
            inner.transform.SetParent(outer.transform, false);
            Image innerImg = inner.AddComponent<Image>();
            innerImg.color = innerColor;
            RectTransform innerRect = inner.GetComponent<RectTransform>();
            innerRect.sizeDelta = new Vector2(size * 0.5f, size * 0.5f);
        }
        
        private void CreateSensorBars()
        {
            // Container at bottom
            GameObject barPanel = new GameObject("SensorBarsPanel");
            barPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = barPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.anchoredPosition = new Vector2(0, 20);
            panelRect.sizeDelta = new Vector2(600, 180);
            
            Image panelBg = barPanel.AddComponent<Image>();
            panelBg.color = new Color(0, 0, 0, 0.5f);
            
            // Create 5 sensor bars
            float barWidth = 70f;
            float spacing = 100f;
            float startX = -2 * spacing;
            
            for (int i = 0; i < 5; i++)
            {
                float xPos = startX + i * spacing;
                
                // Bar background
                GameObject barBg = new GameObject($"Bar{i}Bg");
                barBg.transform.SetParent(barPanel.transform, false);
                Image barBgImg = barBg.AddComponent<Image>();
                barBgImg.color = new Color(sensorColors[i].r * 0.3f, sensorColors[i].g * 0.3f, sensorColors[i].b * 0.3f, 0.8f);
                RectTransform barBgRect = barBg.GetComponent<RectTransform>();
                barBgRect.anchoredPosition = new Vector2(xPos, 50);
                barBgRect.sizeDelta = new Vector2(barWidth, barMaxHeight);
                
                // Bar fill
                GameObject barFill = new GameObject($"Bar{i}Fill");
                barFill.transform.SetParent(barBg.transform, false);
                sensorBars[i] = barFill.AddComponent<Image>();
                sensorBars[i].color = sensorColors[i];
                RectTransform fillRect = barFill.GetComponent<RectTransform>();
                fillRect.anchorMin = new Vector2(0, 0);
                fillRect.anchorMax = new Vector2(1, 0);
                fillRect.pivot = new Vector2(0.5f, 0);
                fillRect.anchoredPosition = Vector2.zero;
                fillRect.sizeDelta = new Vector2(0, 10);
                
                // Sensor icon (colored circle)
                GameObject icon = new GameObject($"Icon{i}");
                icon.transform.SetParent(barPanel.transform, false);
                sensorIcons[i] = icon.AddComponent<Image>();
                sensorIcons[i].color = sensorColors[i];
                RectTransform iconRect = icon.GetComponent<RectTransform>();
                iconRect.anchoredPosition = new Vector2(xPos, -30);
                iconRect.sizeDelta = new Vector2(45, 45);
            }
        }
        
        private void CreateCelebrationPanel()
        {
            celebrationPanel = new GameObject("CelebrationPanel");
            celebrationPanel.transform.SetParent(mainCanvas.transform, false);
            
            RectTransform panelRect = celebrationPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Semi-transparent overlay
            Image overlay = celebrationPanel.AddComponent<Image>();
            overlay.color = new Color(1f, 0.9f, 0.3f, 0.3f);
            
            // Big checkmark in center
            GameObject checkObj = new GameObject("BigCheckmark");
            checkObj.transform.SetParent(celebrationPanel.transform, false);
            bigCheckmark = checkObj.AddComponent<Image>();
            bigCheckmark.color = new Color(0.2f, 1f, 0.3f);
            RectTransform checkRect = checkObj.GetComponent<RectTransform>();
            checkRect.sizeDelta = new Vector2(200, 200);
            
            // Stars around
            celebrationStars = new Image[10];
            for (int i = 0; i < 10; i++)
            {
                GameObject star = new GameObject($"Star{i}");
                star.transform.SetParent(celebrationPanel.transform, false);
                celebrationStars[i] = star.AddComponent<Image>();
                celebrationStars[i].color = new Color(1f, 0.9f, 0.2f);
                
                float angle = (360f / 10) * i;
                float radius = 250f;
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                
                RectTransform starRect = star.GetComponent<RectTransform>();
                starRect.anchoredPosition = new Vector2(x, y);
                starRect.sizeDelta = new Vector2(50, 50);
            }
            
            celebrationPanel.SetActive(false);
        }
        
        // ============ EVENT HANDLERS ============
        
        private void OnColorChanged(Color color)
        {
            currentMixedColor = color;
            if (mixedColorBox != null)
            {
                mixedColorBox.color = color;
            }
            
            UpdateMatchIndicator();
        }
        
        private void OnSensorValuesChanged(float s1, float s2, float s3, float s4, float s5)
        {
            UpdateSensorBar(0, s1);
            UpdateSensorBar(1, s2);
            UpdateSensorBar(2, s3);
            UpdateSensorBar(3, s4);
            UpdateSensorBar(4, s5);
        }
        
        private void UpdateSensorBar(int index, float value)
        {
            if (index < 0 || index >= sensorBars.Length || sensorBars[index] == null) return;
            
            RectTransform rect = sensorBars[index].GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, barMaxHeight * value);
            
            // Pulse icon when active
            if (sensorIcons[index] != null)
            {
                float scale = 1f + value * 0.3f;
                sensorIcons[index].transform.localScale = Vector3.one * scale;
            }
        }
        
        private void UpdateMatchIndicator()
        {
            if (matchArrow == null) return;
            
            float colorDiff = ColorDifference(currentMixedColor, targetColor);
            
            // MUCH MORE FORGIVING TOLERANCES
            if (colorDiff < 0.35f) // Was 0.15f
            {
                // MATCH!
                matchArrow.color = new Color(0.2f, 1f, 0.3f); // Green
            }
            else if (colorDiff < 0.5f) // Was 0.3f
            {
                // Close
                matchArrow.color = Color.yellow;
            }
            else
            {
                // Not close
                matchArrow.color = new Color(1f, 0.5f, 0.2f); // Orange
            }
        }
        
        private float ColorDifference(Color a, Color b)
        {
            // Simple absolute difference
            return (Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b)) / 3f;
        }
        
        private void OnConfirm()
        {
            float diff = ColorDifference(currentMixedColor, targetColor);
            
            // Tolerance set to 0.35f - Strict but fair
            // 0.25f was too hard, 0.45f was too easy (yellow matched red)
            if (diff < 0.35f)
            {
                // Close enough - CELEBRATE!
                StartCoroutine(ShowCelebration());
                
                // Manually trigger flower celebration to stay in sync!
                var flower = FindObjectOfType<Flowers.FiveSensorFlower>();
                if (flower != null) flower.PlayCelebration();
            }
            else
            {
                // Not matched - shake feedback
                ShowTryAgainFeedback();
            }
        }
        
        private IEnumerator ShowCelebration()
        {
            Debug.Log("🎉 CELEBRATION!");
            celebrationPanel.SetActive(true);
            
            float duration = 3f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                // Animate stars
                for (int i = 0; i < celebrationStars.Length; i++)
                {
                    if (celebrationStars[i] != null)
                    {
                        float scale = 1f + Mathf.Sin((elapsed * 4f) + i) * 0.4f;
                        celebrationStars[i].transform.localScale = Vector3.one * scale;
                        celebrationStars[i].transform.Rotate(0, 0, Time.deltaTime * 120f);
                    }
                }
                
                // Pulse checkmark
                if (bigCheckmark != null)
                {
                    float pulse = 1f + Mathf.Sin(elapsed * 6f) * 0.2f;
                    bigCheckmark.transform.localScale = Vector3.one * pulse;
                }
                
                yield return null;
            }
            
            celebrationPanel.SetActive(false);
            
            // Set new target color
            SetRandomTargetColor();
        }
        
        public void ShowTryAgainFeedback()
        {
            StartCoroutine(ShakeEffect());
        }

        private IEnumerator ShakeEffect()
        {
            if (mixedColorBox == null) yield break;
            
            Vector3 originalPos = mixedColorBox.transform.localPosition;
            Color originalColor = mixedColorBox.color;
            float duration = 0.5f;
            float elapsed = 0f;
            
            Debug.Log("❌ INSPECT HERE: Incorrect color! Flashing RED!");
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                // Shake
                float x = UnityEngine.Random.Range(-15f, 15f);
                float y = UnityEngine.Random.Range(-15f, 15f);
                mixedColorBox.transform.localPosition = originalPos + new Vector3(x, y, 0);
                
                // FLASH RED (Try Again Effect)
                float flash = Mathf.PingPong(elapsed * 10f, 1f);
                mixedColorBox.color = Color.Lerp(originalColor, Color.red, flash * 0.7f);
                
                yield return null;
            }
            
            mixedColorBox.transform.localPosition = originalPos;
            mixedColorBox.color = currentMixedColor; // Restore actual mixed color
        }
        
        private IEnumerator PulseTargetColor()
        {
            while (true)
            {
                if (targetColorBox != null)
                {
                    float pulse = 1f + Mathf.Sin(Time.time * 2f) * 0.05f;
                    targetColorBox.transform.localScale = Vector3.one * pulse;
                }
                yield return null;
            }
        }
        
        public void SetRandomTargetColor()
        {
            // EXPANDED TARGETS for VARIETY!
            // 0-4: Single Sensors
            // 5+: Combinations (Orange, Magenta, etc.)
            
            int roll = UnityEngine.Random.Range(0, 100);
            
            if (roll < 50) // 50% chance of Single Sensor (Simple)
            {
                int randomIndex = UnityEngine.Random.Range(0, 5);
                targetColor = sensorColors[randomIndex];
            }
            else // 50% chance of Combinations (Fun!)
            {
                // Defined combinations based on Average Mixing logic
                Color cRed = sensorColors[0];
                Color cYel = sensorColors[1];
                Color cGrn = sensorColors[2];
                Color cCyn = sensorColors[3];
                Color cPur = sensorColors[4];
                
                int combo = UnityEngine.Random.Range(0, 5);
                switch(combo)
                {
                    case 0: targetColor = (cRed + cYel) / 2f; break; // Orange (Thumb + Index)
                    case 1: targetColor = (cRed + cPur) / 2f; break; // Magenta (Thumb + Pinky)
                    case 2: targetColor = (cGrn + cPur) / 2f; break; // Mint/Slate (Middle + Pinky)
                    case 3: targetColor = (cYel + cCyn) / 2f; break; // Light Greenish (Index + Ring)
                    case 4: targetColor = (cRed + cCyn) / 2f; break; // Greyish/White (Thumb + Ring)
                }
            }
            
            // Rare chance for White (All fingers)
            if (UnityEngine.Random.value > 0.9f)
            {
                targetColor = Color.white; // Requires specific balancing or just "close enough"
                // Actually, ensure White is matchable. Average of all is distinct.
                // (R+Y+G+C+P)/5
                targetColor = (sensorColors[0]+sensorColors[1]+sensorColors[2]+sensorColors[3]+sensorColors[4])/5f;
            }
            
            if (targetColorBox != null)
            {
                targetColorBox.color = targetColor;
            }
            
            Debug.Log($"🎨 New VARIED target color: {targetColor}");
        }
        
        public void SetTargetColor(Color color)
        {
            targetColor = color;
            if (targetColorBox != null)
            {
                targetColorBox.color = color;
            }
        }
    }
}
