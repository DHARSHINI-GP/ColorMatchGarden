using UnityEngine;
using System.Collections;

namespace ColorMatchGarden.Core
{
    public class GuideCharacter : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;
        
        [Header("Color Display")]
        [SerializeField] private Renderer colorOrbRenderer;
        [SerializeField] private Light colorLight;
        
        [Header("Animation Triggers")]
        private static readonly int WaveTrigger = Animator.StringToHash("Wave");
        private static readonly int PresentTrigger = Animator.StringToHash("Present");
        private static readonly int CelebrateTrigger = Animator.StringToHash("Celebrate");
        private static readonly int NodTrigger = Animator.StringToHash("Nod");
        private static readonly int PointTrigger = Animator.StringToHash("Point");

        private Material orbMaterial;
        private Color currentDisplayColor;

        private void Start()
        {
            if (colorOrbRenderer != null)
                orbMaterial = colorOrbRenderer.material;
        }

        public void PlayWaveAnimation()
        {
            animator?.SetTrigger(WaveTrigger);
        }

        public void PlayPresentAnimation(Color targetColor)
        {
            animator?.SetTrigger(PresentTrigger);
            StartCoroutine(ShowTargetColor(targetColor));
        }

        public void PlayCelebrateAnimation()
        {
            animator?.SetTrigger(CelebrateTrigger);
        }

        public void PlayGentleNod()
        {
            animator?.SetTrigger(NodTrigger);
        }

        public void PlayPointAnimation()
        {
            animator?.SetTrigger(PointTrigger);
        }

        private IEnumerator ShowTargetColor(Color color)
        {
            float duration = 0.5f;
            float elapsed = 0f;
            Color startColor = currentDisplayColor;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                Color lerpedColor = Color.Lerp(startColor, color, t);
                
                if (orbMaterial != null)
                {
                    orbMaterial.color = lerpedColor;
                    orbMaterial.SetColor("_EmissionColor", lerpedColor * 0.5f);
                }
                if (colorLight != null)
                {
                    colorLight.color = lerpedColor;
                }
                
                yield return null;
            }

            currentDisplayColor = color;
        }
    }
}
