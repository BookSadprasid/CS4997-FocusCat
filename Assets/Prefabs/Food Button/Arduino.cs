#if UNITY_STANDALONE

using System.IO.Ports;
using UnityEngine;

public class Arduino {
  /***** Private Variables *****/
  private const int BaudRate = 9600;
  private SerialPort _serialPort;

  // These values represents the range of acceptable values.
  private int min, max; 
  
  // FIXME: REMOVE ME
  private float _value;
  private float _increment = 1f;
  
  /**
   * Constructor
   * Given a port name, open the port.
   */
  public Arduino(string portName, int min, int max) {
    // Set the min ana max values.
    this.min = min;
    this.max = max;
    
    // Instantiate the serial port.
    _serialPort = new SerialPort();
    // _serialPort.PortName = portName;
    // _serialPort.BaudRate = BaudRate;
    
    // Attempt to open the serial port
    // try {
    //   _serialPort.Open();
    //   Debug.Log("Serial Port Opened");
    // } catch (Exception e) {
    //   Debug.Log("Error opening serial port: " + e.Message);
    // }
  }

  /***** Public Methods *****/
  /** Returns the percentage of the range of based on the min ana max value */
  public float GetValuePercentage() {
    // Get the value from the arduino.
    int value = GetValue();
    Debug.Log("Value: " + value);
    
    // Calculate the percentage.
    if (value <= min) return 0;
    if (value >= max) return 1;
    return (float)(value - min) / (max - min);
  }
  
  public int GetValue() {
    // FIXME: We want to make sure the buffer is not filling quicker than we can read it.
    // FIXME: Since we don't have an arduino connected, we will return growing numbers
    // Debug.Log(serialPort.ReadBufferSize);
    // return int.Parse(serialPort.ReadLine());

    // FIXME: REMOVE ME WHEN ARDUINO IS CONNECTED
    if (_value < min) {
      _value = min;
      _increment *= -1;
    } 
    else if (_value > max) {
      _value = max;
      _increment *= -1;
    }
    _value += _increment;
    return (int)_value;
  }
}

#endif