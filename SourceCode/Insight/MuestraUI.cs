using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Insight
{
    public class MuestraUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject CoolUICanvas = null;
        public static GameObject CoolUIText = null;
        public static GameObject CoolUIText2 = null;
        public static GameObject checkToggle = null;
        public static GameObject checkToggle2 = null;
        //public static GameObject checkTogglesky = null;
        public static GameObject Rawimage = null;
        public static GameObject Rawimage2 = null;
        public static GameObject Rawimage11 = null;
        public static GameObject Rawimage12 = null;
        public static GameObject Rawimage22 = null;
        public static GameObject Rawimage23 = null;
        public static GameObject slide = null;
        public static GameObject slide2 = null;
        public static GameObject butonup = null;
        public static GameObject butondown = null;
        public static GameObject butonleft = null;
        public static GameObject butonright = null;
        public static Button ButonUp = null;
        public static Button ButonDown = null;
        public static Button ButonLeft = null;
        public static Button ButonRight = null;
        public static Toggle toggleButton = null;
        public static Toggle toggleButton2 = null;
        //public static Toggle toggleButtonsky = null;
        public static RawImage rawImage = null;
        public static RawImage rawImage2 = null;
        public static RawImage rawImage11 = null;
        public static RawImage rawImage12 = null;
        public static RawImage rawImage22 = null;
        public static RawImage rawImage23 = null;
        public bool estaon = false;
        public static Slider slider = null;
        public float valor;
        public static Slider slider2 = null;
        public float valor2;
        public bool estaon2 = false;
        private Vector2 dragstart;
        private Vector2 altstart;
        void Awake()
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
        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (toggleButton != null)
                {
                    estaon = toggleButton.isOn;
                }
                if (toggleButton2 != null)
                {
                    estaon2 = toggleButton2.isOn;
                }                
                FindObjectOfType<CamInsight>().encam1 = estaon;
                FindObjectOfType<CamInsight>().encam2 = estaon2;                
            }
        }
        public static void ShowGUI()
        {
            if (CoolUICanvas != null)
                return;
            CoolUICanvas = Instantiate(CoolUILoader.PanelPrefab);
            CoolUICanvas.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            CoolUICanvas.AddComponent<MuestraUI>();
            if (FindObjectOfType<CamInsight>().posi != Vector3.zero)
            {
                CoolUICanvas.transform.localPosition = FindObjectOfType<CamInsight>().posi;
            }
            if (FindObjectOfType<CamInsight>().scala != Vector3.zero)
            {
                CoolUICanvas.transform.localScale = FindObjectOfType<CamInsight>().scala;
            }
            CoolUIText = CoolUICanvas.GetChild("ImportantText");
            CoolUIText2 = CoolUICanvas.GetChild("ImportantText2");
            slide = CoolUICanvas.GetChild("SlideInsightCam");
            slider = slide.GetComponent<Slider>();
            if (FindObjectOfType<CamInsight>().zoom1 != 0)
            {
                slider.value = FindObjectOfType<CamInsight>().zoom1;
            }
            slide2 = CoolUICanvas.GetChild("SlideInsightCam2");
            slider2 = slide2.GetComponent<Slider>();
            if (FindObjectOfType<CamInsight>().zoom2 != 0)
            {
                slider2.value = FindObjectOfType<CamInsight>().zoom2;
            }
            Rawimage = CoolUICanvas.GetChild("RawImagecamera");
            rawImage = Rawimage.GetComponent<RawImage>();
            Rawimage2 = CoolUICanvas.GetChild("RawImagecamera2");
            rawImage2 = Rawimage2.GetComponent<RawImage>();
            Rawimage11 = CoolUICanvas.GetChild("RawImagecamera-power");
            rawImage11 = Rawimage11.GetComponent<RawImage>();
            Rawimage12 = CoolUICanvas.GetChild("RawImagecamera-signal");
            rawImage12 = Rawimage12.GetComponent<RawImage>();
            Rawimage22 = CoolUICanvas.GetChild("RawImagecamera2-power");
            rawImage22 = Rawimage22.GetComponent<RawImage>();
            Rawimage23 = CoolUICanvas.GetChild("RawImagecamera2-signal");
            rawImage23 = Rawimage23.GetComponent<RawImage>();
            checkToggle = CoolUICanvas.GetChild("ToggleInsight");
            toggleButton = checkToggle.GetComponent<Toggle>();
            checkToggle2 = CoolUICanvas.GetChild("ToggleArm");
            toggleButton2 = checkToggle2.GetComponent<Toggle>();
            toggleButton.isOn = false;
            toggleButton2.isOn = false;
            //checkTogglesky = CoolUICanvas.GetChild("ToggleSky");
            //toggleButtonsky = checkTogglesky.GetComponent<Toggle>();
            //toggleButtonsky.isOn = false;
            butonup = CoolUICanvas.GetChild("ButtonUp");
            ButonUp = butonup.GetComponent<Button>();
            butondown = CoolUICanvas.GetChild("ButtonDown");
            ButonDown = butondown.GetComponent<Button>();
            butonleft = CoolUICanvas.GetChild("ButtonLeft");
            ButonLeft = butonleft.GetComponent<Button>();
            butonright = CoolUICanvas.GetChild("ButtonRight");
            ButonRight = butonright.GetComponent<Button>();
            ButonUp.gameObject.AddComponent<ButtonCallback>();
            ButonDown.gameObject.AddComponent<ButtonCallback>();
            ButonLeft.gameObject.AddComponent<ButtonCallback>();
            ButonRight.gameObject.AddComponent<ButtonCallback>();           
        }        
        static void OnClickUp()
        {
            if(FindObjectOfType<CamInsight>() != null)
            {
                FindObjectOfType<CamInsight>().OnUp();
            }
        }
        static void OnClickDown()
        {
            if (FindObjectOfType<CamInsight>() != null)
            {
                FindObjectOfType<CamInsight>().OnDown();
            }
        }
        static void OnClickLeft()
        {
            if (FindObjectOfType<CamInsight>() != null)
            {
                FindObjectOfType<CamInsight>().OnLeft();
            }
        }
        static void OnClickRight()
        {
            if (FindObjectOfType<CamInsight>() != null)
            {
                FindObjectOfType<CamInsight>().OnRight();
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
            FindObjectOfType<CamInsight>().posi = CoolUICanvas.transform.localPosition;
        }
        public void CheckFov(Camera cam)
        {
            if (slider != null)
            {
                valor = slider.value;
                cam.fieldOfView = valor;
            }
        }
        public void CheckFov2(Camera cam, Camera cam2)
        {
            if (slider2 != null)
            {
                valor2 = slider2.value;
                cam.fieldOfView = valor2;
                cam2.fieldOfView = valor2;
            }
        }
        public static void UpdateText(string message)
        {
            if (CoolUIText != null)
            {
                CoolUIText.GetComponent<Text>().text = message;
            }
        }
        public static void UpdateText2(string message)
        {
            if (CoolUIText2 != null)
            {
                CoolUIText2.GetComponent<Text>().text = message;
            }
        }
    }
}
