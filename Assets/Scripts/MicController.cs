using System;
using UnityEngine;
using UnityEngine.Audio;

public class MicController : MonoBehaviour {
  /***** Public Variables *****/
  // The audio mixer for the game
  public AudioMixer audioMixer; 
  // Debug: Send in breathing audio clips for to test hit detection
  public AudioClip overrideMicrophoneInput;
  
  /*** Debug **/
  [Space(20)]
  public int sampleSize = 4096;

  // Frequency of interest
  // 500hz to 1000hz is where the breathing happens
  [Header("Breathing Range (hz)")]
  public int startFreq = 60; 
  public int endFreq = 500;
  
  [Space(20)]
  public float BreathOutThreshold = 2.5f;
  // public int  BreathInThreshold;
  
  // A basic Square which will represent the a spectrum
  public GameObject square;
  
  /***** Private Variables *****/
  private AudioSource _audioSource;
  public float[] _spectrumData;
  private GameObject[] _squares;

  /***** Unity Functions *****/
  void Start() {
    /*** Mic ***/
    // Initialize the Microphone
    InitMic();
    
    // Start listening to the mic
    _audioSource.Play();
    
    /*** Spectrum Visualization ***/
    // Initialize the spectrum data
    _spectrumData = new float[sampleSize];
    _squares      = new GameObject[sampleSize];
    
    // DEBUG: Create a square for each sample
    for (int i = 0; i < sampleSize; i++) {
      _squares[i] = Instantiate(square, new Vector2(i * 1.5f, 0), Quaternion.identity);
    }
    
    // DEBUG: Print information about the ranges we want to calculate
    // Half the audio frequency range since it's doubled for left and right specter
    float channelFrequency = AudioSettings.outputSampleRate / 2;
    Debug.Log("Sample Rate: " + AudioSettings.outputSampleRate / 2);
    Debug.Log("Start Frequency: " + startFreq + "Hz");
    Debug.Log("End Frequency: " + endFreq + "Hz");
    Debug.Log("Start Index: " + (int)Math.Ceiling(startFreq / (channelFrequency / sampleSize)));
    Debug.Log("End Index: " + (int)Math.Ceiling(endFreq / (channelFrequency / sampleSize)));
  }

  void Update() {
    // Refresh the spectrum data
    _audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);
    
    // Only show the spectrum data within the range of interest
    float channelFrequency = AudioSettings.outputSampleRate / 2;
    int   startIndex       = (int) Math.Ceiling(startFreq / (channelFrequency / sampleSize));
    int   endIndex         = (int)Math.Ceiling(endFreq / (channelFrequency / sampleSize));

    float rangeAverage = 0;
    for(int i = 0; i < sampleSize; i++) {
      _squares[i].transform.localScale = new Vector2(_squares[i].transform.localScale.x, 1 + (_spectrumData[i] * 1000));
      _squares[i].transform.position = new Vector2(_squares[i].transform.position.x, _squares[i].transform.localScale.y / 2);
      // Highlight only the ranges of interest
      if (startIndex <= i &&  i <= endIndex) {
        _squares[i].GetComponent<SpriteRenderer>().color = Color.blue;
        rangeAverage += 1 + _spectrumData[i] * 1000;
      }
    }
    
    rangeAverage /= (endIndex - startIndex);
    Debug.Log("Average: " + rangeAverage);
    if (rangeAverage > BreathOutThreshold) {
      Debug.Log("Breathing Out");
    } else {
      Debug.Log("Breathing In");
    }
  }
  
  /***** Private Functions *****/
  private void InitMic() {
    /*** Safety Check ***/
    // Check if a microphone exists
    // if (Microphone.devices.Length == 0) {
    //   Debug.LogError("No microphone found");
    // }

    // for (int i = 0; i < Microphone.devices.Length; i++) {
    //   Debug.Log("Mic found: " + Microphone.devices[i]);
    // }
    
    /*** Create & Set Audio Source ***/
    // Create an AudioSource for the microphone
    // _audioSource = gameObject.AddComponent<AudioSource>();
    _audioSource = GetComponent<AudioSource>();
        
    // Setup microphone AudioSource
    if (overrideMicrophoneInput == null) {
      // _audioSource.clip = Microphone.Start(
      //   // TODO: We may benefit from allowing the user to chose an Microphone device
      //   null, // Selects the default audio device at `Microphone.devices[0]`s.
      //   true,       // So that we continuously record the data
      //   10,     // This should be that minimum size for a sound event due to looping
      //   AudioSettings.outputSampleRate
      // );
      //
      // while (!(Microphone.GetPosition(null) > 0)) { } // Wait for the microphone to start
    }
    // Debug only
    else {
      _audioSource.clip = overrideMicrophoneInput;
    }
    _audioSource.loop = true; // Also loop the audio source
    
    /*** Assign Audio Source to Audio Mixer ***/
    // Since we want to capture the microphones audio but not hear it, we need to 
    // assign it's output to the muted "Microphone" AudioMixerGroup
    AudioMixerGroup micMixerGroup = audioMixer.FindMatchingGroups("Microphone")[0];
    if (micMixerGroup == null) {
      // To resolve this, make sure the AudioMixer is setup on the Game Manager
      // with a AudioMixerGroup named "Microphone"
      throw new Exception("'Microphone' AudioMixerGroup is not found");
    }
    _audioSource.outputAudioMixerGroup = micMixerGroup;
  }
}
