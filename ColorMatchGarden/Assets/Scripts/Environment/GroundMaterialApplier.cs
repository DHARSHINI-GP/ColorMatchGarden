using UnityEngine;

namespace ColorMatchGarden.Environment
{
    /// <summary>
    /// Ensures the ground material is properly applied at runtime.
    /// Attach this to your Ground plane.
    /// </summary>
    public class GroundMaterialApplier : MonoBehaviour
    {
        [Header("Ground Material")]
        [Tooltip("Drag your GrassMaterial here")]
        [SerializeField] private Material grassMaterial;
        
        [Header("Fallback Color (if no material)")]
        [SerializeField] private Color grassColor = new Color(0.18f, 0.49f, 0.2f); // Dark grass green #2E7D32
        
        private void Awake()
        {
            ApplyGrassMaterial();
        }
        
        private void Start()
        {
            // Double-check in Start as well
            ApplyGrassMaterial();
        }
        
        private void ApplyGrassMaterial()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogWarning("[GroundMaterialApplier] No Renderer found on this object!");
                return;
            }
            
            // ALWAYS force a rich grass green color
            Color targetGreen = new Color(0.12f, 0.5f, 0.08f); // Rich grass green
            
            if (grassMaterial != null)
            {
                // Apply the assigned material BUT override its color
                renderer.material = grassMaterial;
                renderer.material.color = targetGreen;
                
                // Remove shininess
                if (renderer.material.HasProperty("_Glossiness")) renderer.material.SetFloat("_Glossiness", 0f);
                if (renderer.material.HasProperty("_Smoothness")) renderer.material.SetFloat("_Smoothness", 0f);
                
                Debug.Log("[GroundMaterialApplier] Grass material applied with FORCED GREEN color!");
            }
            else
            {
                // Create a simple green material if none assigned
                Material simpleMat = new Material(Shader.Find("Standard"));
                simpleMat.color = targetGreen;
                simpleMat.SetFloat("_Glossiness", 0f);
                renderer.material = simpleMat;
                Debug.Log("[GroundMaterialApplier] Created fallback bright green material.");
            }
        }
        
        // Allow re-applying from Inspector context menu
        [ContextMenu("Apply Grass Material")]
        public void ForceApplyMaterial()
        {
            ApplyGrassMaterial();
        }
        
        [ContextMenu("Force Green Color")]
        public void SetGreenColor()
        {
             Renderer r = GetComponent<Renderer>();
             if (r != null) 
             {
                 r.material.color = new Color(0.12f, 0.5f, 0.08f);
                 Debug.Log("Forced Green Color Manually!");
             }
        }
    }
}
