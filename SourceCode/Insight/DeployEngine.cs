using UnityEngine;

namespace Insight
{
    public class DeployEngine : PartModule
    {
        [KSPField(isPersistant = false)]
        public string DeployAnimation;
        [KSPField(isPersistant = false)]
        public string RetractAnimation;
        private AnimationClip anideploy = null;
        private AnimationClip aniretract = null;
        private Animation ani = null;
        private float timer = 0;
        private bool deployed = false , sale = false, repro = false, enespera = false, encendido = false;
        public ModuleEnginesFX motor = null;
        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            SetupAnim(DeployAnimation, RetractAnimation,part);
            if (HighLogic.LoadedSceneIsFlight)
            {
                foreach (var moto in part.FindModulesImplementing<ModuleEnginesFX>())
                {
                    encendido = moto.EngineIgnited;
                    motor = moto;                    
                }
            }            
        }
        public void Timelon(float tiempo)
        {
            timer += Time.deltaTime;
            if(timer < tiempo)
            {
                deployed = false;
                motor.Shutdown();
            }
            if (timer >= tiempo)
            {
                motor.Activate();
                deployed = true;
                sale = true;
                enespera = false;
                timer = 0;
            }                        
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                encendido = motor.EngineIgnited;                
            }
            if (encendido)
            {
                repro = false;
                if (sale)
                {
                    sale = false;
                }
            }            
            if (!encendido)
            {
                if (enespera && !sale)
                {
                    ani.clip = anideploy;
                    ani.Play();
                }
                if (!enespera && deployed)
                {
                    deployed = false;
                }
                if(!enespera && !repro)
                {
                    ani.clip = aniretract;
                    ani.Play();
                    repro = true;
                }
            }                
            if (HighLogic.LoadedSceneIsFlight && !enespera && !deployed && encendido)
            {
                enespera = true;
            }
            if (enespera)
            {
                Timelon(anideploy.length);                    
            }
        }
        public void SetupAnim(string animationName, string animationName2, Part part)
        {
            foreach (var animation in part.FindModelAnimators(animationName))
            {                
                ani = animation;
                anideploy = ani.GetClip(animationName);
                aniretract = ani.GetClip(animationName2);
            }            
        }
        public string GetModuleTitle()
        {
            return "DeployEngine";
        }

        public override string GetInfo()
        {
            return "Retractable engine.";
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }
    }
}
