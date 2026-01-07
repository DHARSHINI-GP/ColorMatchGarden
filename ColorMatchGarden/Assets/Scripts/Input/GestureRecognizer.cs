using UnityEngine;
using UnityEngine.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ColorMatchGarden.Core
{
    /// <summary>
    /// Recognizes hand gestures from webcam via Python bridge.
    /// Open hand ✋ = Confirm | Closed fist ✊ = Reset
    /// </summary>
    public class GestureRecognizer : MonoBehaviour
    {
        [Header("Connection Settings")]
        [SerializeField] private int listenPort = 5001;
        [SerializeField] private bool useSimulation = true;
        
        [Header("Gesture Settings")]
        [SerializeField] private float confirmHoldTime = 1.5f;
        [SerializeField] private float resetHoldTime = 1.0f;
        [SerializeField] private float gestureTimeout = 2f;
        
        [Header("Visual Feedback")]
        [SerializeField] private GameObject confirmIndicator;
        [SerializeField] private GameObject resetIndicator;
        [SerializeField] private UnityEngine.UI.Image holdProgressRing;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = true;

        [Header("Events")]
        public UnityEvent OnOpenHandDetected;
        public UnityEvent OnClosedFistDetected;
        public UnityEvent OnConfirmGesture;
        public UnityEvent OnResetGesture;
        public UnityEvent OnGestureLost;

        private GestureState currentGesture = GestureState.None;
        private float gestureHoldTimer = 0f;
        private float lastGestureTime = 0f;
        private bool gestureConfirmed = false;
        
        private UdpClient udpClient;
        private Thread receiveThread;
        private bool isRunning = false;
        private string latestGestureMessage = "";

        private void Start()
        {
            if (!useSimulation)
            {
                StartNetworkListener();
            }
            
            // Hide indicators initially
            if (confirmIndicator) confirmIndicator.SetActive(false);
            if (resetIndicator) resetIndicator.SetActive(false);
        }

        private void Update()
        {
            if (useSimulation)
            {
                HandleSimulationInput();
            }
            else
            {
                ProcessNetworkGesture();
            }
            
            UpdateGestureHold();
            CheckGestureTimeout();
            UpdateVisualFeedback();
        }

        private void HandleSimulationInput()
        {
            // Space for open hand (confirm)
            if (Input.GetKey(KeyCode.Space))
            {
                SetGesture(GestureState.OpenHand);
            }
            // R for closed fist (reset)
            else if (Input.GetKey(KeyCode.R))
            {
                SetGesture(GestureState.ClosedFist);
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.R))
            {
                SetGesture(GestureState.None);
            }
        }

        private void ProcessNetworkGesture()
        {
            if (string.IsNullOrEmpty(latestGestureMessage)) return;
            
            string gesture = latestGestureMessage.ToLower().Trim();
            latestGestureMessage = "";
            
            switch (gesture)
            {
                case "open":
                case "open_hand":
                case "palm":
                    SetGesture(GestureState.OpenHand);
                    break;
                case "closed":
                case "fist":
                case "closed_fist":
                    SetGesture(GestureState.ClosedFist);
                    break;
                case "none":
                case "":
                    SetGesture(GestureState.None);
                    break;
            }
        }

        private void SetGesture(GestureState newGesture)
        {
            if (newGesture != currentGesture)
            {
                // Reset hold timer when gesture changes
                gestureHoldTimer = 0f;
                gestureConfirmed = false;
                currentGesture = newGesture;
                
                switch (newGesture)
                {
                    case GestureState.OpenHand:
                        OnOpenHandDetected?.Invoke();
                        break;
                    case GestureState.ClosedFist:
                        OnClosedFistDetected?.Invoke();
                        break;
                    case GestureState.None:
                        OnGestureLost?.Invoke();
                        break;
                }
            }
            
            lastGestureTime = Time.time;
        }

        private void UpdateGestureHold()
        {
            if (gestureConfirmed) return;
            
            float holdTime = AccessibilityManager.Instance?.GetCurrentSettings().GestureHoldTime ?? confirmHoldTime;
            
            switch (currentGesture)
            {
                case GestureState.OpenHand:
                    gestureHoldTimer += Time.deltaTime;
                    if (gestureHoldTimer >= holdTime)
                    {
                        gestureConfirmed = true;
                        OnConfirmGesture?.Invoke();
                        GameManager.Instance?.OnConfirmGesture();
                        PlayGestureEffect(true);
                    }
                    break;
                    
                case GestureState.ClosedFist:
                    gestureHoldTimer += Time.deltaTime;
                    if (gestureHoldTimer >= resetHoldTime)
                    {
                        gestureConfirmed = true;
                        OnResetGesture?.Invoke();
                        GameManager.Instance?.OnResetGesture();
                        PlayGestureEffect(false);
                    }
                    break;
            }
        }

        private void CheckGestureTimeout()
        {
            if (currentGesture != GestureState.None && 
                Time.time - lastGestureTime > gestureTimeout)
            {
                SetGesture(GestureState.None);
            }
        }

        private void UpdateVisualFeedback()
        {
            // Show appropriate indicator
            if (confirmIndicator)
            {
                confirmIndicator.SetActive(currentGesture == GestureState.OpenHand);
            }
            if (resetIndicator)
            {
                resetIndicator.SetActive(currentGesture == GestureState.ClosedFist);
            }
            
            // Update progress ring
            if (holdProgressRing != null)
            {
                float targetTime = currentGesture == GestureState.OpenHand ? confirmHoldTime : resetHoldTime;
                float progress = targetTime > 0 ? gestureHoldTimer / targetTime : 0;
                holdProgressRing.fillAmount = progress;
                
                // Gentle color transition
                holdProgressRing.color = Color.Lerp(
                    new Color(0.5f, 0.8f, 1f, 0.5f),
                    new Color(0.6f, 1f, 0.6f, 0.8f),
                    progress
                );
            }
        }

        private void PlayGestureEffect(bool isConfirm)
        {
            // Visual and audio feedback for completed gesture
            // This would trigger particle effects and sounds
            if (isConfirm)
            {
                Debug.Log("[Gesture] ✋ Confirm gesture completed!");
            }
            else
            {
                Debug.Log("[Gesture] ✊ Reset gesture completed!");
            }
        }

        private void StartNetworkListener()
        {
            try
            {
                udpClient = new UdpClient(listenPort);
                isRunning = true;
                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                Debug.Log($"[Gesture] Listening on port {listenPort}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Gesture] Failed to start listener: {e.Message}");
            }
        }

        private void ReceiveData()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            while (isRunning)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    latestGestureMessage = Encoding.UTF8.GetString(data);
                    lastGestureTime = Time.time;
                }
                catch (SocketException)
                {
                    // Socket closed
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Gesture] Receive error: {e.Message}");
                }
            }
        }

        private void OnDestroy()
        {
            isRunning = false;
            udpClient?.Close();
            receiveThread?.Abort();
        }

        private void OnGUI()
        {
            if (!showDebugInfo) return;
            
            GUILayout.BeginArea(new Rect(10, 160, 300, 100));
            GUILayout.Label("Gesture Debug");
            GUILayout.Label($"Current: {currentGesture}");
            GUILayout.Label($"Hold Time: {gestureHoldTimer:F1}s");
            GUILayout.Label($"Confirmed: {gestureConfirmed}");
            GUILayout.EndArea();
        }

        public GestureState GetCurrentGesture() => currentGesture;
        public float GetHoldProgress() => gestureHoldTimer / confirmHoldTime;
    }

    public enum GestureState
    {
        None,
        OpenHand,      // ✋ Confirm
        ClosedFist     // ✊ Reset
    }
}
