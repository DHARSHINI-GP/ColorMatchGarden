using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGarden.Core
{
    public class WebcamHandler : MonoBehaviour
    {
        [SerializeField] private RawImage displayImage;
        [SerializeField] private bool mirrorHorizontally = true;
        [SerializeField] private int requestedWidth = 640;
        [SerializeField] private int requestedHeight = 480;

        private WebCamTexture webcamTexture;
        private bool isInitialized = false;

        private void Start()
        {
            InitializeWebcam();
        }

        private void InitializeWebcam()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0) return;

            string cameraName = devices[0].name;
            foreach (var device in devices)
            {
                if (device.isFrontFacing) { cameraName = device.name; break; }
            }

            webcamTexture = new WebCamTexture(cameraName, requestedWidth, requestedHeight, 30);
            if (displayImage != null)
            {
                displayImage.texture = webcamTexture;
                if (mirrorHorizontally)
                    displayImage.rectTransform.localScale = new Vector3(-1, 1, 1);
            }
            webcamTexture.Play();
            isInitialized = true;
        }

        private void OnDestroy()
        {
            if (webcamTexture != null) webcamTexture.Stop();
        }

        public bool IsActive() => isInitialized && webcamTexture != null && webcamTexture.isPlaying;
    }
}
