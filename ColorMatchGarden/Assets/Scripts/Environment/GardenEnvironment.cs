using UnityEngine;

namespace ColorMatchGarden.Environment
{
    #pragma warning disable 0649
    /// <summary>
    /// Optional garden environment manager.
    /// All fields are optional - leave empty if not using.
    /// </summary>
    public class GardenEnvironment : MonoBehaviour
    {
        [Header("Lighting (Optional)")]
        [SerializeField] private Light mainLight = null;
        [SerializeField] private Light[] decorativeLights = null;
        [SerializeField] private Color dayColor = new Color(1f, 0.95f, 0.8f);
        [SerializeField] private float lightIntensity = 1.5f;
        
        [Header("Ambient Particles (Optional)")]
        [Tooltip("Leave empty if not using")]
        [SerializeField] private ParticleSystem butterflies = null;
        [Tooltip("Leave empty if not using")]
        [SerializeField] private ParticleSystem floatingPetals = null;
        [Tooltip("Leave empty if not using")]
        [SerializeField] private ParticleSystem sparkles = null;
        
        [Header("Swaying Animation (Optional)")]
        [SerializeField] private float swaySpeed = 0.5f;
        [SerializeField] private float swayAmount = 0.02f;
        [SerializeField] private Transform[] swayingPlants = null;

        private void Start()
        {
            SetupLighting();
            StartAmbientEffects();
        }

        private void SetupLighting()
        {
            if (mainLight != null)
            {
                mainLight.color = dayColor;
                mainLight.intensity = lightIntensity;
            }
            
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.6f, 0.65f, 0.7f);
        }

        private void StartAmbientEffects()
        {
            // Only play if assigned
            if (butterflies != null) butterflies.Play();
            if (floatingPetals != null) floatingPetals.Play();
            if (sparkles != null) sparkles.Play();
        }

        private void Update()
        {
            AnimateSwayingPlants();
        }

        private void AnimateSwayingPlants()
        {
            if (swayingPlants == null || swayingPlants.Length == 0) return;
            
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            
            foreach (var plant in swayingPlants)
            {
                if (plant != null)
                {
                    plant.localRotation = Quaternion.Euler(sway * 10f, 0, sway * 5f);
                }
            }
        }

        public void SetBrightnessBoost(float boost)
        {
            if (mainLight != null)
            {
                mainLight.intensity = lightIntensity + boost;
            }
        }
    }
    #pragma warning restore 0649
}
