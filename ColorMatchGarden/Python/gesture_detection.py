"""
Gesture Detection for Color Match Garden
Detects open hand and closed fist gestures using MediaPipe
Sends gesture data to Unity via UDP
"""

import cv2
import mediapipe as mp
import socket
import time

# Configuration
UNITY_HOST = "127.0.0.1"
UNITY_PORT = 5001
CAMERA_INDEX = 0

# MediaPipe setup
mp_hands = mp.solutions.hands
mp_drawing = mp.solutions.drawing_utils

def is_hand_open(hand_landmarks):
    """Check if hand is open (all fingers extended)"""
    # Finger tip landmarks
    finger_tips = [8, 12, 16, 20]  # Index, Middle, Ring, Pinky
    finger_pips = [6, 10, 14, 18]  # PIP joints
    
    fingers_extended = 0
    for tip, pip in zip(finger_tips, finger_pips):
        if hand_landmarks.landmark[tip].y < hand_landmarks.landmark[pip].y:
            fingers_extended += 1
    
    # Thumb check (horizontal movement)
    thumb_tip = hand_landmarks.landmark[4]
    thumb_ip = hand_landmarks.landmark[3]
    if abs(thumb_tip.x - thumb_ip.x) > 0.04:
        fingers_extended += 1
    
    return fingers_extended >= 4

def is_fist(hand_landmarks):
    """Check if hand is closed fist"""
    finger_tips = [8, 12, 16, 20]
    finger_pips = [6, 10, 14, 18]
    
    fingers_closed = 0
    for tip, pip in zip(finger_tips, finger_pips):
        if hand_landmarks.landmark[tip].y > hand_landmarks.landmark[pip].y:
            fingers_closed += 1
    
    return fingers_closed >= 3

def main():
    print("=" * 50)
    print("  Color Match Garden - Gesture Detection")
    print("=" * 50)
    
    # Setup UDP
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    print(f"[UDP] Sending to {UNITY_HOST}:{UNITY_PORT}")
    
    # Setup camera
    cap = cv2.VideoCapture(CAMERA_INDEX)
    if not cap.isOpened():
        print(f"[Error] Cannot open camera {CAMERA_INDEX}")
        return
    
    print(f"[Camera] Opened camera {CAMERA_INDEX}")
    print("\n[Gestures]")
    print("  ✋ Open Hand  = Confirm color")
    print("  ✊ Closed Fist = Reset color")
    print("\nPress 'Q' to quit\n")
    
    hands = mp_hands.Hands(
        static_image_mode=False,
        max_num_hands=1,
        min_detection_confidence=0.7,
        min_tracking_confidence=0.5
    )
    
    last_gesture = "none"
    gesture_start = 0
    
    try:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break
            
            # Mirror and convert
            frame = cv2.flip(frame, 1)
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            
            results = hands.process(rgb_frame)
            
            current_gesture = "none"
            
            if results.multi_hand_landmarks:
                for hand_landmarks in results.multi_hand_landmarks:
                    # Draw hand
                    mp_drawing.draw_landmarks(
                        frame, hand_landmarks, mp_hands.HAND_CONNECTIONS,
                        mp_drawing.DrawingSpec(color=(102, 204, 255), thickness=2),
                        mp_drawing.DrawingSpec(color=(255, 204, 102), thickness=2)
                    )
                    
                    # Detect gesture
                    if is_hand_open(hand_landmarks):
                        current_gesture = "open"
                    elif is_fist(hand_landmarks):
                        current_gesture = "fist"
            
            # Send gesture to Unity
            if current_gesture != last_gesture:
                sock.sendto(current_gesture.encode(), (UNITY_HOST, UNITY_PORT))
                last_gesture = current_gesture
                gesture_start = time.time()
            
            # Draw UI overlay
            gesture_text = {
                "open": "OPEN HAND (Confirm)",
                "fist": "CLOSED FIST (Reset)",
                "none": "Show your hand..."
            }
            
            color = {
                "open": (102, 255, 102),
                "fist": (102, 178, 255),
                "none": (200, 200, 200)
            }
            
            # Background box
            cv2.rectangle(frame, (10, 10), (350, 70), (40, 40, 40), -1)
            cv2.rectangle(frame, (10, 10), (350, 70), color[current_gesture], 2)
            
            # Gesture text
            cv2.putText(frame, gesture_text[current_gesture], (20, 50),
                       cv2.FONT_HERSHEY_SIMPLEX, 0.8, color[current_gesture], 2)
            
            # Hold time indicator
            if current_gesture != "none":
                hold_time = time.time() - gesture_start
                bar_width = min(int(hold_time * 100), 300)
                cv2.rectangle(frame, (20, 60), (20 + bar_width, 65), color[current_gesture], -1)
            
            cv2.imshow("Color Match Garden - Gesture Detection", frame)
            
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
                
    except KeyboardInterrupt:
        pass
    finally:
        cap.release()
        cv2.destroyAllWindows()
        sock.close()
        print("\n[Stopped] Gesture detection closed")

if __name__ == "__main__":
    main()
