using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using KSP.UI.Screens;

namespace Insight
{
    //[KSPAddon(KSPAddon.Startup.Flight, true)]
    public class CoolUI2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject CoolUICanvas = null;
        public static GameObject slide = null;
        public static Slider slider = null;
        private Vector2 dragstart;
        private Vector2 altstart;        
        private void Awake()
        {
            GameEvents.onGameSceneSwitchRequested.Add(OnSceneChange);            
        }        
        void OnSceneChange(GameEvents.FromToAction<GameScenes, GameScenes> fromToScenes)
        {
            if (CoolUICanvas != null)
            {
                Destroy();
            }
        }
        public static void Destroy()
        {
            CoolUICanvas.DestroyGameObject();
            CoolUICanvas = null;            
        }
        public static void ShowGUI()
        {
            if (CoolUICanvas != null)  //if the UI is already showing, don't show another one.
                return;
            CoolUICanvas = Instantiate(CoolUILoader.PanelPrefab2);
            CoolUICanvas.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            CoolUICanvas.AddComponent<CoolUI2>();
            slide = CoolUICanvas.GetChild("Slider");
            slider = slide.GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { O﻿nToggl﻿eClicked(slider.value); });
        } 
        static void OnToggleClicked(float value)
        {
            if (MuestraUI.CoolUICanvas != null)
            {
                MuestraUI.CoolUICanvas.transform.localScale = new Vector3(value, value, value);
            }
            else
            {
                ScreenMessages.PostScreenMessage("First open gui with left click to scale gui", 3f, ScreenMessageStyle.UPPER_CENTER);
            }
        }
        public void OnBeginDrag(PointerEventData data)
        {
            dragstart = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
            altstart = CoolUICanvas.transform.position;
        }
        public void OnDrag(PointerEventData data)
        {
            Vector2 dpos = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
            Vector2 dragdist = dpos - dragstart;
            CoolUICanvas.transform.position = altstart + dragdist;
        }
        public void OnEndDrag(PointerEventData data)
        {
            
        }        
    }
}
