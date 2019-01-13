using UnityEngine;
using KSP.UI.Screens;
using ToolbarControl_NS;

namespace Insight
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ButtonApp : MonoBehaviour
    {
        private static readonly string AppIconPath = $"{"Kunedo/Insight"}/{"button"}";
        private static readonly Texture2D AppIcon = GameDatabase.Instance.GetTexture(AppIconPath, false);
        public bool guiactivada = false;
        private ToolbarControl toolbarControl = null;
        private CamInsight camin = null;
        internal const string MODID = "Insight_NS";
        internal const string MODNAME = "Insight";
        public void OnDisable()
        {
            RemoveLauncher();
        }
        public void Activado()
        {
            AddLauncher();
        }
        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                camin = FindObjectOfType<CamInsight>();
                if(camin == null)
                {
                    if (MuestraUI.CoolUICanvas != null)
                    {
                        MuestraUI.Destroy();
                        guiactivada = false;
                    }
                    OnDisable();
                }
            }
        }
        private void AddLauncher()
        {
            if(toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(OnToggleOn, OnToggleOff, ApplicationLauncher.AppScenes.FLIGHT, MODID, "InsightButton", AppIconPath, AppIconPath, MODNAME);
                toolbarControl.AddLeftRightClickCallbacks(OnLeftClick, OnRightClick);
            }                    
        }
        public void RemoveLauncher()
        {
            if(toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }            
        }
        void OnLeftClick()
        {
            /*if (MuestraUI.CoolUICanvas == null)
            {
                MuestraUI.ShowGUI();
                guiactivada = true;
            }
            else
            {
                FindObjectOfType<CamInsight>().scala = MuestraUI.CoolUICanvas.transform.localScale;
                FindObjectOfType<CamInsight>().posi = MuestraUI.CoolUICanvas.transform.localPosition;
                MuestraUI.Destroy();
                guiactivada = false;
            }*/
        }
        void OnRightClick()
        {
            if (CoolUI2.CoolUICanvas == null)
            {
                CoolUI2.ShowGUI();
            }
            else
            {
                CoolUI2.Destroy();
            }
        }
        private void OnToggleOn()
        {
            MuestraUI.ShowGUI();
            guiactivada = true;                                    
        }
        private void OnToggleOff()
        {
            if (MuestraUI.CoolUICanvas != null)
            {
                FindObjectOfType<CamInsight>().scala = MuestraUI.CoolUICanvas.transform.localScale;
                FindObjectOfType<CamInsight>().posi = MuestraUI.CoolUICanvas.transform.localPosition;
                FindObjectOfType<CamInsight>().SaveZoom();
                MuestraUI.Destroy();
                guiactivada = false;                
            }
        }
        public void Resize(float x)
        {
            MuestraUI.CoolUICanvas.GetComponent<RectTransform>().localScale = new Vector3(x, x, x);
            FindObjectOfType<CamInsight>().scala = new Vector3(x, x, x);
        }
    }
}
