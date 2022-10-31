#if UNITY_STANDALONE

using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour {
  /***** Public Variables *****/
  // Must be configured in the Unity Editor
  public string portName;
  
  /***** Private Variables *****/
  // Since the serial port names on Mac and PC are different, we store all
  // possible names here and print them out when the program starts.
  private string[] _serialPorts;
  private const int BaudRate = 9600; // This matches the Arduino's baud rate.
  private SerialPort _serialPort = new SerialPort(); // TODO: Not sure if we are making duplicates.
  
  public enum BreathingStates { BreathingIn, BreathingOut }
  private BreathingStates _breathingState = BreathingStates.BreathingIn;

  private int _previousValue = 0; // Values cannot be negative. 

  void Start() {
    // Get all possible serial port names.
    _serialPorts = SerialPort.GetPortNames();
    // And print them to the console for debugging purposes.
    Debug.Log("Possible serial ports: " + _serialPorts.Length);
    foreach (string port in _serialPorts) {
      Debug.Log(port);
    }

    // Open the serial port.
    // This will fail if the port is not found.
    _serialPort.PortName = portName;
    _serialPort.BaudRate = BaudRate;
    _serialPort.ReadTimeout = 1; // TODO: Check if this is needed. 
    try {
      _serialPort.Open();
    }
    catch (System.Exception e) {
      Debug.Log("Error opening serial port: " + e.Message);
    }
  }

  void Update() {
    // Read the serial port if open.
    if (_serialPort.IsOpen) {
      try {
        // Parse the serial port data to an integer
        int value = int.Parse(_serialPort.ReadLine());
        Debug.Log("Received: " + value);
        
        // If there is no previous value, set it to the current value
        if (_previousValue == 0) { _previousValue = value; }
        // If there is a previous value, determine if there is a state change
        else {
          if (value > _previousValue) {
            // If the value is greater than the previous value, it is breathing out.
            _breathingState = BreathingStates.BreathingOut;
            Debug.Log("State: Breathing Out");
          }
          else if (value < _previousValue) {
            // If the value is less than the previous value, it is breathing in.
            _breathingState = BreathingStates.BreathingIn;
            Debug.Log("State: Breathing In");
          }
        }
      }
      catch (System.Exception e) {
        Debug.Log("Error reading serial port: " + e.Message);
      }
    }
  }
  
  /***** Public Methods *****/
  public BreathingStates GetBreathingState() {
    // Return the current breathing state.
    return _breathingState;
  }
}

#endif