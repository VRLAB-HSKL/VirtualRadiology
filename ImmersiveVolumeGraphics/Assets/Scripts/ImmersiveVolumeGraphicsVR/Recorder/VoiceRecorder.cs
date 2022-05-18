using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityVolumeRendering;
using ImmersiveVolumeGraphics.ModelImport;
using TMPro;

namespace ImmersiveVolumeGraphics
{

    /// <summary>
    /// This namespace containts all components for the VoiceRecorder aka Recorder 
    /// </summary>

    /// <seealso>
    /// <ul>
    /// <li>Sources:</li>
    /// <li> [1] http://gyanendushekhar.com/2016/10/11/speech-recognition-in-unity3d-windows-speech-api/ </li>
    /// <li> [2] https://docs.microsoft.com/de-de/windows/mixed-reality/voice-input-in-unity </li>
    /// <li> [3] https://stackoverflow.com/questions/47471562/save-and-load-audio </li>
    /// </ul>
    /// </seealso>
    namespace Recorder
    {
        public class VoiceRecorder: MonoBehaviour
        {

            /// <summary>
            /// KeywordRecognizer from the Microsoft Speech API
            /// </summary>
            private KeywordRecognizer keywordRecognizer;


            /// <summary>
            /// DictationRecognizer from the Microsoft Speech API
            /// </summary>

            private DictationRecognizer dictationRecognizer;

        

            /// <summary>
            /// // Storing the text
            /// </summary
            
            private string dictationText = "";

            /// <summary>
            /// Informationhint for the user in VR
            /// </summary
            public TMP_Text RecorderLabel;

            /// <summary>
            /// List of keywords that are usued for the voice commands
            /// </summary
            public string[] Keywords_Array;



            /// <summary>
            /// Initialization of the recognizers
            /// </summary>
            /// <remarks>
            /// 
            /// <ul>
            /// <li>Creates a new array and adds the keywords to it</li>
            /// <li>Initialization of the KeyWordRecognizer</li>
            /// <li>Initialization of the DictationFunction and adding the listeners</li>
            /// <li>Creates a new directory for the recording</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            void Start()
            {
                // Creates a new array and adds the keywords to it
                Keywords_Array = new string[5];
                Keywords_Array[0] = "import";
                Keywords_Array[1] = "record";
                Keywords_Array[2] = "quit";
                Keywords_Array[3] = "stop";
                Keywords_Array[4] = "load";

                // Initialization
                // Keywords
                keywordRecognizer = new KeywordRecognizer(Keywords_Array);
                keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
                keywordRecognizer.Start();

                // DictationFunction Initialization and adding the listeners
                dictationRecognizer = new DictationRecognizer();
                dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
                dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationError += DictationRecognizer_DictationError;

                // Creates a new directory for the recording
                Directory.CreateDirectory(Application.dataPath + "/Recording/");

            }


            /// <summary>
            /// Trigger when a word is recognized
            /// </summary>
            /// <remarks>
            /// 
            /// <ul>
            /// <li>Did the user say "import" ?</li>
            /// <li>Import the model using the OpenRAWDataset-Method</li>
            /// </ul> 
            /// <pre> </pre>
            /// <ul>
            /// <li>Did the user say "record"?</li>
            /// <li>Changes the text to "Recording" in the color red to signalize that the recorded actually started</li>
            /// <li>Stop the keywordlistener</li>
            /// <li>Shut down the PhraseRecognitionSystem</li>
            /// <li>Only one Recognizer can run at a time</li>
            /// <li>Starts the dictation</li>
            /// </ul> 
            /// <pre> </pre>
            /// <ul>
            /// <li>Did the user say "quit" ?</li>
            /// <li>Quit the Application</li>
            /// </ul> 

            /// </remarks>
            /// <param name="args"></param>
            /// <returns>void</returns>



