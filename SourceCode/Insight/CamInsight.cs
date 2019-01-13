using UnityEngine;

namespace Insight
{
    public class CamInsight : PartModule
    {
        public static Transform camer = null;
        public static Transform camerota = null;
        public static Transform camerarm1 = null;
        public static Transform camerarm2 = null;
        public Camera _camera = null;
        public Camera _cameraarm1 = null;
        public Camera _cameraarm2 = null;
        public bool estaactivo = false;
        public bool encam1 = false;
        public bool encam2 = false;
        public Vector3 posi = Vector3.zero;
        public Vector3 scala = Vector3.zero;
        public float zoom1 = 0;
        public float zoom2 = 0;
        public PartResourceList resour = null;
        public MuestraUI bapp = null;
        public ButtonApp bapp2 = null;
        [KSPField(isPersistant = false)]
        public float rate;
        [KSPField(isPersistant = false)]
        public float ratemove;
        [KSPField(isPersistant = false)]
        public string resourceName;
        public float anglex = 0;
        public float anglez = 0;
        [KSPField(isPersistant = false)]
        public float minangle;
        [KSPField(isPersistant = false)]
        public float maxangle;
        public bool moviendo = false;
        public bool conelectricidad = false;
        public bool concomunicacion = false;
        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            var cullmix = (1 << 0) | (1 << 4) | (1 << 9) | (1 << 10) | (1 << 15) | (1 << 19) | (1 << 22) | (1 << 23) | (1 << 24) | (1 << 29) | (1 << 30);
            camer = part.FindModelTransform("CameraICC");
            camerota = part.FindModelTransform("pivotcameraicc");
            _camera = camer.GetComponent<Camera>();
            _camera.gameObject.AddComponent<FlareLayer>();
            _camera.gameObject.AddComponent<UnderwaterFog>();
            _camera.cullingMask = cullmix;
            camerarm1 = part.FindModelTransform("CameraArm1");
            _cameraarm1 = camerarm1.GetComponent<Camera>();
            _cameraarm1.gameObject.AddComponent<FlareLayer>();
            _cameraarm1.gameObject.AddComponent<UnderwaterFog>();
            _cameraarm1.cullingMask = cullmix;
            camerarm2 = part.FindModelTransform("CameraArm2");
            _cameraarm2 = camerarm2.GetComponent<Camera>();
            _cameraarm2.gameObject.AddComponent<FlareLayer>();
            _cameraarm2.gameObject.AddComponent<UnderwaterFog>();
            _cameraarm2.cullingMask = cullmix;
            resour = part.Resources;
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (bapp2 == null)
                {
                    bapp2 = FindObjectOfType<ButtonApp>();
                    bapp2.Activado();
                }
            }
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {                
                if (bapp2 == null)
                {
                    bapp2 = FindObjectOfType<ButtonApp>();
                    bapp2.Activado();
                }
                foreach (PartResource reso in resour)
                {
                    if (reso.resourceName == resourceName)
                    {
                        if (reso.amount <= 0)
                        {
                            conelectricidad = false;
                        }
                        else
                        {
                            conelectricidad = true;
                            if (CommNet.VesselControlState.ProbeNone == vessel.connection.ControlState)
                            {
                                concomunicacion = false;
                            }
                            else
                            {
                                concomunicacion = true;
                            }
                        }
                    }
                }                               
                estaactivo = bapp2.guiactivada;
                if (estaactivo)
                {
                    if (bapp == null)
                    {
                        bapp = FindObjectOfType<MuestraUI>();
                    }
                    
                    anglex = Mathf.Clamp(anglex, minangle, maxangle);
                    anglez = Mathf.Clamp(anglez, minangle, maxangle);
                    if (!conelectricidad)
                    {
                        NoPower();
                    }
                    if (conelectricidad)
                    {                        
                        if (!concomunicacion)
                        {
                            NoSignal();
                        }
                        if (concomunicacion)
                        {
                            WithPower();
                        }                        
                    }                                        
                }                
            }
        }
        void WithPower()
        {
            bapp.CheckFov2(_cameraarm1, _cameraarm2);
            bapp.CheckFov(_camera);
            float shininess = Mathf.PingPong(Time.time * 5, 3f);
            MuestraUI.rawImage2.material.SetFloat("_NoiseOffset", shininess);
            MuestraUI.rawImage.material.SetFloat("_NoiseOffset", shininess);
            if (encam2)
            {
                _cameraarm1.enabled = true;
                _cameraarm2.enabled = true;
                _cameraarm1.targetTexture = (RenderTexture)MuestraUI.rawImage2.texture;
                _cameraarm2.targetTexture = (RenderTexture)MuestraUI.rawImage2.texture;
                MuestraUI.rawImage2.enabled = true;
                MuestraUI.rawImage22.enabled = false;
                MuestraUI.rawImage23.enabled = false;
                MuestraUI.UpdateText2("Camera Arm (ON)");
                //return;
            }
            if (!encam2)
            {
                _cameraarm1.targetTexture = null;
                _cameraarm2.targetTexture = null;
                _cameraarm1.enabled = false;
                _cameraarm2.enabled = false;
                MuestraUI.rawImage2.enabled = false;
                MuestraUI.rawImage22.enabled = true;
                MuestraUI.rawImage23.enabled = false;
                MuestraUI.UpdateText2("Camera Arm (OFF)");
                //return;
            }
            if (encam1)
            {
                _camera.enabled = true;
                _camera.targetTexture = (RenderTexture)MuestraUI.rawImage.texture;
                MuestraUI.rawImage.enabled = true;
                MuestraUI.rawImage11.enabled = false;
                MuestraUI.rawImage12.enabled = false;
                MuestraUI.UpdateText("Camera Insight (ON)");
                //return;
            }
            if (!encam1)
            {
                _camera.targetTexture = null;
                _camera.enabled = false;
                MuestraUI.rawImage.enabled = false;
                MuestraUI.rawImage11.enabled = true;
                MuestraUI.rawImage12.enabled = false;
                MuestraUI.UpdateText("Camera Insight (OFF)");
                //return;
            }
        }
        void NoPower()
        {
            if (encam2)
            {
                encam2 = false;
            }
            if (encam1)
            {
                encam1 = false;
            }
            _cameraarm1.targetTexture = null;
            _cameraarm2.targetTexture = null;
            _cameraarm1.enabled = false;
            _cameraarm2.enabled = false;
            MuestraUI.rawImage2.enabled = false;
            MuestraUI.rawImage22.enabled = true;
            MuestraUI.rawImage23.enabled = false;
            _camera.targetTexture = null;
            _camera.enabled = false;
            MuestraUI.rawImage.enabled = false;
            MuestraUI.rawImage11.enabled = true;
            MuestraUI.rawImage12.enabled = false;
            MuestraUI.UpdateText2("Camera(NO POWER)");
            MuestraUI.UpdateText("Camera(NO POWER)");
            MuestraUI.toggleButton.isOn = false;
            MuestraUI.toggleButton2.isOn = false;
            return;
        }
        void NoSignal()
        {
            if (encam2)
            {
                encam2 = false;
            }
            if (encam1)
            {
                encam1 = false;
            }
            _cameraarm1.targetTexture = null;
            _cameraarm2.targetTexture = null;
            _cameraarm1.enabled = false;
            _cameraarm2.enabled = false;
            MuestraUI.rawImage2.enabled = false;
            MuestraUI.rawImage22.enabled = false;
            MuestraUI.rawImage23.enabled = true;
            _camera.targetTexture = null;
            _camera.enabled = false;
            MuestraUI.rawImage.enabled = false;
            MuestraUI.rawImage11.enabled = false;
            MuestraUI.rawImage12.enabled = true;
            MuestraUI.UpdateText2("Camera(NO SIGNAL)");
            MuestraUI.UpdateText("Camera(NO SIGNAL)");
            MuestraUI.toggleButton.isOn = false;
            MuestraUI.toggleButton2.isOn = false;
            return;
        }
        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {                
                if (estaactivo)
                {
                    if (bapp == null)
                    {
                        bapp = FindObjectOfType<MuestraUI>();
                    }
                    if (conelectricidad)
                    {                            
                        if (concomunicacion)
                        {                                        
                            if (encam2)
                            {
                                Usecam2(rate);
                            }
                            if (encam1)
                            {
                                Usecam(rate);
                                if (moviendo)
                                {
                                    if (anglex >= minangle && anglex <= maxangle && anglez >= minangle && anglez <= maxangle)
                                    {
                                        Movecam(ratemove);
                                    }
                                }
                            }                                        
                        }                                
                    }                    
                }
            }
        }
        public void Movecam(float cantidad)
        {
            foreach (PartResource reso in resour)
            {
                if (reso.resourceName == resourceName)
                {
                    reso.amount -= cantidad * TimeWarp.fixedDeltaTime;
                }
            }
        }
        public void Usecam(float cantidad)
        {
            foreach (PartResource reso in resour)
            {
                if (reso.resourceName == resourceName)
                {
                    reso.amount -= cantidad * TimeWarp.fixedDeltaTime;
                }
            }            
        }
        public void Usecam2(float cantidad)
        {
            foreach (PartResource reso in resour)
            {
                if (reso.resourceName == resourceName)
                {
                    reso.amount -= cantidad * TimeWarp.deltaTime;
                }
            }
        }
        public void OnpressButton1(bool ison)
        {
            encam1 = ison;            
        }
        public void OnpressButton2(bool ison)
        {
            encam2 = ison;            
        }
        public void SaveZoom()
        {
            zoom1 = _camera.fieldOfView;
            zoom2 = _cameraarm1.fieldOfView;
        }        
        public void OnUp()
        {
            anglex -= 1;
            moviendo = true;
            camerota.localEulerAngles = new Vector3(anglex, camerota.localEulerAngles.y, camerota.localEulerAngles.z);            
        }
        public void OnDown()
        {
            anglex += 1;
            moviendo = true;
            camerota.localEulerAngles = new Vector3(anglex, camerota.localEulerAngles.y, camerota.localEulerAngles.z);
        }
        public void OnLeft()
        {
            anglez -= 1;
            moviendo = true;
            camerota.localEulerAngles = new Vector3(camerota.localEulerAngles.x, camerota.localEulerAngles.y, anglez);
        }
        public void OnRight()
        {
            anglez += 1;
            moviendo = true;
            camerota.localEulerAngles = new Vector3(camerota.localEulerAngles.x, camerota.localEulerAngles.y, anglez);
        }
        public string GetModuleTitle()
        {
            return "Insight Camera";
        }
        public override string GetInfo()
        {
            var o = rate * 60;
            var oi = ratemove * 60;
            return "Interactuable cameras.\n\n"+ "Camera Insight active\n" + "<color=orange>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_469084")+ "</color>" + "\n- " + "<b>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_501004") + "</b>" + " " + o + "/min." + "\nCamera Insight movement\n" + "<color=orange>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_469084") + "</color>" + "\n- " + "<b>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_501004") + "</b>" + " " + oi + "/min." + "\n\nCamera Arm\n" + "<color=orange>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_469084") + "</color>" + "\n- " + "<b>" + KSP.Localization.Localizer.GetStringByTag("#autoLOC_501004") + "</b>" + " " + o + "/min.";
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }
    }    
}
