using UnityEngine;
using System.Collections;

namespace ColorMatchGarden.Core
{
    public class ParticleController : MonoBehaviour
    {
        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem celebrationParticles;
        [SerializeField] private ParticleSystem ambientParticles;
        [SerializeField] private ParticleSystem colorChangeParticles;
        [SerializeField] private ParticleSystem confirmGlowParticles;
        
        [Header("Settings")]
        [SerializeField] private float celebrationDuration = 3f;

        public void PlayCelebrationParticles(Color color)
        {
            if (celebrationParticles == null) return;
            
            var main = celebrationParticles.main;
            main.startColor = color;
            celebrationParticles.Play();
            
            if (confirmGlowParticles != null)
            {
                var glowMain = confirmGlowParticles.main;
                glowMain.startColor = new Color(color.r, color.g, color.b, 0.5f);
                confirmGlowParticles.Play();
            }
        }

        public void FadeOutParticles()
        {
            StartCoroutine(FadeParticlesRoutine());
        }

        private IEnumerator FadeParticlesRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            celebrationParticles?.Stop();
            confirmGlowParticles?.Stop();
        }

        public void PlayColorChangeEffect(Color color)
        {
            if (colorChangeParticles == null) return;
            var main = colorChangeParticles.main;
            main.startColor = color;
            colorChangeParticles.Emit(10);
        }

        public void StartAmbientParticles()
        {
            ambientParticles?.Play();
        }

        public void StopAmbientParticles()
        {
            ambientParticles?.Stop();
        }
    }
}
