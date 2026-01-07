using UnityEngine;
using System.Collections;
using ColorMatchGarden.UI;
using ColorMatchGarden.Flowers;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Creates a CLEAN, MINIMAL garden scene - no clutter!
    /// Designed for neurodiverse children with calm, clear visuals.
    /// Updated for 5 FLEX SENSORS!
    /// </summary>
    public class CompleteSceneBuilder : MonoBehaviour
    {
        [Header("Build Options")]
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private bool showGuideCharacter = false; // OFF by default - less clutter!
        [SerializeField] private bool showVisualArrow = false;    // OFF by default
        [SerializeField] private bool showTargetOrb = false;      // OFF by default - UI shows target
        
        [Header("Ground Material")]
        [SerializeField] private Material grassMaterial;
        
        private void Start()
        {
            if (buildOnStart)
            {
                BuildCompleteScene();
            }
        }

        [ContextMenu("Build Complete Scene")]
        public void BuildCompleteScene()
        {
            Debug.Log("🌸 Building CLEAN Color Match Garden scene (5 SENSORS)...");
            
            // Clean up ALL old objects first
            CleanupScene();
            
            SetupCamera();
            SetupLighting();
            CreateGround();
            CreateCleanFlower();
            
            // Only create these if explicitly enabled
            if (showGuideCharacter) CreateSimpleButterfly();
            if (showTargetOrb) CreateTargetOrb();
            if (showVisualArrow) CreateArrow();
            
            CreateGameGuideUI();
            
            Debug.Log("✅ CLEAN scene build complete!");
        }
        
        private void CleanupScene()
        {
            // Remove ALL clutter
            string[] objectsToRemove = {
                "Friendly Guide", "Magic Flower", "Target Color Orb", 
                "Visual Arrow", "Garden Ground", "GameGuideUI", 
                "VisualGuideUI", "VisualGuideCanvas", "ThreeSensorInput",
                "FiveSensorInput", "FiveSensorUI", "Gesture Hints", 
                "Ambient Particles", "Grass", "FiveSensorFlower"
            };
            
            foreach (var name in objectsToRemove)
            {
                var obj = GameObject.Find(name);
                if (obj != null) DestroyImmediate(obj);
            }
            
            // Disable default Ground
            var defaultGround = GameObject.Find("Ground");
            if (defaultGround != null) defaultGround.SetActive(false);
        }

        private void SetupCamera()
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.transform.position = new Vector3(0, 3f, -7f);
                cam.transform.rotation = Quaternion.Euler(15f, 0, 0);
                cam.fieldOfView = 55f;
                cam.clearFlags = CameraClearFlags.Skybox; // Use the nice skybox!
            }
        }

        private void SetupLighting()
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (var light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    light.color = new Color(1f, 0.98f, 0.95f);
                    light.intensity = 1.2f;
                    light.shadows = LightShadows.Soft;
                }
            }
            
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.7f, 0.75f, 0.8f);
        }
        
        private void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Garden Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(4, 1, 4);
            
            // ALWAYS create a GUARANTEED BRIGHT GREEN material
            // This is the ONLY reliable way to ensure green ground!
            Material greenMat = new Material(Shader.Find("Standard"));
            greenMat.name = "BrightGrassMaterial";
            
            // Bright, vibrant grass green color
            greenMat.color = new Color(0.12f, 0.5f, 0.08f); // Rich grass green!
            
            // Make it matte (no shine)
            greenMat.SetFloat("_Glossiness", 0.0f);
            greenMat.SetFloat("_Metallic", 0.0f);
            greenMat.SetFloat("_Smoothness", 0.0f);
            
            // No emission
            greenMat.DisableKeyword("_EMISSION");
            greenMat.SetColor("_EmissionColor", Color.black);
            
            // Assign visual material
            Renderer renderer = ground.GetComponent<Renderer>();
            renderer.material = greenMat;
            renderer.sharedMaterial = greenMat;
            
            // Add the GUARANTEED fixer script to be safe
            if (ground.GetComponent<ColorMatchGarden.Environment.GuaranteedGreenGround>() == null)
            {
                ground.AddComponent<ColorMatchGarden.Environment.GuaranteedGreenGround>();
            }
            
            Debug.Log("🌿 Created BRIGHT GRASS GREEN ground with Texture Backup!");
        }

        private void CreateCleanFlower()
        {
            // Use the new 5-SENSOR FLOWER!
            GameObject flowerObj = new GameObject("FiveSensorFlower");
            flowerObj.AddComponent<FiveSensorFlower>();
            
            Debug.Log("🌸 Created 5-Sensor Flower - petals change color with sensors!");
        }
        
        private void CreateSimpleLeaf(Transform parent, Vector3 pos, float angle)
        {
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leaf.name = "Leaf";
            leaf.transform.SetParent(parent);
            leaf.transform.localPosition = pos;
            leaf.transform.localScale = new Vector3(0.25f, 0.06f, 0.12f);
            leaf.transform.localRotation = Quaternion.Euler(0, 0, angle);
            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0.2f, 0.55f, 0.25f);
            leaf.GetComponent<Renderer>().material = mat;
        }
        
        private void CreateSimplePetal(Transform parent, int index, int total, Color color)
        {
            GameObject petal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            petal.name = $"Petal{index}";
            petal.transform.SetParent(parent);
            
            float angle = (360f / total) * index;
            float radius = 0.8f;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            
            petal.transform.localPosition = new Vector3(x, 2.2f, z);
            petal.transform.localScale = new Vector3(0.6f, 0.25f, 0.4f);
            petal.transform.localRotation = Quaternion.Euler(0, -angle, 0);
            
            var mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color * 0.4f);
            petal.GetComponent<Renderer>().material = mat;
        }
        
        private void CreateSimpleButterfly()
        {
            // Simplified guide - just a cute floating butterfly
            GameObject butterfly = new GameObject("Friendly Guide");
            butterfly.transform.position = new Vector3(-2.5f, 2.5f, 0);
            
            // Body
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(butterfly.transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
            var bodyMat = new Material(Shader.Find("Standard"));
            bodyMat.color = new Color(0.5f, 0.3f, 0.7f);
            body.GetComponent<Renderer>().material = bodyMat;
            
            // Simple wings
            var wingMat = new Material(Shader.Find("Standard"));
            wingMat.color = new Color(0.9f, 0.6f, 0.9f);
            wingMat.EnableKeyword("_EMISSION");
            wingMat.SetColor("_EmissionColor", new Color(0.3f, 0.2f, 0.3f));
            
            GameObject leftWing = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leftWing.name = "LeftWing";
            leftWing.transform.SetParent(butterfly.transform);
            leftWing.transform.localPosition = new Vector3(-0.3f, 0.1f, 0);
            leftWing.transform.localScale = new Vector3(0.35f, 0.5f, 0.08f);
            leftWing.GetComponent<Renderer>().material = wingMat;
            
            GameObject rightWing = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rightWing.name = "RightWing";
            rightWing.transform.SetParent(butterfly.transform);
            rightWing.transform.localPosition = new Vector3(0.3f, 0.1f, 0);
            rightWing.transform.localScale = new Vector3(0.35f, 0.5f, 0.08f);
            rightWing.GetComponent<Renderer>().material = wingMat;
            
            butterfly.AddComponent<FloatingAnimation>();
        }
        
        private void CreateTargetOrb()
        {
            GameObject orb = new GameObject("Target Color Orb");
            orb.transform.position = new Vector3(-2.5f, 3f, 0);
            
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Orb";
            sphere.transform.SetParent(orb.transform);
            sphere.transform.localScale = Vector3.one * 0.5f;
            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(1f, 0.5f, 0.7f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", new Color(0.5f, 0.25f, 0.35f));
            sphere.GetComponent<Renderer>().material = mat;
            
            orb.AddComponent<FloatingAnimation>();
        }
        
        private void CreateArrow()
        {
            GameObject arrow = new GameObject("Visual Arrow");
            arrow.transform.position = new Vector3(-1f, 2.2f, 0);
            
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "ArrowBody";
            body.transform.SetParent(arrow.transform);
            body.transform.localScale = new Vector3(0.1f, 0.4f, 0.1f);
            body.transform.localRotation = Quaternion.Euler(0, 0, 90);
            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(1f, 0.85f, 0.2f);
            body.GetComponent<Renderer>().material = mat;
            
            arrow.AddComponent<PulsingAnimation>();
        }

        private void CreateGameGuideUI()
        {
            // Remove existing UIs
            var existingCanvas = GameObject.Find("VisualGuideCanvas");
            if (existingCanvas != null) DestroyImmediate(existingCanvas);
            
            var existingUI = GameObject.Find("FiveSensorUI");
            if (existingUI != null) DestroyImmediate(existingUI);
            
            // Create 5-SENSOR INPUT
            var existingSensor = FindObjectOfType<FiveSensorInput>();
            if (existingSensor == null)
            {
                GameObject sensorInput = new GameObject("FiveSensorInput");
                sensorInput.AddComponent<FiveSensorInput>();
                Debug.Log("✅ Created 5-Sensor Input Handler!");
            }
            
            // Create 5-SENSOR VISUAL UI (NO TEXT!)
            GameObject uiObj = new GameObject("FiveSensorVisualUI");
            uiObj.AddComponent<FiveSensorVisualUI>();
            
            Debug.Log("🎮 5-Sensor Game Ready!");
            Debug.Log("   Q/W/E/R/T = Bend sensors 1-5");
            Debug.Log("   SPACE = Confirm!");
        }
    }

    // Floating animation component
    public class FloatingAnimation : MonoBehaviour
    {
        public float floatSpeed = 1.5f;
        public float floatAmount = 0.15f;
        private Vector3 startPos;

        void Start() => startPos = transform.position;
        
        void Update()
        {
            float offset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = startPos + Vector3.up * offset;
        }
    }

    // Pulsing animation component
    public class PulsingAnimation : MonoBehaviour
    {
        public float pulseSpeed = 2f;
        private Vector3 startScale;

        void Start() => startScale = transform.localScale;
        
        void Update()
        {
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.15f;
            transform.localScale = startScale * pulse;
        }
    }
}
