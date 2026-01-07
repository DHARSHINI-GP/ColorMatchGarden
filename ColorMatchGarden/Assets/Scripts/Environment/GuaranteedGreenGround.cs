using UnityEngine;

namespace ColorMatchGarden.Environment
{
    /// <summary>
    /// GUARANTEED GREEN GROUND
    /// Creates a texture at runtime to force the ground to be green, regardless of shader.
    /// </summary>
    public class GuaranteedGreenGround : MonoBehaviour
    {
        private void Start()
        {
            ApplyTexture();
        }

        [ContextMenu("Apply Green Texture")]
        public void ApplyTexture()
        {
            Color grassColor = new Color(0.12f, 0.5f, 0.08f); // Rich Green
            
            // Create a 4x4 green texture
            Texture2D texture = new Texture2D(4, 4);
            Color[] pixels = new Color[16];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = grassColor;
            texture.SetPixels(pixels);
            texture.Apply();
            
            Renderer r = GetComponent<Renderer>();
            if (r != null)
            {
                // Create a standard material
                Material mat = new Material(Shader.Find("Standard"));
                mat.mainTexture = texture;
                mat.color = grassColor; // Backup
                mat.SetFloat("_Glossiness", 0f);
                mat.SetFloat("_Metallic", 0f);
                
                // Assign
                r.material = mat;
                Debug.Log("🌿 [GuaranteedGreenGround] Texture Applied!");
            }
        }

        [ContextMenu("Force Simple Green Color")]
        public void SetGreenColor()
        {
             Renderer r = GetComponent<Renderer>();
             if (r != null) 
             {
                 // Create simple material
                 Material mat = new Material(Shader.Find("Standard"));
                 mat.color = new Color(0.12f, 0.5f, 0.08f);
                 mat.SetFloat("_Glossiness", 0f);
                 r.material = mat;
                 Debug.Log("Forced Simple Green Color Manually!");
             }
        }
    }
}
