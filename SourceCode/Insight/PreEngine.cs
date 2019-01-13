using System.Collections.Generic;
using UnityEngine;

namespace Insight
{
    public class PreEngine : PartModule
    {
        [KSPField(isPersistant = false)]
        public float Minsize;
        [KSPField(isPersistant = false)]
        public float Maxsize;
        [KSPField(isPersistant = false)]
        public float Minenergy;
        [KSPField(isPersistant = false)]
        public float Maxenergy;
        [KSPField(isPersistant = false)]
        public int Minemission;
        [KSPField(isPersistant = false)]
        public int Maxemission;
        [KSPField(isPersistant = false)]
        public float LocalSpeedX;
        [KSPField(isPersistant = false)]
        public float LocalSpeedY;
        [KSPField(isPersistant = false)]
        public float LocalSpeedZ;
        [KSPField(isPersistant = false)]
        public float WorldSpeedX;
        [KSPField(isPersistant = false)]
        public float WorldSpeedY;
        [KSPField(isPersistant = false)]
        public float WorldSpeedZ;
        [KSPField(isPersistant = false)]
        public float RndSpeedX;
        [KSPField(isPersistant = false)]
        public float RndSpeedY;
        [KSPField(isPersistant = false)]
        public float RndSpeedZ;
        [KSPField(isPersistant = false)]
        public float ForceX;
        [KSPField(isPersistant = false)]
        public float ForceY;
        [KSPField(isPersistant = false)]
        public float ForceZ;
        [KSPField(isPersistant = false)]
        public float RndForceX;
        [KSPField(isPersistant = false)]
        public float RndForceY;
        [KSPField(isPersistant = false)]
        public float RndForceZ;
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "Effects Pre-Launch"),
         UI_Toggle(disabledText = "Disabled", enabledText = "Enabled")]
        public bool enablesmoke = true;
        public List<KSPParticleEmitter> emission;
        public double estadoparte;
        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            emission = part.FindModelComponents<KSPParticleEmitter>();
            foreach (KSPParticleEmitter emi in emission)
            {
                EffectBehaviour.AddParticleEmitter(emi);
            }
        }
        void Update()
        {            
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (enablesmoke)
                {
                    estadoparte = vessel.speed;
                    foreach (KSPParticleEmitter emi in emission)
                    {
                        if (Vessel.Situations.PRELAUNCH == vessel.situation)
                        {
                            emi.minSize = Minsize;
                            emi.maxSize = Maxsize;
                            emi.minEnergy = Minenergy;
                            emi.maxEnergy = Maxenergy;
                            emi.minEmission = Minemission;
                            emi.maxEmission = Maxemission;
                            emi.worldVelocity = new Vector3(WorldSpeedX, WorldSpeedY, WorldSpeedZ);
                            emi.localVelocity = new Vector3(LocalSpeedX, LocalSpeedY, LocalSpeedZ);
                            emi.rndVelocity = new Vector3(RndSpeedX, RndSpeedY, RndSpeedZ);
                            emi.force = new Vector3(ForceX, ForceY, ForceZ);
                            emi.rndForce = new Vector3(RndForceX, RndForceY, RndForceZ);
                            emi.emit = true;
                        }
                        else
                        {
                            enablesmoke = false;                            
                        }
                    }                                        
                }
                if (!enablesmoke)
                {
                    OnDestroy();
                }
            }
            if (!HighLogic.LoadedSceneIsFlight)
            {                
                foreach (KSPParticleEmitter emi in emission)
                {
                    emi.emit = false;
                }
                return;
            }            
        }
        public void OnDestroy()
        {
            foreach (KSPParticleEmitter emi in emission)
            {
                emi.emit = false;
                enablesmoke = false;
                EffectBehaviour.RemoveParticleEmitter(emi);                
            }            
        }
        public string GetModuleTitle()
        {
            return "PreEngine";
        }

        public override string GetInfo()
        {
            return "Automatic engine warm-up before launch.";
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }
    }
}
