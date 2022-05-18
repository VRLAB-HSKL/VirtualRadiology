using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveVolumeGraphics
{



    /// <summary>
    /// This class represents the MainMenu for Interaction by clicking Buttons the user can get to submenus
    /// <ul>
    /// <li>Status 0 : MainMenu </li>
    /// <li>Status 1: ModelImport-Menu</li>
    /// <li>Status 2: ModelEdit-Menu</li>
    /// <li>Status 3: Transferfunctions-Menu</li>
    /// <li>Status 4: Recorder-Menu</li>
    /// <li>Status 5: Dashboard-Menu</li>
    /// <li>Status 6: About-Menu </li>
    /// </ul>
    /// </summary>
    public class MainMenu : MonoBehaviour
        {
            //MainMenu              | Status 0
            /// <summary>
            /// This Button shows the ModelImport-Menu
            /// </summary>
            public Button ModelImportButton;
            /// <summary>
            /// This Button shows the ModelEdit-Menu
            /// </summary>
            public Button ModelEditButton;
        /// <summary>
        /// This Button shows the Transferfunctions-Menu
        /// </summary>
            public Button TransferfunctionButton;
            /// <summary>
            /// This Button shows the Recorder-Menu
            /// </summary>
            public Button RecorderButton;
            /// <summary>
            /// This Button shows the Dashboard-Menu
            /// </summary>
            public Button DashboardButton;
            /// <summary>
            /// This Button shows the About-Menu
            /// </summary>
            public Button AboutButton;

        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the MainMenu
        /// </summary>
        public GameObject MainMenuParent;

        //ModelImport           | Status 1
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the ModelImportmenu
        /// </summary>
        public GameObject ModelImportParent;
        //ModelEdit             | Status 2
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the ModelEditmenu
        /// </summary>
        public GameObject ModelEditParent;
        //Transferfunction      | Status 3
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the Transferfunctionmenu
        /// </summary>
        public GameObject TransferfunctionParent;
        //Recorder              | Status 4
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the Recordermenu
        /// </summary>
        public GameObject RecorderParent;
        //Dashboard             | Status 5
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the Dashboardmenu
        /// </summary>
        public GameObject DashboardParent;
        //Info / About          | Status 6
        /// <summary>
        /// This empty GameObject(Parent) containts every Object (Children) of the Aboutmenu
        /// </summary>
        public GameObject AboutParent;


        //Backbutton to get back to the MainMenu
        /// <summary>
        /// This empty GameObject(Parent) containts the backbtn(Child)
        /// </summary>
        public GameObject BackButtonParent;
        /// <summary>
        /// Button to get back to the MainMenu
        /// </summary>
        public Button BackButton;
        /// <summary>
        /// This Integer represents the currently visible Menu
        /// </summary>
        public static int Status = 0;




        /// <summary>
        /// The Start-Method adds the OnClickListeners(ToStatus) to the corresponding Buttons
        /// </summary>
        /// <remarks>
        /// 
        /// <ul>
        /// <li>The ModelImportButton      adds ToStatus1</li>
        /// <li>The ModelEditButton        adds ToStatus2</li>
        /// <li>The TransferfunctionButton adds ToStatus3</li>
        /// <li>The RecorderButton         adds ToStatus4</li>
        /// <li>The DashboardButton        adds ToStatus5</li>
        /// <li>The AboutButton            adds ToStatus6</li>
        /// <li>The BackButton             adds ToStatus0</li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        private void Start()
            {

                //Add Listeners 

                ModelImportButton.onClick.AddListener(ToStatus1);
                ModelEditButton.onClick.AddListener(ToStatus2);
                TransferfunctionButton.onClick.AddListener(ToStatus3);
                RecorderButton.onClick.AddListener(ToStatus4);
                DashboardButton.onClick.AddListener(ToStatus5);
                AboutButton.onClick.AddListener(ToStatus6);
                BackButton.onClick.AddListener(ToStatus0);





            }

        //MainMenu  
        /// <summary>
        ///  OnClickListener to return to the MainMenu
        /// </summary>
        /// <remarks>
        /// When a Menu is active it will be set to inactive: 
        /// <ul>
        /// <li>Deactivate the BackButton </li>
        /// <li>Deactivate the ModelImportMenu </li>
        /// <li>Deactivate the ModelEditMenu   </li>
        /// <li>Deactivate the TransferfunctionMenu </li>
        /// <li>Deactivate the RecorderMenu </li>
        /// <li>Deactivate the DashboardMenu</li>
        /// <li>Deactivate the AboutMenu</li>
        /// <li>Activate the MainMenu</li>
        /// <li>Set the Status to 0</li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>



        public void ToStatus0()
            {
                
                BackButtonParent.SetActive(false);

                if (ModelImportParent.active == true) { ModelImportParent.SetActive(false); }
                if (ModelEditParent.active == true) { ModelEditParent.SetActive(false); }
                if (TransferfunctionParent.active == true) { TransferfunctionParent.SetActive(false); }
                if (RecorderParent.active == true) { RecorderParent.SetActive(false); }
                if (DashboardParent.active == true) { DashboardParent.SetActive(false); }
                if (AboutParent.active == true) { AboutParent.SetActive(false); }

                MainMenuParent.SetActive(true);

                Status = 0;
            }

        //ModelImport 
      
        /// <summary>
        ///  OnClickListener to return to the ModelImportMenu
        /// </summary>
        /// <remarks>
        /// When the ModelImportButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the ModelImportMenu </li>
        /// <li>Set Status to 1 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        public void ToStatus1()
            {

            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            ModelImportParent.SetActive(true);

            Status = 1;

            }

        /// <summary>
        ///  OnClickListener to return to the ModelEditMenu
        /// </summary>
        /// <remarks>
        /// When the ModelEditButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the ModelEditMenu </li>
        /// <li>Set Status to 2 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>


        public void ToStatus2()
            {
            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            ModelEditParent.SetActive(true);

                Status = 2;
            }


        /// <summary>
        ///  OnClickListener to return to the TransferfunctionMenu
        /// </summary>
        /// <remarks>
        /// When the  TransferfunctionButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the TransferfunctionMenu </li>
        /// <li>Set Status to 3 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        public void ToStatus3()
            {
            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            TransferfunctionParent.SetActive(true);

                Status = 3;

            }
        //Recorder 



        /// <summary>
        ///  OnClickListener to return to the RecorderMenu
        /// </summary>
        /// <remarks>
        /// When the RecorderButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the RecorderMenu </li>
        /// <li>Set Status to 4 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        public void ToStatus4()
            {
            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            RecorderParent.SetActive(true);

                Status = 4;
            }
        //Dashboard  



        /// <summary>
        ///  OnClickListener to return to the DashboardMenu
        /// </summary>
        /// <remarks>
        /// When the DashboardButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the DashboardMenu </li>
        /// <li>Set Status to 5 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        public void ToStatus5()
            {
            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            DashboardParent.SetActive(true);

                Status = 5;
            }

        //Info / About


        /// <summary>
        ///  OnClickListener to return to the AboutMenu
        /// </summary>
        /// <remarks>
        /// When the AboutButton on the MainMenu is pressed:
        /// <ul>
        /// <li>Deactivate the MainMenu </li>
        /// <li>Activate the BackButton  </li>
        /// <li>Activate the AboutMenu </li>
        /// <li>Set Status to 6 </li>
        /// </ul> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>

        public void ToStatus6()
            {
            MainMenuParent.SetActive(false);
            BackButtonParent.SetActive(true);
            AboutParent.SetActive(true);

                Status = 6;

            }




        }



    }

