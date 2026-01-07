using UnityEngine;

namespace ColorMatchGarden.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Focus Target")]
        [SerializeField] private Transform flowerTarget;
        [SerializeField] private Transform guideTarget;
        
        [Header("Camera Movement")]
        [SerializeField] private float smoothSpeed = 2f;
        [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
        
        [Header("Breathing Effect")]
        [SerializeField] private float breatheAmount = 0.1f;
        [SerializeField] private float breatheSpeed = 0.5f;

        private Vector3 targetPosition;
        private Vector3 originalOffset;

        private void Start()
        {
            originalOffset = offset;
            
            if (flowerTarget == null)
            {
                var flower = FindObjectOfType<Flowers.InteractiveFlower>();
                if (flower != null) flowerTarget = flower.transform;
            }
            
            if (guideTarget == null)
            {
                var guide = FindObjectOfType<GuideCharacter>();
                if (guide != null) guideTarget = guide.transform;
            }
            
            PositionCamera();
        }

        private void LateUpdate()
        {
            ApplyBreathingEffect();
            SmoothFollow();
        }

        private void PositionCamera()
        {
            if (flowerTarget != null && guideTarget != null)
            {
                // Position between flower and guide
                Vector3 midpoint = (flowerTarget.position + guideTarget.position) / 2f;
                targetPosition = midpoint + offset;
            }
        }

        private void ApplyBreathingEffect()
        {
            float breathe = Mathf.Sin(Time.time * breatheSpeed) * breatheAmount;
            offset = originalOffset + new Vector3(0, breathe, 0);
            PositionCamera();
        }

        private void SmoothFollow()
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition, 
                Time.deltaTime * smoothSpeed
            );
            
            // Look at the scene center
            if (flowerTarget != null && guideTarget != null)
            {
                Vector3 lookTarget = (flowerTarget.position + guideTarget.position) / 2f;
                lookTarget.y += 1f;
                
                Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * smoothSpeed
                );
            }
        }

        public void FocusOnFlower()
        {
            if (flowerTarget != null)
            {
                targetPosition = flowerTarget.position + offset;
            }
        }

        public void FocusOnGuide()
        {
            if (guideTarget != null)
            {
                targetPosition = guideTarget.position + offset;
            }
        }
    }
}
