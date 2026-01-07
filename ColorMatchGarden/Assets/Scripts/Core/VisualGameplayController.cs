using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Visual gameplay controller that makes the game flow clear and understandable.
    /// Uses only visual feedback - no text!
    /// </summary>
    public class VisualGameplayController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Transform guideCharacter;
        [SerializeField] private Transform targetOrb;
        [SerializeField] private Transform magicFlower;
        [SerializeField] private Light orbLight;
        [SerializeField] private Light flowerLight;

        [Header("Colors")]
        [SerializeField] private Color[] availableColors = new Color[]
        {
            new Color(1f, 0.6f, 0.8f),      // Soft Pink
            new Color(0.6f, 0.8f, 1f),      // Gentle Blue
            new Color(1f, 0.9f, 0.5f),      // Warm Yellow
            new Color(0.6f, 1f, 0.7f),      // Calm Green
            new Color(0.85f, 0.7f, 1f),     // Lavender
            new Color(1f, 0.75f, 0.5f)      // Peach
        };

        [Header("Visual Feedback")]
        [SerializeField] private ParticleSystem celebrationParticles;
        [SerializeField] private ParticleSystem matchingParticles;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip celebrationSound;
        [SerializeField] private AudioClip colorChangeSound;

        private Color currentTargetColor;
        private Color currentFlowerColor;
        private float currentBrightness = 0.5f;
        private int colorIndex = 0;
        private bool isPlaying = true;

        private Material orbMaterial;
        private Material[] petalMaterials;
        private ColorController flowerChanger;

        private void Start()
        {
            StartCoroutine(InitializeGame());
        }

        private IEnumerator InitializeGame()
        {
            yield return new WaitForSeconds(1f);
            
            // Find objects if not assigned
            FindGameObjects();
            
            // Start with first color
            yield return StartCoroutine(ShowNewTargetColor());
        }

        private void FindGameObjects()
        {
            if (guideCharacter == null)
                guideCharacter = GameObject.Find("Friendly Guide")?.transform;
            
            if (targetOrb == null)
                targetOrb = GameObject.Find("Target Color Orb")?.transform;
            
            if (magicFlower == null)
                magicFlower = GameObject.Find("Magic Flower")?.transform;
            
            if (targetOrb != null)
            {
                var inner = targetOrb.Find("Target Color");
                if (inner != null)
                    orbMaterial = inner.GetComponent<Renderer>()?.material;
                orbLight = targetOrb.GetComponent<Light>();
            }

            if (magicFlower != null)
            {
                flowerChanger = magicFlower.GetComponent<ColorController>();
                flowerLight = magicFlower.GetComponentInChildren<Light>();
            }
        }

        private void Update()
        {
            if (!isPlaying) return;

            // Handle keyboard input for testing
            HandleTestInput();
        }

        private void HandleTestInput()
        {
            // Arrow keys to change brightness
            if (Input.GetKey(KeyCode.UpArrow))
            {
                SetFlowerBrightness(currentBrightness + Time.deltaTime * 0.5f);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                SetFlowerBrightness(currentBrightness - Time.deltaTime * 0.5f);
            }

            // Number keys for quick brightness
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetFlowerBrightness(0.2f);  // Light
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetFlowerBrightness(0.5f);  // Medium
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetFlowerBrightness(0.9f);  // Bright

            // Space to confirm
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Celebrate());
            }

            // R to reset
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetFlower();
            }
        }

        private IEnumerator ShowNewTargetColor()
        {
            // Pick next color
            currentTargetColor = availableColors[colorIndex % availableColors.Length];
            colorIndex++;

            // Animate the orb
            yield return StartCoroutine(AnimateOrbColorChange(currentTargetColor));

            // Make guide character "present" the color
            if (guideCharacter != null)
            {
                yield return StartCoroutine(GuidePresents());
            }
        }

        private IEnumerator AnimateOrbColorChange(Color newColor)
        {
            if (orbMaterial == null) yield break;

            Color startColor = orbMaterial.color;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = t * t * (3f - 2f * t); // Smoothstep

                Color lerpedColor = Color.Lerp(startColor, newColor, t);
                orbMaterial.color = lerpedColor;
                orbMaterial.SetColor("_EmissionColor", lerpedColor * 0.5f);
                
                if (orbLight != null)
                    orbLight.color = lerpedColor;

                yield return null;
            }

            orbMaterial.color = newColor;
        }

        private IEnumerator GuidePresents()
        {
            if (guideCharacter == null) yield break;

            // Simple bounce animation
            Vector3 startPos = guideCharacter.position;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float bounce = Mathf.Sin(t * Mathf.PI) * 0.3f;
                guideCharacter.position = startPos + Vector3.up * bounce;
                yield return null;
            }

            guideCharacter.position = startPos;
        }

        public void SetFlowerBrightness(float brightness)
        {
            currentBrightness = Mathf.Clamp01(brightness);
            
            // Apply to flower
            Color flowerColor = currentTargetColor * currentBrightness;
            currentFlowerColor = flowerColor;
            
            if (flowerChanger != null)
            {
                flowerChanger.SetColor(currentTargetColor);
                flowerChanger.SetBrightness(currentBrightness);
            }

            // Update flower light
            if (flowerLight != null)
            {
                flowerLight.color = flowerColor;
                flowerLight.intensity = currentBrightness * 2f;
            }

            // Emit particles on change
            if (matchingParticles != null)
            {
                var main = matchingParticles.main;
                main.startColor = flowerColor;
                matchingParticles.Emit(2);
            }
        }

        /// <summary>
        /// Called when child confirms their color choice.
        /// Always celebrates - no wrong answers!
        /// </summary>
        public IEnumerator Celebrate()
        {
            isPlaying = false;

            // Play celebration particles
            if (celebrationParticles != null)
            {
                var main = celebrationParticles.main;
                main.startColor = currentFlowerColor;
                celebrationParticles.Play();
            }

            // Play sound
            if (audioSource != null && celebrationSound != null)
            {
                audioSource.PlayOneShot(celebrationSound);
            }

            // Animate guide doing a happy dance
            yield return StartCoroutine(GuideHappyDance());

            // Wait
            yield return new WaitForSeconds(2f);

            // Stop particles
            if (celebrationParticles != null)
                celebrationParticles.Stop();

            isPlaying = true;

            // Show next color
            yield return StartCoroutine(ShowNewTargetColor());
        }

        private IEnumerator GuideHappyDance()
        {
            if (guideCharacter == null) yield break;

            Vector3 startPos = guideCharacter.position;
            Quaternion startRot = guideCharacter.rotation;
            float duration = 2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Jump
                float jump = Mathf.Sin(t * Mathf.PI * 4) * 0.3f;
                jump = Mathf.Max(0, jump);

                // Spin
                float spin = t * 360f;

                guideCharacter.position = startPos + Vector3.up * jump;
                guideCharacter.rotation = startRot * Quaternion.Euler(0, spin, 0);

                yield return null;
            }

            guideCharacter.position = startPos;
            guideCharacter.rotation = startRot;
        }

        public void ResetFlower()
        {
            SetFlowerBrightness(0.5f);
            
            // Gentle reset animation
            if (guideCharacter != null)
            {
                StartCoroutine(GuideNod());
            }
        }

        private IEnumerator GuideNod()
        {
            if (guideCharacter == null) yield break;

            Quaternion startRot = guideCharacter.rotation;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float nod = Mathf.Sin(t * Mathf.PI) * 15f;
                guideCharacter.rotation = startRot * Quaternion.Euler(nod, 0, 0);
                yield return null;
            }

            guideCharacter.rotation = startRot;
        }

        // Called by FlexSensorInput
        public void OnFlexSensorUpdate(float value)
        {
            SetFlowerBrightness(value);
        }

        // Called by GestureRecognizer
        public void OnConfirmGesture()
        {
            if (isPlaying)
                StartCoroutine(Celebrate());
        }

        public void OnResetGesture()
        {
            ResetFlower();
        }
    }
}
