using UnityEngine;
using ToolbarControl_NS;

namespace Insight
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(ButtonApp.MODID, ButtonApp.MODNAME);
        }
    }
}
