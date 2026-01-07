using UnityEngine;
using System.Collections;

namespace ColorMatchGarden.Flowers
{
    /// <summary>
    /// Big colorful flower that changes color based on 5 sensor input.
    /// Large, visible, and attractive for neurodiverse children.
    /// </summary>
    public class FiveSensorFlower : MonoBehaviour
    {
        [Header("Flower Parts")]
        private GameObject flowerCenter;
        private GameObject[] petals = new GameObject[8];
        private Light flowerGlow;
        private ParticleSystem celebrationParticles;
        
        [Header("Settings")]
        [SerializeField] private float petalSize = 1.2f;
        [SerializeField] private float bloomSpeed = 5f;
        
        private Color currentColor = new Color(1f, 0.4f, 0.7f);
        private Material[] petalMaterials;
        private Material centerMaterial;
        
        private Core.FiveSensorInput sensorInput;
        
        private void Start()
        {
            CreateFlower();
            
            // Subscribe to sensor input
            sensorInput = FindObjectOfType<Core.FiveSensorInput>();
            if (sensorInput != null)
            {
                sensorInput.OnColorChanged += OnColorChanged;
                // REMOVED OnConfirm subscription - Controlled by UI now!
                Debug.Log("🌸 Flower connected to 5-sensor input!");
            }
        }
        
        private void OnDestroy()
        {
            if (sensorInput != null)
            {
                sensorInput.OnColorChanged -= OnColorChanged;
                // sensorInput.OnConfirmGesture -= OnConfirm;
            }
        }
        
        private void CreateFlower()
        {
            // Position in center, visible to camera
            transform.position = new Vector3(0, 0.5f, 0);
            
            // THICK STEM
            GameObject stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.name = "Stem";
            stem.transform.SetParent(transform);
            stem.transform.localPosition = new Vector3(0, 1f, 0);
            stem.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
            
            Material stemMat = new Material(Shader.Find("Standard"));
            stemMat.color = new Color(0.1f, 0.5f, 0.1f);
            stem.GetComponent<Renderer>().material = stemMat;
            
            // BIG LEAVES
            CreateLeaf(new Vector3(-0.7f, 0.8f, 0), -40);
            CreateLeaf(new Vector3(0.7f, 0.6f, 0), 40);
            CreateLeaf(new Vector3(0, 0.5f, 0.6f), 30);
            
            // LARGE FLOWER CENTER
            flowerCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flowerCenter.name = "FlowerCenter";
            flowerCenter.transform.SetParent(transform);
            flowerCenter.transform.localPosition = new Vector3(0, 2.5f, 0);
            flowerCenter.transform.localScale = Vector3.one * 1.5f;
            
            centerMaterial = new Material(Shader.Find("Standard"));
            centerMaterial.color = new Color(1f, 0.9f, 0.3f);
            centerMaterial.EnableKeyword("_EMISSION");
            centerMaterial.SetColor("_EmissionColor", new Color(1f, 0.8f, 0.2f));
            flowerCenter.GetComponent<Renderer>().material = centerMaterial;
            
            // BIG COLORFUL PETALS
            petalMaterials = new Material[8];
            for (int i = 0; i < 8; i++)
            {
                petals[i] = CreatePetal(i);
            }
            
            // BRIGHT GLOW LIGHT
            GameObject glowObj = new GameObject("FlowerGlow");
            glowObj.transform.SetParent(transform);
            glowObj.transform.localPosition = new Vector3(0, 2.5f, 0);
            flowerGlow = glowObj.AddComponent<Light>();
            flowerGlow.type = LightType.Point;
            flowerGlow.color = currentColor;
            flowerGlow.intensity = 5f;
            flowerGlow.range = 10f;
            
            // Celebration particles
            CreateCelebrationParticles();
            
            Debug.Log("🌸 Created BIG 5-sensor flower!");
        }
        
