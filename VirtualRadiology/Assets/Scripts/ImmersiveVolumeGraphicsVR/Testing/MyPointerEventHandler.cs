using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using Microsoft.Win32;

namespace TestScripts {
    public class MyPointerEventHandler : MonoBehaviour
     , IPointerEnterHandler
     , IPointerExitHandler
     , IPointerClickHandler
    {



        private Material testmat;
        private Texture2D testtex;
        public GameObject HitMarker;
        public GameObject Target;

        private HashSet<PointerEventData> hovers = new HashSet<PointerEventData>();
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hovers.Add(eventData) && hovers.Count == 1)
            {
                // turn to highlight state
                Debug.Log("Entered ");
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (hovers.Remove(eventData) && hovers.Count == 0)
            {
                // turn to normal state

                Debug.Log("Exit ");

            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.IsViveButton(ControllerButton.Trigger))
            {
                // Vive button triggered!


                Debug.Log("Trigger betätigt");
                //Testtexture
                // testtex = new Texture2D(10, 10); 
                testtex = new Texture2D(64, 64);

                Debug.Log(HitMarker.transform.position.x + "      " + HitMarker.transform.position.y);
                Debug.Log(this.transform.localPosition);

                //  testtex.SetPixel(5,5,Color.red);
                // testtex.SetPixel(5, 7, Color.red);

                //Linear Interpolation
                //Straight line between two points in R^2
                //Point1 (x,y)=(x0,f0) , Point2 (x,y)=(x1,f1) 
                //Formula
                //Unknown Variable X;
                // f(x)= f0 + [ (f1-f0) / (x1-x0) ]  * (X-x0)
                //x0 = 0.85 f0=0, x1=1.15 , f1=1 
                // Input (0.85...1.15) -> Output(0...1)
                // In X-Direction -> Output*Texturewitdh
                // In Y-Direction -> Output*Textureheight
                //Hitpositioncoordinates of the Controllerray will be used to set the Color 



                int x = (int)((1 / 0.3f) * (HitMarker.transform.position.x - 0.85f) * testtex.width);
                int y = (int)((1 / 0.3f) * (HitMarker.transform.position.y - 0.85f) * testtex.height);

                Debug.Log(x);
                Debug.Log(y);
                testtex.SetPixel(x, y, Color.red);

                //   testtex.SetPixel(0, 0, Color.red);
                //  testtex.SetPixel(0, 1, Color.blue);
                //  testtex.SetPixel(1, 0, Color.green);
                testtex.wrapMode = TextureWrapMode.Clamp;
                // testmat.SetTexture("_MainTex",testtex);
                testtex.Apply();

                Target.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", testtex);


            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Standalone button triggered!
            }
        }

        void Start()
        {
            testmat = Resources.Load<Material>("TestMat");

        }
    }
}