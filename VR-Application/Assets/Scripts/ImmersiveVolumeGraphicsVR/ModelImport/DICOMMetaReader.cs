using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityVolumeRendering;
using TMPro;

namespace ImmersiveVolumeGraphics {


    namespace ModelImport
    {
        /// <summary>
        ///This class reads DICOM-Metadata from a Textfile in and stores them in Variables 
        /// </summary>


        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] http://dicom.nema.org/medical/dicom/current/output/chtml/part06/chapter_1.html </li>
        /// <li> [2] http://dicom.nema.org/medical/dicom/current/output/chtml/part06/chapter_6.html </li>
        /// </ul>
        /// </seealso>

        public class DICOMMetaReader : MonoBehaviour
        {
            /// <summary>
            /// Array of all  metainformation eg. patient´s name , modality etc.
            /// Only reasonable information is included
            /// </summary>
            private static string[] metaInformation = new string[50];

            /// <summary>
            /// Textfield which shows the metainformation to the user 
            /// </summary>
            public static TMP_Text MetaInformationText;
          

           


            /// <summary>
            /// Distance from the center of a pixel to the border in x-Direction in Millimeter(mm), eg. 1.0 = 1 mm distance
            /// </summary>
            private static float pixelSpacingX = 0;
            /// <summary>
            /// Distance from the center of a pixel to the border in y-Direction in Millimeter(mm),  eg. 1.0 = 1 mm distance
            /// </summary>
            private static float pixelSpacingY = 0;

            /// <summary>
            /// This value represents the thickness of each slice made in the computertomograph in Millimeter eg. 1.0 = 1 mm thickness
            /// </summary>
            private static float sliceThickness = 0;


            // Start is called before the first frame update

            /// <summary>
            /// Findings the MetaInfoLabel in the Scene and sets MetaInformationText
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            void OnEnable()//Start()
            {
                // Finding the text in the scene 
                MetaInformationText = GameObject.Find("MetaInfoLabel").GetComponent<TMP_Text>();
                if (MetaInformationText is null)
                {
                    Debug.Log("MetaInfo null");
                }
            }



            /// <summary>
            /// Read the DICOM-Metainformation from a Textfile
            /// </summary>

            /// <remarks>

            /// Finding the GameObjects in the Scene
            /// <ul>
            /// <li>Set the Filepath</li>
            /// <li> Path to the File: Application.dataPath + "/StreamingAssets/" + ImportRAWModel.ModelPath + ".txt" </li>
            /// <li>Set the pixelSpacingX, pixelSpacingY , sliceThickness to 0 and MetaInformationText to empty (for resetting when a new Model will be loaded) </li>
            /// <li>Check if the Textfile exists</li>
            /// <li>Create a new StreamReader that reads the Data</li>
            /// <li>The datastructure for the metaInformation-Array which stores the data looks like this: </li>
            /// </ul>   

            /// <table>

            /// <tr>
            /// <th> metaInformation[] </th>
            /// <th> Value </th>

            /// </tr>

            /// <tr>
            /// <td>0</td>
            /// <td>Patientname </td>
            /// </tr>

            /// <tr>
            /// <td>1</td>
            /// <td>Patienname.value</td>
            /// </tr>

            /// <tr>
            /// <td>2</td>
            /// <td>Patientid </td>
            /// </tr>

            /// <tr>
            /// <td>3</td>
            /// <td>Patientid.value</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>4</td>
            /// <td>Patientbirthdate</td>
            /// </tr>

            /// <tr>
            /// <td>5</td>
            /// <td>Patientbirthdate.value</td>
            /// </tr>

            /// <tr>
            /// <td>6</td>
            /// <td>Patientsex</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>7</td>
            /// <td>Patientsex.value</td>
            /// </tr>

            /// <tr>
            /// <td>8</td>
            /// <td>Institutionname</td>
            /// </tr>

            /// <tr>
            /// <td>9</td>
            /// <td>Institutionname.value</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>10</td>
            /// <td>Institutionaddress</td>
            /// </tr>

            /// <tr>
            /// <td>11</td>
            /// <td>Institutionaddress.value</td>
            /// </tr>

            /// <tr>
            /// <td>12</td>
            /// <td>Physicianname</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>13</td>
            /// <td>Physicianname.value</td>
            /// </tr>

            /// <tr>
            /// <td>14</td>
            /// <td>Studydiscription</td>
            /// </tr>

            /// <tr>
            /// <td>15</td>
            /// <td>Studydiscription.value</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>16</td>
            /// <td>Modality</td>
            /// </tr>

            /// <tr>
            /// <td>17</td>
            /// <td>Modality.value</td>
            /// </tr>

            /// <tr>
            /// <td>18</td>
            /// <td>Manufacturer</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>19</td>
            /// <td>Manufacturer.value</td>
            /// </tr>

