using UnityEngine;

namespace ColorMatchGarden.Flowers
{
    public class InteractiveFlower : MonoBehaviour
    {
        [Header("Flower Parts")]
        [SerializeField] private Renderer petalRenderer;
        [SerializeField] private Renderer centerRenderer;
        [SerializeField] private Light flowerGlow;
        
        [Header("Animation")]
        [SerializeField] private float breatheSpeed = 1f;
        [SerializeField] private float breatheAmount = 0.05f;
        [SerializeField] private float rotateSpeed = 5f;
        
        [Header("Effects")]
        [SerializeField] private ParticleSystem pollenParticles;
        [SerializeField] private ParticleSystem sparkleParticles;

        private Material petalMaterial;
        private Vector3 originalScale;
        private Color currentColor;
        private float currentBrightness = 0.5f;

        private void Start()
        {
            if (petalRenderer != null)
                petalMaterial = petalRenderer.material;
            originalScale = transform.localScale;
        }

        private void Update()
        {
            AnimateBreathe();
            AnimateGentleRotation();
        }

        private void AnimateBreathe()
        {
            float breathe = 1f + Mathf.Sin(Time.time * breatheSpeed) * breatheAmount;
            transform.localScale = originalScale * breathe;
        }

        private void AnimateGentleRotation()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
        }

        public void SetColor(Color color)
        {
            currentColor = color;
            
            if (petalMaterial != null)
            {
                petalMaterial.color = color;
                petalMaterial.SetColor("_EmissionColor", color * 0.3f);
            }
            
            if (flowerGlow != null)
            {
                flowerGlow.color = color;
            }
            
            if (pollenParticles != null)
            {
                var main = pollenParticles.main;
                main.startColor = new Color(color.r, color.g, color.b, 0.5f);
            }
        }

        public void SetBrightness(float brightness)
        {
            currentBrightness = brightness;
            
            Color adjustedColor = currentColor * brightness;
            
            if (petalMaterial != null)
            {
                petalMaterial.color = adjustedColor;
            }
            
            if (flowerGlow != null)
            {
                flowerGlow.intensity = brightness * 2f;
            }
        }

        public void PlayCelebration()
        {
            sparkleParticles?.Play();
            pollenParticles?.Emit(30);
        }

        public void PlayGentleReset()
        {
            pollenParticles?.Emit(5);
        }
    }
}
