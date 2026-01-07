using UnityEngine;

namespace ColorMatchGarden.Core
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource feedbackSource;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip ambientGarden;
        [SerializeField] private AudioClip softChime;
        [SerializeField] private AudioClip softNote;
        [SerializeField] private AudioClip happyChime;
        [SerializeField] private AudioClip softWhoosh;
        [SerializeField] private AudioClip gentleClick;
        
        [Header("Volume Settings")]
        [SerializeField] private float ambientVolume = 0.3f;
        [SerializeField] private float feedbackVolume = 0.5f;

        private void Start()
        {
            ApplyAccessibilitySettings();
        }

        private void ApplyAccessibilitySettings()
        {
            if (AccessibilityManager.Instance != null)
            {
                var settings = AccessibilityManager.Instance.GetCurrentSettings();
                ambientVolume = settings.AmbientVolume;
                feedbackVolume = settings.FeedbackVolume;
            }
            
            if (ambientSource != null)
                ambientSource.volume = ambientVolume;
        }

        public void PlayAmbient()
        {
            if (ambientSource != null && ambientGarden != null)
            {
                ambientSource.clip = ambientGarden;
                ambientSource.loop = true;
                ambientSource.volume = ambientVolume;
                ambientSource.Play();
            }
        }

        public void PlaySoftChime()
        {
            PlayFeedback(softChime);
        }

        public void PlaySoftNote()
        {
            PlayFeedback(softNote);
        }

        public void PlayHappyChime()
        {
            PlayFeedback(happyChime);
        }

        public void PlaySoftWhoosh()
        {
            PlayFeedback(softWhoosh);
        }

        public void PlayGentleClick()
        {
            PlayFeedback(gentleClick);
        }

        private void PlayFeedback(AudioClip clip)
        {
            if (feedbackSource != null && clip != null)
            {
                feedbackSource.PlayOneShot(clip, feedbackVolume);
            }
        }

        public void StopAmbient()
        {
            ambientSource?.Stop();
        }

        public void SetAmbientVolume(float volume)
        {
            ambientVolume = volume;
            if (ambientSource != null)
                ambientSource.volume = volume;
        }

        public void SetFeedbackVolume(float volume)
        {
            feedbackVolume = volume;
        }
    }
}
