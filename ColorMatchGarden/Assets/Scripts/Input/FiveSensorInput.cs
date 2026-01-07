using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Handles 5 flex sensors for color mixing.
    /// RECREATED with fixes for "Sticky Color" issue and missing properties.
    /// </summary>
    public class FiveSensorInput : MonoBehaviour
    {
        [Header("Network Settings")]
        [SerializeField] private int udpPort = 5005;
        [SerializeField] private bool autoConnect = true;
        
        [Header("Sensor Values (0-1)")]
        [SerializeField] [Range(0, 1)] private float sensor1Value = 0f; // Thumb (Red)
        [SerializeField] [Range(0, 1)] private float sensor2Value = 0f; // Index (Yellow)
        [SerializeField] [Range(0, 1)] private float sensor3Value = 0f; // Middle (Green)
        [SerializeField] [Range(0, 1)] private float sensor4Value = 0f; // Ring (Cyan/Brightness)
        [SerializeField] [Range(0, 1)] private float sensor5Value = 0f; // Pinky (Blue/Magic)
        
        [Header("Color Mixing")]
        [SerializeField] private Color mixedColor = Color.black;
        
        [Header("Responsiveness")]
        [SerializeField] private float riseSpeed = 3f;   // Slower, smoother rise
        [SerializeField] private float decaySpeed = 2f;  // Slower decay for stability (was 8f)
        
        [Header("Stability")]
        [SerializeField] private bool stabilizeValues = true;
        [SerializeField] private int stabilitySteps = 5; // Snap to 0, 0.2, 0.4, 0.6, 0.8, 1.0
        
        [Header("Test Mode (Keyboard)")]
        [SerializeField] private bool testMode = true;
        
        // Target values for specific sensors (raw input)
        private float target1, target2, target3, target4, target5;
        
        // UDP
        private UdpClient udpClient;
        private Thread receiveThread;
        private bool isRunning = false;
        
        // Events
        public event Action<Color> OnColorChanged;
        public event Action<float, float, float, float, float> OnSensorValuesChanged;
        public event Action OnConfirmGesture;
        
        // Compatibility Properties for ColorController
        public float ThumbValue => sensor1Value;
        public float IndexValue => sensor2Value;
        public float MiddleValue => sensor3Value;
        public float RingValue => sensor4Value;
        public float PinkyValue => sensor5Value;

        // Singleton
        public static FiveSensorInput Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start()
        {
            if (autoConnect && !testMode) StartUDPListener();
        }
        
        private void Update()
        {
            float dt = Time.deltaTime;
            
            // Handle Keyboard Test Input
            if (testMode) HandleTestInput();
            
            // Apply Stability (Quantization) to Targets
            // This prevents shaky hands from flickering the color
            float t1 = stabilizeValues ? Quantize(target1) : target1;
            float t2 = stabilizeValues ? Quantize(target2) : target2;
            float t3 = stabilizeValues ? Quantize(target3) : target3;
            float t4 = stabilizeValues ? Quantize(target4) : target4;
            float t5 = stabilizeValues ? Quantize(target5) : target5;
            
            // Smoothly move current values towards STABLE targets
            sensor1Value = SmoothValue(sensor1Value, t1, dt);
            sensor2Value = SmoothValue(sensor2Value, t2, dt);
            sensor3Value = SmoothValue(sensor3Value, t3, dt);
            sensor4Value = SmoothValue(sensor4Value, t4, dt);
            sensor5Value = SmoothValue(sensor5Value, t5, dt);
            
            // Calculate mixed color
            UpdateMixedColor();
            
            // Notify listeners
            OnSensorValuesChanged?.Invoke(sensor1Value, sensor2Value, sensor3Value, sensor4Value, sensor5Value);
            OnColorChanged?.Invoke(mixedColor);
        }
        
        private float Quantize(float value)
        {
            if (stabilitySteps <= 1) return value;
            float step = 1f / stabilitySteps;
            return Mathf.Round(value / step) * step;
        }
        
        private float SmoothValue(float current, float target, float dt)
        {
            // If very close, just snap to avoid micro-jitter
            if (Mathf.Abs(current - target) < 0.01f) return target;
            
            if (target > current)
                return Mathf.MoveTowards(current, target, riseSpeed * dt);
            else
                return Mathf.MoveTowards(current, target, decaySpeed * dt);
        }
        
        private void HandleTestInput()
        {
            // Reset targets to 0 every frame in test mode, unless key held
            // This ensures "non-sticky" behavior
            target1 = Input.GetKey(KeyCode.Q) ? 1f : 0f;
            target2 = Input.GetKey(KeyCode.W) ? 1f : 0f;
            target3 = Input.GetKey(KeyCode.E) ? 1f : 0f;
            target4 = Input.GetKey(KeyCode.R) ? 1f : 0f;
            target5 = Input.GetKey(KeyCode.T) ? 1f : 0f;
            
            // SPACE to confirm
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnConfirmGesture?.Invoke();
            }
        }
        
        private void UpdateMixedColor()
        {
            // NEW MIXING LOGIC: Weighted Average
            // This allows Red + Yellow = Orange! ((1,0,0) + (1,1,0) = (1, 0.5, 0))
            
            Color c1 = Color.red;
            Color c2 = Color.yellow;
            Color c3 = Color.green;
            Color c4 = Color.cyan;
            Color c5 = new Color(0.6f, 0f, 1f); // Purple
            
            float w1 = sensor1Value;
            float w2 = sensor2Value;
            float w3 = sensor3Value;
            float w4 = sensor4Value;
            float w5 = sensor5Value;
            
            float totalWeight = w1 + w2 + w3 + w4 + w5;
            
            if (totalWeight < 0.1f)
            {
                mixedColor = Color.black;
                return;
            }
            
            // Average mixing
            float r = (c1.r * w1 + c2.r * w2 + c3.r * w3 + c4.r * w4 + c5.r * w5) / totalWeight;
            float g = (c1.g * w1 + c2.g * w2 + c3.g * w3 + c4.g * w4 + c5.g * w5) / totalWeight;
            float b = (c1.b * w1 + c2.b * w2 + c3.b * w3 + c4.b * w4 + c5.b * w5) / totalWeight;
            
            // Boost brightness (Average can be dark)
            // If total weight is high, keep it bright
            float brightnessBoost = 1.0f; // Could be dynamic
            
            mixedColor = new Color(Mathf.Clamp01(r * brightnessBoost), Mathf.Clamp01(g * brightnessBoost), Mathf.Clamp01(b * brightnessBoost));
        }
        
        private void StartUDPListener()
        {
            try
            {
                udpClient = new UdpClient(udpPort);
                udpClient.Client.ReceiveTimeout = 100; // fast timeout
                isRunning = true;
                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception e) { Debug.LogError($"UDP Error: {e.Message}"); }
        }
        
        private void ReceiveData()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, udpPort);
            while (isRunning)
            {
                try
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] data = udpClient.Receive(ref endPoint);
                        string message = Encoding.UTF8.GetString(data);
                        ParseSensorData(message);
                    }
                    else Thread.Sleep(10);
                }
                catch { /* Ignore timeouts */ }
            }
        }
        
        private void ParseSensorData(string data)
        {
            try
            {
                string[] parts = data.Trim().Split(',');
                if (parts.Length >= 5)
                {
                    target1 = Mathf.Clamp01(float.Parse(parts[0]));
                    target2 = Mathf.Clamp01(float.Parse(parts[1]));
                    target3 = Mathf.Clamp01(float.Parse(parts[2]));
                    target4 = Mathf.Clamp01(float.Parse(parts[3]));
                    target5 = Mathf.Clamp01(float.Parse(parts[4]));
                }
            }
            catch {}
        }
        
        private void OnDestroy()
        {
            isRunning = false;
            udpClient?.Close();
            receiveThread?.Abort();
        }
        
        // Public setters
        public void SetSensorValues(float s1, float s2, float s3, float s4, float s5)
        {
            target1 = s1; target2 = s2; target3 = s3; target4 = s4; target5 = s5;
        }
    }
}
