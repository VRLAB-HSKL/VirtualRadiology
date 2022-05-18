using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;
using ImmersiveVolumeGraphics.ModelEdit;
using ImmersiveVolumeGraphics.ModelImport;
using ImmersiveVolumeGraphics.Transferfunctions;
using Debug = UnityEngine.Debug;


/// <summary>
/// Main Namespace for the Immersive Volume Graphics Applikation
/// This Namespace uses classes of the UnityVolumeRendering-Namespace
/// </summary>







namespace ImmersiveVolumeGraphics
{

    /// <summary>
    /// This namespace lists all components that are needed for the Import of a 3D-Model
    /// </summary>
    /// 
    namespace ModelImport
    {
        /// <summary>
        /// This Class handles the Import of a 3D-RAW-Model
        /// </summary>


        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] https://github.com/mlavik1/UnityVolumeRendering </li>
        /// <li> [2] UnityVolumeRendering </li>
        /// </ul>
        /// </seealso>


        public class ImportRAWModel : MonoBehaviour
        {

        
            /// <summary>
            /// Name or path of the model
            /// </summary>
            public static string ModelPath = "";

            /// <summary>
            /// Initial data for the modelimport eg. height, width
            /// </summary>
            public static DatasetIniData InitalizationData;

            /// <summary>
            /// Accessible setter-method.  Other scripts can change the modelpath-variable
            /// </summary>
            /// <param name="Path"></param>
            /// <returns> void</returns>
            public static void SetModelPath(string Path)
            {
                ModelPath = Path;

            }

            /// <summary>
            /// Imports a 3D-RAW-Model into the scene by using voice commands 
            /// </summary>
            /// <remarks>
            /// 
            /// <ul>
            /// <li>Find the consolebase and the regulators </li>
            /// <li>Reset the position of the consolebase and the regulators back to their origin when user changes the model changes</li>
            /// <li>Find the EditSlicerenderers (sliders in the regulators)</li>
            /// <li>Find the Rotatable Table</li>
            /// <li> Use the DespawnAllDatasets-Methode:  We'll only allow one dataset and one Model at a time  </li>
            /// <li>Parse the .ini file specifically for the current Model</li>
            /// <li>Import the dataset with Methods from the UnityVolumeRendering-Namespace</li>
            /// <li>Spawn the object</li>
            /// <li>Create the Volume Object</li>
            /// <li>Set the model into the right place of the scene </li>
            /// <li>Rotate the object that it faces us </li>
            /// </ul>

            /// <pre> </pre>

            /// <h3>Calculating the dimensions of the model: </h3>
            /// <h3>Unity doesn't use units for its worldspace but the VR-Environment needs units for the object mapping. </h3>
            /// <h3>1 unit in Unity equals to 1 meter in VR/Real Life </h3>
            /// <h3>Normally the VolumeObject has a default size of 1m x 1m x 1m </h3>
            /// <pre> </pre>
            /// <h3>The Volume Objects size will be adjusted according to the DICOM-Information that we gathered: </h3>
            /// <h3>The scaling in x will be  = (amount of slices in X  * PixelSpacingX ) / 1000  </h3>
            /// <h3>The scaling in y will be  = (amount of slices in Y  * PixelSpacingX ) / 1000  </h3>
            /// <h3>The scaling in z will be  = (amount of slices in Z  * sliceThickness ) / 1000  </h3>

            /// <pre> </pre>

            /// <h3> Remark:</h3>
            /// <h3> The slicethickness is measured  in millimeter but the mapped worldspace is in meter so we have to take the factor 1000 into consideration </h3>
            /// <h3> Every slice has the same thickness </h3>
            /// <h3> The slicethickness can never be 0 except the metainfo file wasnt loaded , default dimensions (scales) are (x,y,z) = (1 meter , 1 meter , 1 meter) </h3>

            /// <pre> </pre>

            /// <ul>
            /// <li>Check if the slicethickness is > 0 (not loaded or no DICOMMeta-File exists)</li>
            /// <li>Set the Scaling of the VolumeObject to the Dimensions that were calculated </li>
            /// <li>Dont set a scaling when the slicethickness is  0 or < 0 (DICOMMeta-File doesnt exist or is not loaded)</li>
            /// <li>Spawn the CrosssectionPlane</li>
            /// <li>Find a quad-Object, rename it to Crossection and disabled its MeshRenderer</li>
            /// <li>Add this Object to a Parent called CrosssectionSelection</li>
            /// <li> Add the VRRotateWithObject-Script to the RotatableTable </li>
            /// <li> Create the three SlicingPlanes for the scene and rotate each of them to fit them to the  x, y and z- plane </li>
            /// <li> Find the three ImageViewers in the scene </li>
            /// <li> Disable the three MeshRenderers of the SlicingPlanes </li>
            /// <li> Set the ImageViewer´s sharedMaterial to the SlicingPlane´s sharedMaterial  </li>
            /// <li> Add the VRRotateWithObject-Script to the EditSliceRenderer-Objects (sliders in the regulators) </li>
            /// </ul>


