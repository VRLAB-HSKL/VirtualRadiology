using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LoadingBarBehaviour : MonoBehaviour
    {
        public GameObject loadingPanel;
        public Slider loadingBar;
        public TextMeshProUGUI loadingText;

        private void Update()
        {
            loadingPanel.SetActive(GlobalDataModel.IsImporting);
            
            //Debug.Log("loading bar update - isImporting: " + GlobalDataModel.IsImporting);
            
            if (GlobalDataModel.IsImporting)
            {
                
                UpdateLoadingBar();
            }
        }
        
        
        public void UpdateLoadingBar ()
        {
            float progress = Mathf.Clamp01(GlobalDataModel.ModelImportProgress); // / .9f);
            //Debug.Log(op.progress);
            loadingBar.value = progress;
            loadingText.text = (progress * 100f).ToString("0") + "%";

            //Debug.Log("updating loading bar - " + progress);
            
            if (progress >= 1f)
            {
                GlobalDataModel.IsImporting = false;
            }
        }

    }
}