            /// <tr>
            /// <td>20</td>
            /// <td>Studyid</td>
            /// </tr>

            /// <tr>
            /// <td>21</td>
            /// <td>Studyid.value</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>22</td>
            /// <td>Studydate</td>
            /// </tr>

            /// <tr>
            /// <td>23</td>
            /// <td>Studydate.value</td>
            /// </tr>

            /// <tr>
            /// <td>24</td>
            /// <td>Seriesnumber</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>25</td>
            /// <td>Seriesnumber.value</td>
            /// </tr>

            /// <tr>
            /// <td>26</td>
            /// <td>Pixelspacing in X-Direction</td>
            /// </tr>

            /// <tr>
            /// <td>27</td>
            /// <td>PixelspacingX.value</td>
            /// </tr>
            /// 

            /// <tr>
            /// <td>28</td>
            /// <td>Pixelspacing in Y-Direction</td>
            /// </tr>

            /// <tr>
            /// <td>29</td>
            /// <td>PixelspacingY.value</td>
            /// </tr>

            /// <tr>
            /// <td>30</td>
            /// <td>Slicethickness in mm</td>
            /// </tr>

            /// <tr>
            /// <td>31</td>
            /// <td>Slicethickness.value</td>
            /// </tr>

            /// <tr>
            /// <td>32</td>
            /// <td>Amount of columns of the model</td>
            /// </tr>

            /// <tr>
            /// <td>33</td>
            /// <td>Columns.value</td>
            /// </tr> 

            /// <tr>
            /// <td>34</td>
            /// <td>Amount of rows of the model</td>
            /// </tr>

            /// <tr>
            /// <td>35</td>
            /// <td>Rows.value</td>
            /// </tr>

            /// <tr>
            /// <td>36</td>
            /// <td>Patientposition</td>
            /// </tr>

            /// <tr>
            /// <td>37</td>
            /// <td>Patientposition.value</td>
            /// </tr>

            /// <tr>
            /// <td>38</td>
            /// <td>ImageorientationPatientRowX</td>
            /// </tr>

            /// <tr>
            /// <td>39</td>
            /// <td>ImageorientationPatientRowX.value</td>
            /// </tr>

            /// <tr>
            /// <td>40</td>
            /// <td>ImageorientationPatientRowY</td>
            /// </tr>

            /// <tr>
            /// <td>41</td>
            /// <td>ImageorientationPatientRowX.value</td>
            /// </tr>

            /// <tr>
            /// <td>42</td>
            /// <td>ImageorientationPatientRowZ</td>
            /// </tr>

            /// <tr>
            /// <td>43</td>
            /// <td>ImageorientationPatientRowZ.value</td>
            /// </tr>

            /// <tr>
            /// <td>44</td>
            /// <td>ImageorientationPatientColumnX</td>
            /// </tr>  

            /// <tr>
            /// <td>45</td>
            /// <td>ImageorientationPatientColumnX.value</td>
            /// </tr>

            /// <tr>
            /// <td>46</td>
            /// <td>ImageorientationPatientColumnY</td>
            /// </tr>   

            /// <tr>
            /// <td>47</td>
            /// <td>ImageorientationPatientColumnY.value</td>
            /// </tr>

            /// <tr>
            /// <td>48</td>
            /// <td>ImageorientationPatientColumnZ</td>
            /// </tr> 

            /// <tr>
            /// <td>49</td>
            /// <td>ImageorientationPatientColumnZ.value</td>
            /// </tr>