            /// </remarks>

            /// <param name="void"></param>
            /// <returns>void</returns>
            public static void OpenRAWDataset()
            {

                //Resets for Up+DownButton 
                GameObject consoleBase = GameObject.Find("ConsoleBase");
                GameObject regulator1 = GameObject.Find("Regulator");
                GameObject regulator2 = GameObject.Find("Regulator (1)");
                GameObject regulator3 = GameObject.Find("Regulator (2)");

                consoleBase.transform.localPosition = new Vector3(0, -0.4f, 0);

                regulator1.transform.localPosition = new Vector3(-0.620646f, 0.004025102f, 0.4748505f);
                regulator2.transform.localPosition = new Vector3(0.03635401f, -0.002974927f, 0.9838505f);
                regulator3.transform.localPosition = new Vector3(0.6293541f, 0.00402510f, 0.3688505f);



                //Resets for new Model

                GameObject editSliceRenderer1 = GameObject.Find("EditSliceRenderer1");
                Destroy(editSliceRenderer1.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer2 = GameObject.Find("EditSliceRenderer2");
                Destroy(editSliceRenderer2.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer3 = GameObject.Find("EditSliceRenderer3");
                Destroy(editSliceRenderer3.GetComponent<VRMoveWithObject>());

                GameObject rotTable = GameObject.Find("Rotatable Table");
                Destroy(rotTable.GetComponent<VRRotateWithObject>());



                // We'll only allow one dataset at a time in the runtime GUI (for simplicity)
                DespawnAllDatasets();




                // Did the user try to import an .ini-file? Open the corresponding .raw file instead
                //  string filePath = Application.dataPath + "/StreamingAssets/" ;
                //if (System.IO.Path.GetExtension(filePath) == ".ini")
                //  filePath = filePath.Replace(".ini", ".raw");

                // Parse .ini file
                InitalizationData = DatasetIniReader.ParseIniFile(Application.dataPath + "/StreamingAssets/" + ModelPath + ".ini");
                if (InitalizationData != null)
                {
                    // Import the dataset
                    RawDatasetImporter importer = new RawDatasetImporter(Application.dataPath + "/StreamingAssets/" + ModelPath + ".raw", InitalizationData.dimX, InitalizationData.dimY, InitalizationData.dimZ, InitalizationData.format, InitalizationData.endianness, InitalizationData.bytesToSkip);
                    VolumeDataset dataset = importer.Import();
                    // Spawn the object
                    if (dataset != null)
                    {
                        // Create the Volume Object
                        VolumeObjectFactory.CreateObject(dataset);
                        VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                        // Sets the model into the right place of the scene 
                        volobj.gameObject.transform.position = new Vector3(0, 1.62f, -0.1f);

                        // Rotates the object facing us
                        Vector3 rotation = new Vector3(-90, 90, 0);
                        volobj.gameObject.transform.rotation = Quaternion.Euler(rotation);

                        // Calculating the dimensions of the model 

                        // Unity doesn't use units for its worldspace but the VR-Environment needs units for the object mapping. 
                        // 1 unit in Unity equals to 1 meter in VR/Real Life
                        // normally the VolumeObject has a default size of 1x1x1 

                        // The Volume Objects size will be adjusted according to the DICOM information that we gathered
                        // the scaling in x will be  (amount of slices in X  * slicethickness ) / 1000  
                        // the scaling in y will be  (amount of slices in Y  * slicethickness ) / 1000  
                        // the scaling in z will be  (amount of slices in Z  * slicethickness ) / 1000  

                        // Remark:
                        // the slicethickness is measured  in Millimeter but the mapped Worldspace is in Meter so we have to take the factor 1000 into consideration
                        // the slicethickness is the same for every dimension 
                        //SliceThickness can never be 0! except the metainfo file wasnt loaded , default dimensions (scales) are (x,y,z) = (1 meter , 1 meter , 1 meter)
                        if (DICOMMetaReader.GetThickness() > 0)
                        {



                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX *DICOMMetaReader.getThickness())/ 1000, (initData.dimY * DICOMMetaReader.getThickness()) / 1000, (initData.dimZ * DICOMMetaReader.getThickness()) / 1000);

                            // DICOMMetaReader.getThickness()
                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX * 0.46875f) / 1000, (initData.dimY * 0.46875f) / 1000, (initData.dimZ * 0.46875f) / 1000);

                            volobj.gameObject.transform.localScale = new Vector3((InitalizationData.dimX * DICOMMetaReader.GetPixelSpacingX() * 1.0f) / 1000, (InitalizationData.dimY * 1.0f * DICOMMetaReader.GetPixelSpacingX()) / 1000, (InitalizationData.dimZ * 1.0f * DICOMMetaReader.GetThickness()) / 1000);
                        }

                        VolumeObjectFactory.SpawnCrossSectionPlane(volobj);
                        GameObject quad = GameObject.Find("Quad");
                        quad.name = "CrossSection";

                        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                        meshRenderer.enabled = false;



                        GameObject crosssectionselection = GameObject.Find("CrosssectionSelection");
                        quad.transform.SetParent(crosssectionselection.transform);
                        crosssectionselection.gameObject.transform.position = new Vector3(0.5f, 0.86f, 0.643f);


                        rotTable.AddComponent<VRRotateWithObject>().initObj(rotTable.name, volobj.name, "y");





                        // Finding the ImageViewer
                        //   GameObject SlicingPlane = GameObject.Find("SlicingPlane(Clone)");

                        SlicingPlane SlicingPlane = volobj.CreateSlicingPlane();
                        SlicingPlane.name = "SlicingPlane1";


                        SlicingPlane SlicingPlane2 = volobj.CreateSlicingPlane();
                        SlicingPlane2.name = "SlicingPlane2";
                        Vector3 rot2 = new Vector3(0, 0, 90);
                        SlicingPlane2.transform.localRotation = Quaternion.Euler(rot2);

                        SlicingPlane SlicingPlane3 = volobj.CreateSlicingPlane();
                        SlicingPlane3.name = "SlicingPlane3";
                        Vector3 rot3 = new Vector3(90, 0, 0);
                        SlicingPlane3.transform.localRotation = Quaternion.Euler(rot3);



                        GameObject ImageViewer = GameObject.Find("ImageViewer1");
                        GameObject ImageViewer2 = GameObject.Find("ImageViewer2");
                        GameObject ImageViewer3 = GameObject.Find("ImageViewer3");

                        MeshRenderer SlicerMeshrenderer1 = SlicingPlane.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer1.enabled = false;
                        ImageViewer.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane.GetComponent<MeshRenderer>().sharedMaterial;


                        MeshRenderer SlicerMeshrenderer2 = SlicingPlane2.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer2.enabled = false;
                        ImageViewer2.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane2.GetComponent<MeshRenderer>().sharedMaterial;

                        MeshRenderer SlicerMeshrenderer3 = SlicingPlane3.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer3.enabled = false;
                        ImageViewer3.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane3.GetComponent<MeshRenderer>().sharedMaterial;

                        // Destroy(EditSliceRenderer1.GetComponent<VRMoveWithObject>());

                        //EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        editSliceRenderer1.AddComponent<VRMoveWithObject>().initObj("SlicingPlane1", "EditSliceRenderer1", "z");
                        // EditSliceRenderer2.AddComponent<VRMoveWithObject>().objname= "SlicingPlane2";
                        editSliceRenderer2.AddComponent<VRMoveWithObject>().initObj("SlicingPlane2", "EditSliceRenderer2", "x");
                        // EditSliceRenderer2.GetComponent<VRClampDirection>().start = new Vector3(0, 1.029f, -0.06f);
                        //EditSliceRenderer2.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer2";

                        //EditSliceRenderer3.AddComponent<VRMoveWithObject>().objname = "SlicingPlane3";
                        editSliceRenderer3.AddComponent<VRMoveWithObject>().initObj("SlicingPlane3", "EditSliceRenderer3", "y");
                        //EditSliceRenderer3.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer3";

                        //  EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        // SlicingPlane.transform.SetParent(EditSliceRenderer1.transform);



                    }
                }

            }






            /// <summary>
            /// Imports a 3D-RAW-Model into the scene by using a Button´s OnClickListener 
            /// </summary>
            /// <remarks>
            /// 
            /// <ul>
            /// <li>Find the consolebase and the regulators </li>
            /// <li>Reset the position of the consolebase and the regulators back to their origin when user changes the model changes</li>
            /// <li>Find the EditSlicerenderers (sliders in the regulators)</li>
            /// <li>Find the Rotatable Table</li>
            /// <li> Use the DespawnAllDatasets-Methode:  We'll only allow one dataset and one Model at a time  </li>
            /// <li>Parse the .ini file specifically for the current Model</li>
            /// <li>Import the dataset with Methods from the UnityVolumeRendering-Namespace</li>
            /// <li>Spawn the object</li>
            /// <li>Create the Volume Object</li>
            /// <li>Set the model into the right place of the scene </li>
            /// <li>Rotate the object that it faces us </li>
            /// </ul>

            /// <pre> </pre>

            /// <h3>Calculating the dimensions of the model: </h3>
            /// <h3>Unity doesn't use units for its worldspace but the VR-Environment needs units for the object mapping. </h3>
            /// <h3>1 unit in Unity equals to 1 meter in VR/Real Life </h3>
            /// <h3>Normally the VolumeObject has a default size of 1m x 1m x 1m </h3>
            /// <pre> </pre>
            /// <h3>The Volume Objects size will be adjusted according to the DICOM-Information that we gathered: </h3>
            /// <h3>The scaling in x will be  = (amount of slices in X  * PixelSpacingX ) / 1000  </h3>
            /// <h3>The scaling in y will be  = (amount of slices in Y  * PixelSpacingX ) / 1000  </h3>
            /// <h3>The scaling in z will be  = (amount of slices in Z  * sliceThickness ) / 1000  </h3>

            /// <pre> </pre>

            /// <h3> Remark:</h3>
            /// <h3> The slicethickness is measured  in millimeter but the mapped worldspace is in meter so we have to take the factor 1000 into consideration </h3>
            /// <h3> Every slice has the same thickness </h3>
            /// <h3> The slicethickness can never be 0 except the metainfo file wasnt loaded , default dimensions (scales) are (x,y,z) = (1 meter , 1 meter , 1 meter) </h3>

            /// <pre> </pre>

            /// <ul>
            /// <li>Check if the slicethickness is > 0 (not loaded or no DICOMMeta-File exists)</li>
            /// <li>Set the Scaling of the VolumeObject to the Dimensions that were calculated </li>
            /// <li>Dont set a scaling when the slicethickness is  0 or < 0 (DICOMMeta-File doesnt exist or is not loaded)</li>
            /// <li>Spawn the CrosssectionPlane</li>
            /// <li>Find a quad-Object, rename it to Crossection and disabled its MeshRenderer</li>
            /// <li>Add this Object to a Parent called CrosssectionSelection</li>
            /// <li> Add the VRRotateWithObject-Script to the RotatableTable </li>
            /// <li> Create the three SlicingPlanes for the scene and rotate each of them to fit them to the  x, y and z- plane </li>
            /// <li> Find the three ImageViewers in the scene </li>
            /// <li> Disable the three MeshRenderers of the SlicingPlanes </li>
            /// <li> Set the ImageViewer´s sharedMaterial to the SlicingPlane´s sharedMaterial  </li>
            /// <li> Add the VRRotateWithObject-Script to the EditSliceRenderer-Objects (sliders in the regulators) </li>
            /// </ul>


            /// </remarks>

            /// <param name="void"></param>
            /// <returns>void</returns>


            public void StartOpenRawData()
            {
                StartCoroutine(OpenRawDataRoutine());
            }
            
            public IEnumerator OpenRawDataRoutine()
            {
                //Resets for Up+DownButton 
                GameObject consoleBase = GameObject.Find("ConsoleBase");
                GameObject regulator1 = GameObject.Find("Regulator");
                GameObject regulator2 = GameObject.Find("Regulator (1)");
                GameObject regulator3 = GameObject.Find("Regulator (2)");

                consoleBase.transform.localPosition = new Vector3(0, -0.4f,0);

                regulator1.transform.localPosition = new Vector3(-0.620646f, 0.004025102f, 0.4748505f);
                regulator2.transform.localPosition = new Vector3(0.03635401f, -0.002974927f, 0.9838505f);
                regulator3.transform.localPosition = new Vector3(0.6293541f, 0.00402510f, 0.3688505f);

                

                yield return null;

                //Resets for new Model

                GameObject editSliceRenderer1 = GameObject.Find("EditSliceRenderer1");
                Destroy(editSliceRenderer1.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer2 = GameObject.Find("EditSliceRenderer2");
                Destroy(editSliceRenderer2.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer3 = GameObject.Find("EditSliceRenderer3");
                Destroy(editSliceRenderer3.GetComponent<VRMoveWithObject>());

                GameObject rotTable = GameObject.Find("Rotatable Table");
                Destroy(rotTable.GetComponent<VRRotateWithObject>());

                yield return null;

                // We'll only allow one dataset at a time in the runtime GUI (for simplicity)
                DespawnAllDatasets();

                yield return null;


                // Did the user try to import an .ini-file? Open the corresponding .raw file instead
                //  string filePath = Application.dataPath + "/StreamingAssets/" ;
                //if (System.IO.Path.GetExtension(filePath) == ".ini")
                //  filePath = filePath.Replace(".ini", ".raw");

                // Parse .ini file
                //InitalizationData = DatasetIniReader.ParseIniFile(Application.dataPath + "/StreamingAssets/" + ModelPath + ".ini");
                var initDataPath = Application.streamingAssetsPath + "/" + ModelPath + ".ini";
                InitalizationData = DatasetIniReader.ParseIniFile(initDataPath);
                Debug.Log("initDataPath: " + initDataPath);
                
                yield return null;
                
                if (InitalizationData is null)
                {
                    Debug.Log("INIT DATA IS NULL !");
                }
                
                if (InitalizationData != null)
                {
                    // Import the dataset
                    //RawDatasetImporter importer = new RawDatasetImporter(Application.dataPath + "/StreamingAssets/" + ModelPath + ".raw", InitalizationData.dimX, InitalizationData.dimY, InitalizationData.dimZ, InitalizationData.format, InitalizationData.endianness, InitalizationData.bytesToSkip);
                    RawDatasetImporter importer = new RawDatasetImporter(
                        Application.streamingAssetsPath + "/" + ModelPath + ".raw", 
                        InitalizationData.dimX, InitalizationData.dimY, InitalizationData.dimZ, 
                        InitalizationData.format, InitalizationData.endianness, InitalizationData.bytesToSkip);
                    
                    //VolumeDataset dataset = importer. Import();

                    VolumeDataset dataset = null;
                    //StartCoroutine(importer.ImportRoutine(x => dataset = x));

                    Stopwatch sw = new Stopwatch();
                    StringBuilder sb = new StringBuilder();
                    
                    sw.Start();
                    yield return importer.ImportRoutine(x => dataset = x);
                    sw.Stop();
                    sb.AppendLine("Import_Time: " + sw.ElapsedMilliseconds + " ms");
                    sw.Restart();
                    
                    if (dataset is null)
                    {
                        Debug.Log("DATASET IS NULL !");
                    }
                    
                    
                    
                    // Spawn the object
                    if (dataset != null)
                    {
                        // Create the Volume Object
                        //VolumeObjectFactory.CreateObject(dataset);
                        VolumeRenderedObject vol;
                        yield return StartCoroutine(VolumeObjectFactory.CreateObjectRoutine(dataset, x => vol = x));
                        //VolumeObjectFactory.CreateObject(dataset);
                        
                        sw.Stop();
                        sb.AppendLine("VolumeObjectCreation: " + sw.ElapsedMilliseconds + " ms");
                        sw.Restart();
                        
                        yield return null;
                        
                        
                        VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                        if (volobj is null)
                        {
                            Debug.Log("volobj IS NULL");
                        }
                        
                        
                        // Sets the model into the right place of the scene 
                        volobj.gameObject.transform.position = new Vector3(0, 1.62f, -0.1f);

                        // Rotates the object facing us
                        Vector3 rotation = new Vector3(-90, 90, 0);
                        volobj.gameObject.transform.rotation = Quaternion.Euler(rotation);

                        sw.Stop();
                        sb.AppendLine("VolumeObjectTransformation: " + sw.ElapsedMilliseconds + " ms");
                        sw.Restart();
                        
                        // Calculating the dimensions of the model 

                        // Unity doesn't use units for its worldspace but the VR-Environment needs units for the object mapping. 
                        // 1 unit in Unity equals to 1 meter in VR/Real Life
                        // normally the VolumeObject has a default size of 1x1x1 

                        // The Volume Objects size will be adjusted according to the DICOM information that we gathered
                        // the scaling in x will be  (amount of slices in X  * slicethickness ) / 1000  
                        // the scaling in y will be  (amount of slices in Y  * slicethickness ) / 1000  
                        // the scaling in z will be  (amount of slices in Z  * slicethickness ) / 1000  

                        // Remark:
                        // the slicethickness is measured  in Millimeter but the mapped Worldspace is in Meter so we have to take the factor 1000 into consideration
                        // the slicethickness is the same for every dimension 
                        //SliceThickness can never be 0! except the metainfo file wasnt loaded , default dimensions (scales) are (x,y,z) = (1 meter , 1 meter , 1 meter)
                        if (DICOMMetaReader.GetThickness() > 0)
                        {
                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX *DICOMMetaReader.getThickness())/ 1000, (initData.dimY * DICOMMetaReader.getThickness()) / 1000, (initData.dimZ * DICOMMetaReader.getThickness()) / 1000);

                            // DICOMMetaReader.getThickness()
                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX * 0.46875f) / 1000, (initData.dimY * 0.46875f) / 1000, (initData.dimZ * 0.46875f) / 1000);

                            volobj.gameObject.transform.localScale = new Vector3((InitalizationData.dimX * DICOMMetaReader.GetPixelSpacingX() * 1.0f) / 1000, (InitalizationData.dimY * 1.0f * DICOMMetaReader.GetPixelSpacingX()) / 1000, (InitalizationData.dimZ * 1.0f * DICOMMetaReader.GetThickness()) / 1000);

                            sw.Stop();
                            sb.AppendLine("VolumeObjectThicknessScaling: " + sw.ElapsedMilliseconds + " ms");
                            sw.Restart();
                            
                            //yield return null;
                        }

                        VolumeObjectFactory.SpawnCrossSectionPlane(volobj);
                        GameObject quad = GameObject.Find("Quad");
                        quad.name = "CrossSection";

                        sw.Stop();
                        sb.AppendLine("SpawnCrossSectionPlane: " + sw.ElapsedMilliseconds + " ms");
                        sw.Restart();
                        
                        //yield return null;
                        

                        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                        meshRenderer.enabled = false;



                        GameObject crosssectionselection = GameObject.Find("CrosssectionSelection");
                        quad.transform.SetParent(crosssectionselection.transform);
                        crosssectionselection.gameObject.transform.position = new Vector3(0.5f, 0.86f, 0.643f);


                        //yield return null;
                        
                        rotTable.AddComponent<VRRotateWithObject>().initObj(rotTable.name, volobj.name, "y");





                        // Finding the ImageViewer
                        //   GameObject SlicingPlane = GameObject.Find("SlicingPlane(Clone)");

                        SlicingPlane SlicingPlane = volobj.CreateSlicingPlane();
                        SlicingPlane.name = "SlicingPlane1";

                        //yield return null;

                        SlicingPlane SlicingPlane2 = volobj.CreateSlicingPlane();
                        SlicingPlane2.name = "SlicingPlane2";
                        Vector3 rot2 = new Vector3(0, 0, 90);
                        SlicingPlane2.transform.localRotation = Quaternion.Euler(rot2);

                        //yield return null;
                        
                        SlicingPlane SlicingPlane3 = volobj.CreateSlicingPlane();
                        SlicingPlane3.name = "SlicingPlane3";
                        Vector3 rot3 = new Vector3(90, 0, 0);
                        SlicingPlane3.transform.localRotation = Quaternion.Euler(rot3);

                        //yield return null;

                        GameObject ImageViewer = GameObject.Find("ImageViewer1");
                        GameObject ImageViewer2 = GameObject.Find("ImageViewer2");
                        GameObject ImageViewer3 = GameObject.Find("ImageViewer3");

                        MeshRenderer SlicerMeshrenderer1 = SlicingPlane.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer1.enabled = false;
                        ImageViewer.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane.GetComponent<MeshRenderer>().sharedMaterial;

                        //yield return null;

                        MeshRenderer SlicerMeshrenderer2 = SlicingPlane2.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer2.enabled = false;
                        ImageViewer2.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane2.GetComponent<MeshRenderer>().sharedMaterial;

                        //yield return null;
                        
                        MeshRenderer SlicerMeshrenderer3 = SlicingPlane3.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer3.enabled = false;
                        ImageViewer3.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane3.GetComponent<MeshRenderer>().sharedMaterial;

                        //yield return null;
                        
                        // Destroy(EditSliceRenderer1.GetComponent<VRMoveWithObject>());

                        //EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        editSliceRenderer1.AddComponent<VRMoveWithObject>().initObj("SlicingPlane1", "EditSliceRenderer1", "z");
                        // EditSliceRenderer2.AddComponent<VRMoveWithObject>().objname= "SlicingPlane2";
                        
                        //yield return null;
                        
                        editSliceRenderer2.AddComponent<VRMoveWithObject>().initObj("SlicingPlane2", "EditSliceRenderer2", "x");
                        // EditSliceRenderer2.GetComponent<VRClampDirection>().start = new Vector3(0, 1.029f, -0.06f);
                        //EditSliceRenderer2.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer2";

                        //yield return null;
                        
                        //EditSliceRenderer3.AddComponent<VRMoveWithObject>().objname = "SlicingPlane3";
                        editSliceRenderer3.AddComponent<VRMoveWithObject>().initObj("SlicingPlane3", "EditSliceRenderer3", "y");
                        //EditSliceRenderer3.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer3";

                        //  EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        // SlicingPlane.transform.SetParent(EditSliceRenderer1.transform);


                        Debug.Log(sb);
                    }
                }
            }

            public void OpenRAWData()
            {

                //Resets for Up+DownButton 
                GameObject consoleBase = GameObject.Find("ConsoleBase");
                GameObject regulator1 = GameObject.Find("Regulator");
                GameObject regulator2 = GameObject.Find("Regulator (1)");
                GameObject regulator3 = GameObject.Find("Regulator (2)");

                consoleBase.transform.localPosition = new Vector3(0, -0.4f,0);

                regulator1.transform.localPosition = new Vector3(-0.620646f, 0.004025102f, 0.4748505f);
                regulator2.transform.localPosition = new Vector3(0.03635401f, -0.002974927f, 0.9838505f);
                regulator3.transform.localPosition = new Vector3(0.6293541f, 0.00402510f, 0.3688505f);



                //Resets for new Model

                GameObject editSliceRenderer1 = GameObject.Find("EditSliceRenderer1");
                Destroy(editSliceRenderer1.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer2 = GameObject.Find("EditSliceRenderer2");
                Destroy(editSliceRenderer2.GetComponent<VRMoveWithObject>());

                GameObject editSliceRenderer3 = GameObject.Find("EditSliceRenderer3");
                Destroy(editSliceRenderer3.GetComponent<VRMoveWithObject>());

                GameObject rotTable = GameObject.Find("Rotatable Table");
                Destroy(rotTable.GetComponent<VRRotateWithObject>());



                // We'll only allow one dataset at a time in the runtime GUI (for simplicity)
                DespawnAllDatasets();




                // Did the user try to import an .ini-file? Open the corresponding .raw file instead
                //  string filePath = Application.dataPath + "/StreamingAssets/" ;
                //if (System.IO.Path.GetExtension(filePath) == ".ini")
                //  filePath = filePath.Replace(".ini", ".raw");

                // Parse .ini file
                //InitalizationData = DatasetIniReader.ParseIniFile(Application.dataPath + "/StreamingAssets/" + ModelPath + ".ini");
                var initDataPath = Application.streamingAssetsPath + "/" + ModelPath + ".ini";
                InitalizationData = DatasetIniReader.ParseIniFile(initDataPath);
                Debug.Log("initDataPath: " + initDataPath);
                
                if (InitalizationData != null)
                {
                    //Debug.Log("Test");    
                    
                    // Import the dataset
                    //RawDatasetImporter importer = new RawDatasetImporter(Application.dataPath + "/StreamingAssets/" + ModelPath + ".raw", InitalizationData.dimX, InitalizationData.dimY, InitalizationData.dimZ, InitalizationData.format, InitalizationData.endianness, InitalizationData.bytesToSkip);
                    RawDatasetImporter importer = new RawDatasetImporter(
                        Application.streamingAssetsPath + "/" + ModelPath + ".raw", 
                        InitalizationData.dimX, InitalizationData.dimY, InitalizationData.dimZ, 
                        InitalizationData.format, InitalizationData.endianness, InitalizationData.bytesToSkip);
                    
                    VolumeDataset dataset = importer. Import();
                    
                    // Spawn the object
                    if (dataset != null)
                    {
                        // Create the Volume Object
                        VolumeObjectFactory.CreateObject(dataset);
                        VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                        // Sets the model into the right place of the scene 
                        volobj.gameObject.transform.position = new Vector3(0, 1.62f, -0.1f);

                        // Rotates the object facing us
                        Vector3 rotation = new Vector3(-90, 90, 0);
                        volobj.gameObject.transform.rotation = Quaternion.Euler(rotation);

                        // Calculating the dimensions of the model 

                        // Unity doesn't use units for its worldspace but the VR-Environment needs units for the object mapping. 
                        // 1 unit in Unity equals to 1 meter in VR/Real Life
                        // normally the VolumeObject has a default size of 1x1x1 

                        // The Volume Objects size will be adjusted according to the DICOM information that we gathered
                        // the scaling in x will be  (amount of slices in X  * slicethickness ) / 1000  
                        // the scaling in y will be  (amount of slices in Y  * slicethickness ) / 1000  
                        // the scaling in z will be  (amount of slices in Z  * slicethickness ) / 1000  

                        // Remark:
                        // the slicethickness is measured  in Millimeter but the mapped Worldspace is in Meter so we have to take the factor 1000 into consideration
                        // the slicethickness is the same for every dimension 
                        //SliceThickness can never be 0! except the metainfo file wasnt loaded , default dimensions (scales) are (x,y,z) = (1 meter , 1 meter , 1 meter)
                        if (DICOMMetaReader.GetThickness() > 0)
                        {



                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX *DICOMMetaReader.getThickness())/ 1000, (initData.dimY * DICOMMetaReader.getThickness()) / 1000, (initData.dimZ * DICOMMetaReader.getThickness()) / 1000);

                            // DICOMMetaReader.getThickness()
                            // volobj.gameObject.transform.localScale = new Vector3((initData.dimX * 0.46875f) / 1000, (initData.dimY * 0.46875f) / 1000, (initData.dimZ * 0.46875f) / 1000);

                            volobj.gameObject.transform.localScale = new Vector3((InitalizationData.dimX * DICOMMetaReader.GetPixelSpacingX() * 1.0f) / 1000, (InitalizationData.dimY * 1.0f * DICOMMetaReader.GetPixelSpacingX()) / 1000, (InitalizationData.dimZ * 1.0f * DICOMMetaReader.GetThickness()) / 1000);
                        }

                        VolumeObjectFactory.SpawnCrossSectionPlane(volobj);
                        GameObject quad = GameObject.Find("Quad");
                        quad.name = "CrossSection";

                        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                        meshRenderer.enabled = false;



                        GameObject crosssectionselection = GameObject.Find("CrosssectionSelection");
                        quad.transform.SetParent(crosssectionselection.transform);
                        crosssectionselection.gameObject.transform.position = new Vector3(0.5f, 0.86f, 0.643f);


                        rotTable.AddComponent<VRRotateWithObject>().initObj(rotTable.name, volobj.name, "y");





                        // Finding the ImageViewer
                        //   GameObject SlicingPlane = GameObject.Find("SlicingPlane(Clone)");

                        SlicingPlane SlicingPlane = volobj.CreateSlicingPlane();
                        SlicingPlane.name = "SlicingPlane1";


                        SlicingPlane SlicingPlane2 = volobj.CreateSlicingPlane();
                        SlicingPlane2.name = "SlicingPlane2";
                        Vector3 rot2 = new Vector3(0, 0, 90);
                        SlicingPlane2.transform.localRotation = Quaternion.Euler(rot2);

                        SlicingPlane SlicingPlane3 = volobj.CreateSlicingPlane();
                        SlicingPlane3.name = "SlicingPlane3";
                        Vector3 rot3 = new Vector3(90, 0, 0);
                        SlicingPlane3.transform.localRotation = Quaternion.Euler(rot3);



                        GameObject ImageViewer = GameObject.Find("ImageViewer1");
                        GameObject ImageViewer2 = GameObject.Find("ImageViewer2");
                        GameObject ImageViewer3 = GameObject.Find("ImageViewer3");

                        MeshRenderer SlicerMeshrenderer1 = SlicingPlane.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer1.enabled = false;
                        ImageViewer.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane.GetComponent<MeshRenderer>().sharedMaterial;


                        MeshRenderer SlicerMeshrenderer2 = SlicingPlane2.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer2.enabled = false;
                        ImageViewer2.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane2.GetComponent<MeshRenderer>().sharedMaterial;

                        MeshRenderer SlicerMeshrenderer3 = SlicingPlane3.GetComponent<MeshRenderer>();
                        SlicerMeshrenderer3.enabled = false;
                        ImageViewer3.GetComponent<MeshRenderer>().sharedMaterial = SlicingPlane3.GetComponent<MeshRenderer>().sharedMaterial;

                        // Destroy(EditSliceRenderer1.GetComponent<VRMoveWithObject>());

                        //EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        editSliceRenderer1.AddComponent<VRMoveWithObject>().initObj("SlicingPlane1", "EditSliceRenderer1", "z");
                        // EditSliceRenderer2.AddComponent<VRMoveWithObject>().objname= "SlicingPlane2";
                        editSliceRenderer2.AddComponent<VRMoveWithObject>().initObj("SlicingPlane2", "EditSliceRenderer2", "x");
                        // EditSliceRenderer2.GetComponent<VRClampDirection>().start = new Vector3(0, 1.029f, -0.06f);
                        //EditSliceRenderer2.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer2";

                        //EditSliceRenderer3.AddComponent<VRMoveWithObject>().objname = "SlicingPlane3";
                        editSliceRenderer3.AddComponent<VRMoveWithObject>().initObj("SlicingPlane3", "EditSliceRenderer3", "y");
                        //EditSliceRenderer3.GetComponent<VRMoveWithObject>().objname2 = "EditSliceRenderer3";

                        //  EditSliceRenderer1.AddComponent<VRMoveWithObject>();
                        // SlicingPlane.transform.SetParent(EditSliceRenderer1.transform);



                    }
                }

            }


            /// <summary>
            ///  Clears the current VolumeRenderedObject, CrossSection and SlicingPlane
            /// </summary>
            /// <remarks>
            /// <h3> Only ONE VolumeRenderedObject is allowed at anytime because of the high GPU Load  </h3>
            /// <ul>
            /// <li>Find the Objects</li>
            /// <li>Destroy them</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            /// 

            /// <seealso>
            /// <ul>
            /// <li>Sources:</li>
            /// <li> [1] UnityVolumeRendering</li>
            /// </ul>
            /// </seealso>


            private static void DespawnAllDatasets()
            {
             


                VolumeRenderedObject[] volobjs = GameObject.FindObjectsOfType<VolumeRenderedObject>();
                foreach (VolumeRenderedObject volobj in volobjs)
                {
                    GameObject.Destroy(volobj.gameObject);

                }

                Object crosssection = GameObject.Find("CrossSection");
                GameObject.Destroy(crosssection);

                Object slicingplane = GameObject.Find("SlicingPlane(Clone)");
                GameObject.Destroy(slicingplane);





            }

        }
    }
}