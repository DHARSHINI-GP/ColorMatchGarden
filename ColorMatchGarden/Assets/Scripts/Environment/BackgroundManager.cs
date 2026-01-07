using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGarden.Environment
{
    /// <summary>
    /// Sets up a beautiful background for the garden scene.
    /// Can use either a skybox material or a background image.
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        [Header("Background Mode")]
        [SerializeField] private BackgroundMode mode = BackgroundMode.GradientSkybox;
        
        [Header("Image Background")]
        [SerializeField] private Sprite backgroundImage;
        [SerializeField] private Canvas backgroundCanvas;
        [SerializeField] private Image backgroundImageComponent;
        
        [Header("Gradient Skybox Colors")]
        [SerializeField] private Color skyColorTop = new Color(0.6f, 0.8f, 1f);      // Soft blue
        [SerializeField] private Color skyColorMiddle = new Color(0.9f, 0.85f, 0.95f); // Soft lavender
        [SerializeField] private Color skyColorBottom = new Color(1f, 0.9f, 0.85f);   // Soft peach
        
        [Header("Ambient Particles")]
        [SerializeField] private bool enableFloatingParticles = true;
        [SerializeField] private ParticleSystem floatingDust;

        private Material skyboxMaterial;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            SetupBackground();
        }

        private void SetupBackground()
        {
            switch (mode)
            {
                case BackgroundMode.GradientSkybox:
                    SetupGradientSkybox();
                    break;
                case BackgroundMode.ImageBackground:
                    SetupImageBackground();
                    break;
                case BackgroundMode.SolidColor:
                    SetupSolidColor();
                    break;
            }
            
            if (enableFloatingParticles && floatingDust != null)
            {
                floatingDust.Play();
            }
        }

        private void SetupGradientSkybox()
        {
            // Set camera to use solid color as base
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.SolidColor;
                mainCamera.backgroundColor = skyColorMiddle;
            }
            
            // Set ambient lighting to match
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = skyColorTop;
            RenderSettings.ambientEquatorColor = skyColorMiddle;
            RenderSettings.ambientGroundColor = skyColorBottom;
        }

        private void SetupImageBackground()
        {
            if (backgroundImage == null)
            {
                Debug.LogWarning("[Background] No background image assigned!");
                SetupGradientSkybox();
                return;
            }

            // Create background canvas if not assigned
            if (backgroundCanvas == null)
            {
                CreateBackgroundCanvas();
            }

            if (backgroundImageComponent != null)
            {
                backgroundImageComponent.sprite = backgroundImage;
                backgroundImageComponent.preserveAspect = false;
            }

            // Set camera to render UI
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.Depth;
            }
        }

        private void CreateBackgroundCanvas()
        {
            // Create canvas
            GameObject canvasObj = new GameObject("BackgroundCanvas");
            backgroundCanvas = canvasObj.AddComponent<Canvas>();
            backgroundCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            backgroundCanvas.worldCamera = mainCamera;
            backgroundCanvas.planeDistance = 100; // Far back
            backgroundCanvas.sortingOrder = -100;  // Behind everything
            
            canvasObj.AddComponent<CanvasScaler>();
            
            // Create image
            GameObject imageObj = new GameObject("BackgroundImage");
            imageObj.transform.SetParent(canvasObj.transform, false);
            
            backgroundImageComponent = imageObj.AddComponent<Image>();
            
            RectTransform rect = imageObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void SetupSolidColor()
        {
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.SolidColor;
                mainCamera.backgroundColor = skyColorMiddle;
            }
        }

        /// <summary>
        /// Creates ambient floating particles for magical effect.
        /// </summary>
        public void CreateAmbientParticles()
        {
            if (floatingDust != null) return;

            GameObject particleObj = new GameObject("FloatingDust");
            particleObj.transform.SetParent(transform);
            particleObj.transform.position = mainCamera != null ? 
                mainCamera.transform.position + mainCamera.transform.forward * 5f : 
                Vector3.zero;

            floatingDust = particleObj.AddComponent<ParticleSystem>();
            
            var main = floatingDust.main;
            main.startLifetime = 8f;
            main.startSpeed = 0.2f;
            main.startSize = 0.05f;
            main.startColor = new Color(1f, 1f, 1f, 0.3f);
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 100;
            
            var emission = floatingDust.emission;
            emission.rateOverTime = 10f;
            
            var shape = floatingDust.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(15f, 8f, 10f);
            
            var colorOverLifetime = floatingDust.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.white, 0f), 
                    new GradientColorKey(Color.white, 1f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0f, 0f), 
                    new GradientAlphaKey(0.3f, 0.3f),
                    new GradientAlphaKey(0.3f, 0.7f),
                    new GradientAlphaKey(0f, 1f) 
                }
            );
            colorOverLifetime.color = gradient;
            
            floatingDust.Play();
        }

        // Public methods for runtime changes
        public void SetBackgroundColor(Color top, Color middle, Color bottom)
        {
            skyColorTop = top;
            skyColorMiddle = middle;
            skyColorBottom = bottom;
            SetupGradientSkybox();
        }

        public void SetBackgroundImage(Sprite image)
        {
            backgroundImage = image;
            mode = BackgroundMode.ImageBackground;
            SetupImageBackground();
        }
    }

    public enum BackgroundMode
    {
        GradientSkybox,
        ImageBackground,
        SolidColor
    }
}
