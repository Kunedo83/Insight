using UnityEngine;
using System.IO;
using System.Reflection;

namespace Insight
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CoolUILoader : MonoBehaviour
    {
        public static GameObject PanelPrefab { get; private set; }
        public static GameObject PanelPrefab2 { get; private set; }
        private void Awake()
        {
            AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "caminsight.ksp"));
            PanelPrefab = prefabs.LoadAsset("MyCoolUIPanel") as GameObject;
            PanelPrefab2 = prefabs.LoadAsset("MyCoolUIPanel2") as GameObject;            
        }
    }
}