            /// </table>
            /// <ul>
            /// <li>The Streamwriter reads the datavalues in for-loop and fills them into the array </li>
            /// <li>The strings of  pixelSpacingX, pixelSpacingY and sliceThickness are converted from dot to  comma representation because of the  float parse (american vs european way of writing floats) </li>
            /// <li>Afterwards these values are parsed into floats </li>
            /// </ul>

            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            /// <seealso>
            /// <ul>
            /// <li>Sources:</li>
            /// <li> [1] http://dicom.nema.org/medical/dicom/current/output/chtml/part06/chapter_1.html </li>
            /// <li> [2] http://dicom.nema.org/medical/dicom/current/output/chtml/part06/chapter_6.html </li>
            /// </ul>
            /// </seealso>
            public static void ReadDICOMMetaInformation()
            {
                // The path to the meta information (saved as a text file) based on the model.
                //The model´s path or name is loaded when the user clicks on dropdown element
                // For Example : string path = Application.dataPath + "/StreamingAssets/" + "Male_Head.metainfo" + ".txt";
                //string path = Application.dataPath + "/StreamingAssets/" + ImportRAWModel.ModelPath + ".txt";
                string path = Application.streamingAssetsPath + ImportRAWModel.ModelPath + ".txt";


                // Resets when you change the model
                pixelSpacingX = 0;
                pixelSpacingY = 0;
                sliceThickness = 0;

                if (MetaInformationText is null)
                {
                    MetaInformationText = GameObject.Find("MetaInfoLabel").GetComponent<TMP_Text>();
                }
                
                MetaInformationText.text = "";

                // Check if the model has a corresponding metainfo file
                bool FileValid = IsFileValid(path);

                // When the File exists 
                if (FileValid)
                {
                    //StreamReader reads the meta-information of the text file 
                    StreamReader streamReader = new StreamReader(path);

                    // Safety check
                    if (streamReader != null)
                    {


                        //Displaying additional Information
                        metaInformation[0] = "patientname  :   ";
                        metaInformation[2] = "patientid    :   ";
                        metaInformation[4] = "patientbirthdate :   ";
                        metaInformation[6] = "patientsex   :   ";
                        metaInformation[8] = "institutionname  :   ";
                        metaInformation[10] = "institutionaddress  :   ";
                        metaInformation[12] = "physicianname   :   ";
                        metaInformation[14] = "studydiscription    :   ";
                        metaInformation[16] = "modality    :   ";
                        metaInformation[18] = "manufacturer    :   ";
                        metaInformation[20] = "studyid     :   ";
                        metaInformation[22] = "studydate   :   ";
                        metaInformation[24] = "seriesnumber    :   ";
                        metaInformation[26] = "pixelspacingx    :   ";
                        metaInformation[28] = "pixelspacingy  :   ";
                        metaInformation[30] = "slicethickness  :   ";
                        metaInformation[32] = "columns :   ";
                        metaInformation[34] = "rows    :   ";
                        metaInformation[36] = "patientposition     :   ";
                        metaInformation[38] = "imageorientationpatientrowx     :   ";
                        metaInformation[40] = "imageorientationpatientrowy     :   ";
                        metaInformation[42] = "imageorientationpatientrowz     :   ";
                        metaInformation[44] = "imageorientationpatientcolumnx     :   ";
                        metaInformation[46] = "imageorientationpatientcolumny     :   ";
                        metaInformation[48] = "imageorientationpatientcolumnz     :   ";

                        for (int i = 1; i < metaInformation.Length; i += 2)
                        {
                            metaInformation[i] = streamReader.ReadLine();


                            if (i <= 25)
                            {
                                MetaInformationText.text += metaInformation[i - 1] + metaInformation[i] + "\n";



                            }






                        }


                        //Parsing the incoming DICOM-Data into unity 


                        //Converting  -> dot to  comma because of float parse (american vs european way of writing floats)
                        metaInformation[27] = metaInformation[27].Replace(".", ",");
                        metaInformation[29] = metaInformation[29].Replace(".", ",");
                        metaInformation[31] = metaInformation[31].Replace(".", ",");


                        pixelSpacingX = float.Parse(metaInformation[27]);
                        pixelSpacingY = float.Parse(metaInformation[29]);
                        sliceThickness = float.Parse(metaInformation[31]);



                        MetaInformationText.text += metaInformation[26] + pixelSpacingX + " mm" + "\n"; ;
                        MetaInformationText.text += metaInformation[28] + pixelSpacingY + " mm" + "\n"; ;
                        MetaInformationText.text += metaInformation[30] + sliceThickness + " mm" + "\n"; ;

                        MetaInformationText.fontSize = 70;
                    }

                }
            }



            // Approach
            // The data is structured like a table
            // On the left side is a tag and the name of the value
            // On the right side are some letters and the actual value
            // eg. (0018, 0050) Slice Thickness                     DS: "1.0"
            // 


            // Left column , name eg. : Slice Thickness 


 


            // Getter-Method 
            /// <summary>
            /// Getter-Method for SliceThickness
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>sliceThickness</returns>
            public static float GetThickness()
            {

                return sliceThickness;

            }


            /// <summary>
            /// Getter-Method for pixelSpacingX
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>pixelSpacingX</returns>
            public static float GetPixelSpacingX()
            {

                return pixelSpacingX;

            }



            /// <summary>
            /// Getter-Method for pixelSpacingY
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>pixelSpacingY</returns>
            public static float GetPixelSpacingY()
            {

                return pixelSpacingY;

            }


            /// <summary>
            /// Checks if the File exists or not 
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <param name="filePath"></param>
            /// <returns>isValid</returns>

            /// <seealso>
            /// <ul>
            /// <li>Sources:</li>
            /// <li> [1] https://stackoverflow.com/questions/10260353/working-with-streamreader-and-text-files </li>
            /// </ul>
            /// </seealso>

            private static bool IsFileValid(string filePath)
            {
                bool IsValid = true;

                if (!File.Exists(filePath))
                {
                    IsValid = false;
                }
                else if (Path.GetExtension(filePath).ToLower() != ".txt")
                {
                    IsValid = false;
                }

                return IsValid;
            }


        }

    }
}

