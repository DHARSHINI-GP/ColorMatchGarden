#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ColorMatchGarden.Editor
{
    public class GardenSceneSetup : EditorWindow
    {
        [MenuItem("Color Match Garden/Setup Scene")]
        public static void SetupScene()
        {
            if (EditorUtility.DisplayDialog("Setup Garden Scene",
                "This will create all required game objects for Color Match Garden. Continue?",
                "Yes", "Cancel"))
            {
                CreateGameManager();
                CreateEnvironment();
                CreateGuideCharacter();
                CreateInteractiveFlower();
                CreateParticleController();
                CreateAudioManager();
                CreateUICanvas();
                
                Debug.Log("🌸 Color Match Garden scene setup complete!");
            }
        }

        private static void CreateGameManager()
        {
            var go = new GameObject("[GameManager]");
            go.AddComponent<Core.GameManager>();
            go.AddComponent<Core.FiveSensorInput>();
            go.AddComponent<Core.GestureRecognizer>();
            go.AddComponent<Core.AccessibilityManager>();
        }

        private static void CreateEnvironment()
        {
            var env = new GameObject("[Environment]");
            env.AddComponent<Environment.GardenEnvironment>();
            
            // Ground
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetParent(env.transform);
            ground.transform.localScale = new Vector3(3, 1, 3);
            
            // Light
            var light = new GameObject("Main Light");
            var lightComp = light.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            lightComp.color = new Color(1f, 0.95f, 0.9f);
            lightComp.intensity = 1.2f;
            lightComp.shadows = LightShadows.Soft;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
        }

        private static void CreateGuideCharacter()
        {
            var guide = new GameObject("[Guide Character]");
            guide.AddComponent<Core.GuideCharacter>();
            
            // Body
            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(guide.transform);
            body.transform.localScale = new Vector3(0.4f, 0.5f, 0.4f);
            body.transform.localPosition = new Vector3(-2, 1, 0);
            
            // Color Orb
            var orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.name = "ColorOrb";
            orb.transform.SetParent(guide.transform);
            orb.transform.localScale = Vector3.one * 0.3f;
            orb.transform.localPosition = new Vector3(-2, 1.8f, 0);
            
            var orbLight = orb.AddComponent<Light>();
            orbLight.type = LightType.Point;
            orbLight.range = 2;
            orbLight.intensity = 1;
            
            guide.transform.position = new Vector3(-2, 0, 0);
        }

        private static void CreateInteractiveFlower()
        {
            var flower = new GameObject("[Interactive Flower]");
            flower.AddComponent<Flowers.InteractiveFlower>();
            flower.AddComponent<Core.ColorController>();
            
            // Stem
            var stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.name = "Stem";
            stem.transform.SetParent(flower.transform);
            stem.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
            stem.transform.localPosition = new Vector3(0, 0.5f, 0);
            var stemMat = new Material(Shader.Find("Standard"));
            stemMat.color = new Color(0.3f, 0.6f, 0.2f);
            stem.GetComponent<Renderer>().material = stemMat;
            
            // Center
            var center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "Center";
            center.transform.SetParent(flower.transform);
            center.transform.localScale = Vector3.one * 0.2f;
            center.transform.localPosition = new Vector3(0, 1.2f, 0);
            var centerMat = new Material(Shader.Find("Standard"));
            centerMat.color = new Color(1f, 0.9f, 0.4f);
            center.GetComponent<Renderer>().material = centerMat;
            
            // Petals
            for (int i = 0; i < 6; i++)
            {
                var petal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                petal.name = $"Petal_{i}";
                petal.transform.SetParent(flower.transform);
                petal.transform.localScale = new Vector3(0.25f, 0.05f, 0.12f);
                float angle = i * 60 * Mathf.Deg2Rad;
                petal.transform.localPosition = new Vector3(
                    Mathf.Cos(angle) * 0.25f,
                    1.2f,
                    Mathf.Sin(angle) * 0.25f
                );
                petal.transform.localRotation = Quaternion.Euler(0, -i * 60, 0);
                
                var petalMat = new Material(Shader.Find("Standard"));
                petalMat.color = Color.white;
                petalMat.EnableKeyword("_EMISSION");
                petal.GetComponent<Renderer>().material = petalMat;
            }
            
            // Glow light
            var glow = new GameObject("Glow");
            var glowLight = glow.AddComponent<Light>();
            glowLight.type = LightType.Point;
            glowLight.range = 3;
            glowLight.intensity = 1;
            glowLight.color = Color.white;
            glow.transform.SetParent(flower.transform);
            glow.transform.localPosition = new Vector3(0, 1.2f, 0);
            
            flower.transform.position = new Vector3(2, 0, 0);
        }

        private static void CreateParticleController()
        {
            var particles = new GameObject("[Particles]");
            particles.AddComponent<Core.ParticleController>();
            
            // Celebration particles
            var celebration = new GameObject("CelebrationParticles");
            celebration.transform.SetParent(particles.transform);
            var ps = celebration.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 2;
            main.startSpeed = 3;
            main.startSize = 0.1f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            ps.Stop();
        }

        private static void CreateAudioManager()
        {
            var audio = new GameObject("[Audio]");
            audio.AddComponent<Core.SoundManager>();
            
            var ambient = audio.AddComponent<AudioSource>();
            ambient.loop = true;
            ambient.playOnAwake = false;
            ambient.volume = 0.3f;
            
            var feedback = audio.AddComponent<AudioSource>();
            feedback.playOnAwake = false;
            feedback.volume = 0.5f;
        }

        private static void CreateUICanvas()
        {
            var canvas = new GameObject("[UI Canvas]");
            var canvasComp = canvas.AddComponent<Canvas>();
            canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            // Event System
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }
    }
}
#endif
