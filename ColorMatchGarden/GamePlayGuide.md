# 🎮 How to Connect & Play Color Match Garden

Follow these steps to connect your 3 flex sensors and play the game using a mix of hardware and keyboard.

## 1. Setup the Hardware (Pico)
1.  Open **Thonny** and connect your Raspberry Pi Pico.
2.  Open [PicoFlexReader.py](file:///d:/unity/flexsensor/ColorMatchGarden/Python/PicoFlexReader.py) and copy the code.
3.  In Thonny, create a new file on the **Pico**, paste the code, and save it as `main.py`.
4.  **Run** the script. You should see three numbers appearing (e.g., `0,0,0`) in the Thonny console.

## 2. Connect to Unity (Computer)
1.  Keep the Pico running.
2.  In Thonny (on your computer), open [ThonnyUnityBridge.py](file:///d:/unity/flexsensor/ColorMatchGarden/Python/ThonnyUnityBridge.py).
3.  Change `SERIAL_PORT = "COM5"` to match your Pico's port (Look at the bottom right of Thonny to see your port).
4.  **Run** this script on your computer. You should see color icons (🔴🟢🔵) with numbers changing as you bend the sensors.

## 3. Play the Game
1.  Open your **Unity** project and press **Play**.
2.  The garden will appear with a target color in the top-left box.

### Controls:
-   **🔴 Red, 🟢 Green, 🔵 Blue**: Bend your **3 physical flex sensors**.
-   **🌟 Brightness**: Use keys **F** (brighter) and **T** (darker).
-   **✨ Magic Effect**: Use keys **G** (sparkle) and **Y** (clean).
-   **✋ Check Color**: Press the **SPACE BAR** to see if your color matches!

### Feedback:
-   **Match!**: If correct, you'll see a celebration and a new flower!
-   **Try Again**: If the color is wrong, the box will shake and say "Keep trying!"

---
> [!TIP]
> **Check the ground!** I've updated it to a vibrant **Grass Green** to make everything look neat and clear.
