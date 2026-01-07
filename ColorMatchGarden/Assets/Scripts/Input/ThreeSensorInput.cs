using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ColorMatchGarden.UI;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Handles 3 flex sensors for RGB color mixing.
    /// Sensor 1 = RED, Sensor 2 = GREEN, Sensor 3 = BLUE
    /// </summary>
    public class ThreeSensorInput : MonoBehaviour
    {
        [Header("Network Settings")]
        [SerializeField] private int udpPort = 5005;
        [SerializeField] private bool autoConnect = true;
        
        [Header("Sensor Values (0-1)")]
        [SerializeField] [Range(0, 1)] private float redSensorValue = 0f;
        [SerializeField] [Range(0, 1)] private float greenSensorValue = 0f;
        [SerializeField] [Range(0, 1)] private float blueSensorValue = 0f;
        
        [Header("Smoothing")]
        [SerializeField] private float smoothSpeed = 5f;
        
        [Header("Test Mode (No Hardware)")]
        [SerializeField] private bool testMode = false;
        
        [Header("References")]
        [SerializeField] private VisualOnlyGuideUI visualGuide;
        
        // UDP Connection
        private UdpClient udpClient;
        private Thread receiveThread;
        private bool isRunning = false;
        
        // Target values (for smoothing)
        private float targetRed = 0f;
        private float targetGreen = 0f;
        private float targetBlue = 0f;
        
        // Events
        public event Action<float, float, float> OnSensorValuesChanged;
        public event Action OnConfirmGesture;
        
        private void Start()
        {
            // Find visual guide if not assigned
            if (visualGuide == null)
            {
                visualGuide = FindObjectOfType<VisualOnlyGuideUI>();
            }
            
            if (autoConnect && !testMode)
            {
                StartUDPListener();
            }
            
            if (testMode)
            {
                Debug.Log("[ThreeSensorInput] TEST MODE - Use A/S/D keys for R/G/B, SPACE to confirm");
            }
        }
        
        private void Update()
        {
            // Smooth the sensor values
            redSensorValue = Mathf.Lerp(redSensorValue, targetRed, Time.deltaTime * smoothSpeed);
            greenSensorValue = Mathf.Lerp(greenSensorValue, targetGreen, Time.deltaTime * smoothSpeed);
            blueSensorValue = Mathf.Lerp(blueSensorValue, targetBlue, Time.deltaTime * smoothSpeed);
            
            // Test mode controls
            if (testMode)
            {
                HandleTestInput();
            }
            
            // Update visual guide
            if (visualGuide != null)
            {
                visualGuide.SetAllSensors(redSensorValue, greenSensorValue, blueSensorValue);
            }
            
            // Notify listeners
            OnSensorValuesChanged?.Invoke(redSensorValue, greenSensorValue, blueSensorValue);
        }
        
        private void HandleTestInput()
        {
            // A/S/D keys control R/G/B - colors STAY after releasing!
            // Hold key to INCREASE the color, release to STOP (color stays)
            
            if (Input.GetKey(KeyCode.A))
            {
                // Increase red while holding A
                targetRed = Mathf.Min(targetRed + Time.deltaTime * 1.5f, 1f);
            }
            // Red STAYS at current value when key is released!
            
            if (Input.GetKey(KeyCode.S))
            {
                // Increase green while holding S
                targetGreen = Mathf.Min(targetGreen + Time.deltaTime * 1.5f, 1f);
            }
            // Green STAYS at current value when key is released!
            
            if (Input.GetKey(KeyCode.D))
            {
                // Increase blue while holding D
                targetBlue = Mathf.Min(targetBlue + Time.deltaTime * 1.5f, 1f);
            }
            // Blue STAYS at current value when key is released!
            
            // Q/W/E to DECREASE colors (optional fine-tuning)
            if (Input.GetKey(KeyCode.Q))
            {
                targetRed = Mathf.Max(targetRed - Time.deltaTime * 1.5f, 0f);
            }
            if (Input.GetKey(KeyCode.W))
            {
                targetGreen = Mathf.Max(targetGreen - Time.deltaTime * 1.5f, 0f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                targetBlue = Mathf.Max(targetBlue - Time.deltaTime * 1.5f, 0f);
            }
            
            // R to RESET all colors to zero
            if (Input.GetKeyDown(KeyCode.R))
            {
                targetRed = 0f;
                targetGreen = 0f;
                targetBlue = 0f;
                Debug.Log("🔄 Colors reset to zero!");
            }
            
            // SPACE to confirm your color match!
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"✋ Confirming color: R={targetRed:F2}, G={targetGreen:F2}, B={targetBlue:F2}");
                visualGuide?.OnConfirm();
                OnConfirmGesture?.Invoke();
            }
        }
        
        private void StartUDPListener()
        {
            try
            {
                udpClient = new UdpClient(udpPort);
                isRunning = true;
                
                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                
                Debug.Log($"[ThreeSensorInput] Listening on UDP port {udpPort}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ThreeSensorInput] Failed to start UDP: {e.Message}");
            }
        }
        
        private void ReceiveData()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, udpPort);
            
            while (isRunning)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref endPoint);
                    string message = Encoding.UTF8.GetString(data);
                    
                    // Expected format: "R:0.5,G:0.3,B:0.8" or "0.5,0.3,0.8"
                    ParseSensorData(message);
                }
                catch (Exception)
                {
                    // Socket closed or error
                }
            }
        }
        
        private void ParseSensorData(string data)
        {
            try
            {
                string[] parts;
                
                if (data.Contains("R:"))
                {
                    // Format: R:0.5,G:0.3,B:0.8
                    data = data.Replace("R:", "").Replace("G:", "").Replace("B:", "");
                }
                
                parts = data.Split(',');
                
                if (parts.Length >= 3)
                {
                    targetRed = float.Parse(parts[0].Trim());
                    targetGreen = float.Parse(parts[1].Trim());
                    targetBlue = float.Parse(parts[2].Trim());
                }
                else if (parts.Length == 1)
                {
                    // Single sensor mode - use for all
                    float val = float.Parse(parts[0].Trim());
                    targetRed = val;
                    targetGreen = val;
                    targetBlue = val;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[ThreeSensorInput] Parse error: {e.Message}");
            }
        }
        
        /// <summary>
        /// Manually set sensor values (for testing or webcam input)
        /// </summary>
        public void SetSensorValues(float red, float green, float blue)
        {
            targetRed = Mathf.Clamp01(red);
            targetGreen = Mathf.Clamp01(green);
            targetBlue = Mathf.Clamp01(blue);
        }
        
        /// <summary>
        /// Trigger confirm action
        /// </summary>
        public void TriggerConfirm()
        {
            visualGuide?.OnConfirm();
            OnConfirmGesture?.Invoke();
        }
        
        /// <summary>
        /// Get current RGB values
        /// </summary>
        public Color GetCurrentColor()
        {
            return new Color(redSensorValue, greenSensorValue, blueSensorValue);
        }
        
        public float RedValue => redSensorValue;
        public float GreenValue => greenSensorValue;
        public float BlueValue => blueSensorValue;
        
        private void OnDestroy()
        {
            isRunning = false;
            udpClient?.Close();
            receiveThread?.Abort();
        }
    }
}
