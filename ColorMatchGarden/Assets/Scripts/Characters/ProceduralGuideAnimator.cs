using UnityEngine;
using System.Collections;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Simple procedural animation for guide character.
    /// Use this when you don't have 3D models with animations.
    /// </summary>
    public class ProceduralGuideAnimator : MonoBehaviour
    {
        [Header("Body Parts")]
        [SerializeField] private Transform body;
        [SerializeField] private Transform head;
        [SerializeField] private Transform leftArm;
        [SerializeField] private Transform rightArm;
        [SerializeField] private Transform colorOrb;
        
        [Header("Idle Animation")]
        [SerializeField] private float breatheSpeed = 2f;
        [SerializeField] private float breatheAmount = 0.05f;
        [SerializeField] private float floatSpeed = 1f;
        [SerializeField] private float floatAmount = 0.1f;
        [SerializeField] private float swaySpeed = 0.5f;
        [SerializeField] private float swayAmount = 5f;

        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private bool isAnimating = false;
        private string currentAnimation = "idle";

        private void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        private void Update()
        {
            if (!isAnimating)
            {
                PlayIdleAnimation();
            }
        }

        private void PlayIdleAnimation()
        {
            // Breathing - scale body
            if (body != null)
            {
                float breathe = 1f + Mathf.Sin(Time.time * breatheSpeed) * breatheAmount;
                body.localScale = new Vector3(1, breathe, 1);
            }

            // Floating - vertical movement
            float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = originalPosition + Vector3.up * floatOffset;

            // Gentle sway - rotation
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            transform.rotation = originalRotation * Quaternion.Euler(0, sway, 0);

            // Head bob
            if (head != null)
            {
                float headBob = Mathf.Sin(Time.time * 1.5f) * 3f;
                head.localRotation = Quaternion.Euler(headBob, 0, 0);
            }

            // Orb glow pulsing
            if (colorOrb != null)
            {
                float pulse = 1f + Mathf.Sin(Time.time * 3f) * 0.1f;
                colorOrb.localScale = Vector3.one * 0.3f * pulse;
            }
        }

        public void PlayWave()
        {
            if (isAnimating) return;
            StartCoroutine(WaveRoutine());
        }

        private IEnumerator WaveRoutine()
        {
            isAnimating = true;
            float duration = 2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Wave arm
                if (rightArm != null)
                {
                    float waveAngle = Mathf.Sin(elapsed * 8f) * 30f;
                    float raiseAngle = Mathf.Sin(t * Mathf.PI) * 60f;
                    rightArm.localRotation = Quaternion.Euler(0, 0, raiseAngle + waveAngle);
                }

                // Lean toward child
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, Mathf.Sin(t * Mathf.PI) * 10f);

                yield return null;
            }

            if (rightArm != null)
                rightArm.localRotation = Quaternion.identity;
            transform.rotation = originalRotation;
            isAnimating = false;
        }

        public void PlayPresent()
        {
            if (isAnimating) return;
            StartCoroutine(PresentRoutine());
        }

        private IEnumerator PresentRoutine()
        {
            isAnimating = true;
            float duration = 1.5f;
            float elapsed = 0f;
            Vector3 originalOrbPos = colorOrb != null ? colorOrb.localPosition : Vector3.zero;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Extend arms forward
                if (leftArm != null)
                    leftArm.localRotation = Quaternion.Euler(Mathf.Sin(t * Mathf.PI) * -45f, 0, 0);
                if (rightArm != null)
                    rightArm.localRotation = Quaternion.Euler(Mathf.Sin(t * Mathf.PI) * -45f, 0, 0);

                // Orb moves forward and pulses
                if (colorOrb != null)
                {
                    float forward = Mathf.Sin(t * Mathf.PI) * 0.3f;
                    colorOrb.localPosition = originalOrbPos + Vector3.forward * forward;
                    
                    float pulse = 1f + Mathf.Sin(t * Mathf.PI) * 0.3f;
                    colorOrb.localScale = Vector3.one * 0.3f * pulse;
                }

                yield return null;
            }

            if (colorOrb != null)
            {
                colorOrb.localPosition = originalOrbPos;
                colorOrb.localScale = Vector3.one * 0.3f;
            }
            if (leftArm != null) leftArm.localRotation = Quaternion.identity;
            if (rightArm != null) rightArm.localRotation = Quaternion.identity;
            isAnimating = false;
        }

        public void PlayCelebrate()
        {
            if (isAnimating) return;
            StartCoroutine(CelebrateRoutine());
        }

        private IEnumerator CelebrateRoutine()
        {
            isAnimating = true;
            float duration = 2.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Happy jump
                float jump = Mathf.Sin(t * Mathf.PI * 2) * 0.5f;
                jump = Mathf.Max(0, jump);
                transform.position = originalPosition + Vector3.up * (floatAmount + jump);

                // Spin
                float spin = t * 360f;
                transform.rotation = originalRotation * Quaternion.Euler(0, spin, 0);

                // Arms up
                if (leftArm != null)
                    leftArm.localRotation = Quaternion.Euler(0, 0, -45 * Mathf.Sin(t * Mathf.PI));
                if (rightArm != null)
                    rightArm.localRotation = Quaternion.Euler(0, 0, 45 * Mathf.Sin(t * Mathf.PI));

                yield return null;
            }

            transform.position = originalPosition;
            transform.rotation = originalRotation;
            if (leftArm != null) leftArm.localRotation = Quaternion.identity;
            if (rightArm != null) rightArm.localRotation = Quaternion.identity;
            isAnimating = false;
        }

        public void PlayNod()
        {
            if (isAnimating) return;
            StartCoroutine(NodRoutine());
        }

        private IEnumerator NodRoutine()
        {
            isAnimating = true;
            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                if (head != null)
                {
                    float nod = Mathf.Sin(t * Mathf.PI) * 20f;
                    head.localRotation = Quaternion.Euler(nod, 0, 0);
                }

                // Slight body lean
                transform.rotation = originalRotation * Quaternion.Euler(Mathf.Sin(t * Mathf.PI) * 5f, 0, 0);

                yield return null;
            }

            if (head != null) head.localRotation = Quaternion.identity;
            transform.rotation = originalRotation;
            isAnimating = false;
        }
    }
}