        private void CreateLeaf(Vector3 pos, float angle)
        {
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leaf.name = "Leaf";
            leaf.transform.SetParent(transform);
            leaf.transform.localPosition = pos;
            leaf.transform.localScale = new Vector3(0.6f, 0.15f, 0.35f);
            leaf.transform.localRotation = Quaternion.Euler(0, 0, angle);
            
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0.1f, 0.55f, 0.1f);
            leaf.GetComponent<Renderer>().material = mat;
        }
        
        private GameObject CreatePetal(int index)
        {
            GameObject petal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            petal.name = $"Petal{index}";
            petal.transform.SetParent(transform);
            
            float angle = (360f / 8) * index;
            float radius = 1.5f;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            
            petal.transform.localPosition = new Vector3(x, 2.5f, z);
            petal.transform.localScale = new Vector3(petalSize, 0.4f, 0.7f);
            petal.transform.localRotation = Quaternion.Euler(0, -angle, 0);
            
            petalMaterials[index] = new Material(Shader.Find("Standard"));
            petalMaterials[index].color = currentColor;
            petalMaterials[index].EnableKeyword("_EMISSION");
            petalMaterials[index].SetColor("_EmissionColor", currentColor * 0.7f);
            petal.GetComponent<Renderer>().material = petalMaterials[index];
            
            return petal;
        }
        
        private void CreateCelebrationParticles()
        {
            GameObject particleObj = new GameObject("CelebrationParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 2.5f, 0);
            
            celebrationParticles = particleObj.AddComponent<ParticleSystem>();
            var main = celebrationParticles.main;
            main.startLifetime = 2f;
            main.startSpeed = 5f;
            main.startSize = 0.3f;
            main.startColor = Color.yellow;
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = celebrationParticles.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 50) });
            
            var shape = celebrationParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 1f;
            
            celebrationParticles.Stop();
        }
        
        private void OnColorChanged(Color newColor)
        {
            currentColor = newColor;
            UpdateFlowerColor();
        }
        
        private void UpdateFlowerColor()
        {
            // Update all petal colors with smooth transition
            for (int i = 0; i < petalMaterials.Length; i++)
            {
                if (petalMaterials[i] != null)
                {
                    petalMaterials[i].color = currentColor;
                    petalMaterials[i].SetColor("_EmissionColor", currentColor * 0.8f);
                }
            }
            
            // Update glow
            if (flowerGlow != null)
            {
                flowerGlow.color = currentColor;
                flowerGlow.intensity = 4f + (currentColor.r + currentColor.g + currentColor.b);
            }
        }
        
        public void PlayCelebration()
        {
            StartCoroutine(CelebrationAnimation());
        }
        
        private IEnumerator CelebrationAnimation()
        {
            // Play particles
            if (celebrationParticles != null)
            {
                var main = celebrationParticles.main;
                main.startColor = currentColor;
                celebrationParticles.Play();
            }
            
            // Bloom animation - petals grow!
            float duration = 1f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float scale = 1f + Mathf.Sin(t * Mathf.PI) * 0.5f;
                
                for (int i = 0; i < petals.Length; i++)
                {
                    if (petals[i] != null)
                    {
                        petals[i].transform.localScale = new Vector3(
                            petalSize * scale,
                            0.4f * scale,
                            0.7f * scale
                        );
                    }
                }
                
                // Pulse glow
                if (flowerGlow != null)
                {
                    flowerGlow.intensity = 5f + Mathf.Sin(t * Mathf.PI * 4) * 3f;
                }
                
                yield return null;
            }
            
            // Reset petal size
            for (int i = 0; i < petals.Length; i++)
            {
                if (petals[i] != null)
                {
                    petals[i].transform.localScale = new Vector3(petalSize, 0.4f, 0.7f);
                }
            }
        }

        private void Update()
        {
            // Gentle floating animation
            float bob = Mathf.Sin(Time.time * 1.5f) * 0.1f;
            transform.position = new Vector3(0, 0.5f + bob, 0);
            
            // Gentle rotation
            transform.Rotate(0, Time.deltaTime * 5f, 0);
        }
    }
}