            // Triggers when a word is recognized
            void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
            {

                //Debugs the word
                Debug.Log(args.text);

                // Did the user say "import" ?
                if (MainMenu.Status == 1)
                {
                    if (args.text == "import")
                    {
                        //Debuginformation
                        Debug.Log("Model loaded sucessfully");
                        //Imports the model using the OpenRAWDataset-Method
                        ImportRAWModel.OpenRAWDataset();

                    }
                }

                // Did the user say "record" ?
                if (MainMenu.Status == 4)
                {
                    if (args.text == "record")
                    {
                        //Changes the text to "Recording" in the color red to signalize that the recorded actually started

                        RecorderLabel.text = "recording";
                        RecorderLabel.color = Color.red;

                        // Stops the keyword listener
                        keywordRecognizer.Stop();
                        //Shuts down the PhraseRecognitionSystem
                        //Only one Recognizer can run at a time
                        PhraseRecognitionSystem.Shutdown();
                        //Debuginformation
                        Debug.Log("Recording started");
                        // Starts the dictation
                        dictationRecognizer.Start();

                    }
                }


                // Did the user say "quit" ?


                if (args.text == "quit")
                {
                    // Quits the program
                    Application.Quit();

                }

                /*
                //For Testing
                if (args.text == "load")
                {

                    //  string tempPath = Path.Combine(Application.persistentDataPath, "Audio");
                    //tempPath = Path.Combine(tempPath, "storyrecord.wav");
                    //                Debug.Log(tempPath);  
                        string tempPath = Application.dataPath + "/Recording/";
                        Debug.Log(tempPath);

                }*/

            }


            // Triggers when a sentence is finished 



            /// <summary>
            /// Triggers when a sentence is finished 
            /// </summary>
            /// <remarks>
            /// Finding the GameObjects in the Scene
            /// <ul>
            /// <li>Did the user say "stop"? </li>
            /// <li>Stop the Dication / Recording</li>
            /// <li>Changes the text to "Not Recording" in the color black to signalize that the recorded actually stopped</li>
            /// <li>Add the current date and time to the filename</li>
            /// <li>the location where the recording is stored</li>
            /// <code>Application.dataPath + "/Recording/" + date + "_" + "recording.txt"</code>
            /// <li>Give the path to a Streamwriter</li>
            /// <li>The StreamWriter writes the stored Data</li>
            /// </ul> 
            /// </remarks>
            /// <param name="confidence"></param>
            /// <returns>void</returns>


            private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
            {

                // Debuginformation
                Debug.Log(text + " ");
                //adds the text our string in a format
                dictationText += text + "\n";

                // Did the user say "stop"? 
                // sometimes it happens that the Microsoft Speech API doesnt recognize words well 

                if ((text == "stopp" || text == "stop"))
                {

                    // Debuginformation
                    Debug.Log(text + " ");
                    // Stop the Dication / Recording 
                    dictationRecognizer.Stop();
                    //Debuinformation
                    Debug.Log("Recording stopped ");
                    //Changes the text to "Not Recording" in the color black to signalize that the recorded actually stopped
                    RecorderLabel.text = "not recording";
                    RecorderLabel.color = Color.black;

                    //adds the current date and time to the filename
                    string date = DateTime.Now.ToString("dd-MM-yyyy+HH-mm-ss");
                    // the location where the recording is stored
                    // file type is .txt 
                    string filepath = Application.dataPath + "/Recording/" + date + "_" + "recording.txt";
                    // gives the path to the Streamwriter 
                    StreamWriter Writer = new StreamWriter(filepath);
                    // the StreamWriter writes the stored Data 
                    Writer.WriteLine(dictationText);
                    // Clearing the buffer
                    Writer.Flush();
                    // Closing the Writer to prevent memory leak
                    Writer.Close();
                    // Reset of the text because the recording is finished
                    dictationText = "";


                }

            }

            // The Microsoft Voice API can predict words that you are saying 
            // This event handles predicted words 


            /// <summary>
            /// The Microsoft Voice API can predict words that the user is saying
            /// </summary>
            /// <remarks>
            /// This event handles predicted words
            /// </remarks>
            /// <param name="text"></param>
            /// <returns>void</returns>
            /// 
            private void DictationRecognizer_DictationHypothesis(string text)
            {


            }


            /// <summary>
            /// This method is triggered when the dication is complete
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Restart the  PhraseRecognitionSystem/li>
            /// <li>Start the keywordRecognizer for new voice commands</li>
            /// </ul> 
            /// </remarks>
            /// <param name="cause"></param>
            /// <returns>void</returns>


            // Is triggered when the Dication is complete
            private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
            {

                // Restarts the  PhraseRecognitionSystem 
                PhraseRecognitionSystem.Restart();
                // Starts the keywordRecognizer for new voice commands
                keywordRecognizer.Start();


            }

            /// <summary>
            /// This method is triggered when an error occured (error handling)
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="error, hresult"></param>
            /// <returns>void</returns>

            private void DictationRecognizer_DictationError(string error, int hresult)
            {

            }


        }

    }
}
