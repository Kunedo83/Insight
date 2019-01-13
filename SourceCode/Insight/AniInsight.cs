using UnityEngine;
using System.Collections;
namespace Insight
{
    public class AniInsight : PartModule
    {
        private ModuleAnimateGeneric taladro = null;
        private ModuleAnimateGeneric presion = null;
        private ModuleScienceExperiment tala = null;
        private ModuleScienceExperiment pre = null;
        private bool aniactive = false;
        private float timer = 0;
        private GameObject brazo1 = null;
        private GameObject brazo2 = null;
        private bool brazo = false;
        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight)
            {
                brazo1 = part.FindModelTransform("arm_01 (1)").gameObject;
                brazo2 = part.FindModelTransform("arm_01 (2)").gameObject;
                brazo1.SetActive(true);
                brazo2.SetActive(false);
                foreach (var ciencia in part.FindModulesImplementing<ModuleScienceExperiment>())
                {
                    if (ciencia.experimentID == "taladro")
                    {
                        tala = ciencia;
                    }
                    if (ciencia.experimentID == "mobileMaterialsLab")
                    {
                        pre = ciencia;
                    }
                }
                foreach (var anim in part.FindModulesImplementing<ModuleAnimateGeneric>())
                {
                    if (anim.moduleID == "tala")
                    {
                        taladro = anim;
                    }
                    if (anim.moduleID == "pre")
                    {
                        presion = anim;
                    }
                }
            }
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                pre.Events["DeployExperiment"].guiActive = false;
                tala.Events["DeployExperiment"].guiActive = false;
                BaseEvent deplo1 = Events["StartSEIS"];
                BaseEvent deplo2 = Events["StartHeat"];
                if (aniactive)
                {
                    timer += Time.deltaTime;
                    if (timer < 20)
                    {
                        deplo1.guiActive = false;
                        deplo2.guiActive = false;
                    }
                    if (timer >= 20)
                    {
                        if (!brazo)
                        {
                            if (presion.animTime < 0.01f)
                            {
                                deplo1.guiActive = true;
                                deplo2.guiActive = true;
                                aniactive = false;
                            }
                        }
                        if (brazo)
                        {
                            if (taladro.animTime < 0.01f)
                            {
                                deplo1.guiActive = true;
                                deplo2.guiActive = true;
                                aniactive = false;
                            }
                        }
                    }
                }
            }
        }
        [KSPEvent(guiActive = true, guiName = "Start SEIS", active = true)]
        public void StartSEIS()
        {
            BaseEvent deplo = Events["StartSEIS"];
            BaseEvent deplo2 = Events["StartHeat"];
            deplo.guiActive = false;
            deplo2.guiActive = false;
            timer = 0;
            brazo1 = part.FindModelTransform("arm_01 (1)").gameObject;
            brazo2 = part.FindModelTransform("arm_01 (2)").gameObject;
            brazo1.SetActive(true);
            brazo2.SetActive(false);
            Quesoy(false);
            pre.DeployExperiment();
            aniactive = true;
        }
        [KSPEvent(guiActive = true, guiName = "Start Heat Probe", active = true)]
        public void StartHeat()
        {
            BaseEvent deplo = Events["StartHeat"];
            BaseEvent deplo2 = Events["StartSEIS"];
            deplo.guiActive = false;
            deplo2.guiActive = false;
            timer = 0;
            brazo1 = part.FindModelTransform("arm_01 (1)").gameObject;
            brazo2 = part.FindModelTransform("arm_01 (2)").gameObject;
            brazo1.SetActive(false);
            brazo2.SetActive(true);
            Quesoy(true);
            tala.DeployExperiment();
            aniactive = true;
        }
        public void Quesoy(bool hohoho)
        {
            brazo = hohoho;
        }
    }
}
