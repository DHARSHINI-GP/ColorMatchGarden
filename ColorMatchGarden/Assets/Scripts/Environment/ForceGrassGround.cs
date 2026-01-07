using UnityEngine;

namespace ColorMatchGarden.Environment
{
    /// <summary>
    /// FORCE the ground to be grass green!
    /// Attach this to any ground object to ensure it's always green.
    /// </summary>
    [ExecuteAlways]
    public class ForceGrassGround : MonoBehaviour
    {
        [Header("Grass Color")]
        [SerializeField] private Color grassColor = new Color(0.12f, 0.5f, 0.08f); // Rich grass green
        
        [Header("Debug")]
        [SerializeField] private bool applyOnStart = true;
        [SerializeField] private bool applyEveryFrame = false;
        
        private Renderer groundRenderer;
        private Material grassMaterial;
        
        private void Start()
        {
            if (applyOnStart)
            {
                ApplyGrassColor();
            }
        }
        
        private void Update()
        {
            if (applyEveryFrame)
            {
                ApplyGrassColor();
            }
        }
        
        [ContextMenu("Apply Grass Color NOW")]
        public void ApplyGrassColor()
        {
            groundRenderer = GetComponent<Renderer>();
            if (groundRenderer == null)
            {
                Debug.LogWarning("[ForceGrassGround] No Renderer found!");
                return;
            }
            
            // Create new material to avoid modifying shared material
            if (grassMaterial == null)
            {
                grassMaterial = new Material(Shader.Find("Standard"));
            }
            
            // Set properties
            grassMaterial.color = grassColor;
            grassMaterial.SetFloat("_Glossiness", 0f); // Matte
            grassMaterial.SetFloat("_Metallic", 0f);   // Not metallic
            grassMaterial.DisableKeyword("_EMISSION"); // No glow
            
            // Apply
            groundRenderer.sharedMaterial = grassMaterial;
            
            Debug.Log($"✅ [ForceGrassGround] Applied grass color: {grassColor}");
        }
        
        private void OnValidate()
        {
            // Auto-apply when color changes in editor
            if (Application.isPlaying == false && groundRenderer != null)
            {
                ApplyGrassColor();
            }
        }
    }
}
