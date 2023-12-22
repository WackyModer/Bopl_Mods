using BepInEx;
using HarmonyLib;
using System.Reflection;
using BoplFixedMath;

namespace noSizeCaps
{
    [BepInPlugin("com.WackyModer.noSizeCaps", "No Size Caps", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");


            Harmony harmony = new Harmony("com.WackyModer.noSizeCaps");

            MethodInfo original = AccessTools.Method(typeof(PlatformTransform), "Init");
            MethodInfo patch = AccessTools.Method(typeof(myPatches), "Init_plattrans_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));


            original = AccessTools.Method(typeof(DPhysicsBox), "Init");
            patch = AccessTools.Method(typeof(myPatches), "Init_dphybox_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));


            original = AccessTools.Method(typeof(DPhysicsRoundedRect), "Init");
            patch = AccessTools.Method(typeof(myPatches), "Init_dphyrndrct_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));


            original = AccessTools.Method(typeof(DPhysicsCircle), "Init");
            patch = AccessTools.Method(typeof(myPatches), "Init_dphycirl_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));

            harmony.PatchAll();
        }
    }
    public class myPatches
    {
        public static bool Init_plattrans_myplug(PlatformTransform __instance)
        {
            __instance.MaxSize = (Fix)1000L;
            __instance.MinSize = (Fix)0.00001f;
            __instance.maxArea = (Fix)2000L;
            return true;
        }

        public static bool Init_dphybox_myplug(DPhysicsBox __instance)
        {
            __instance.MaxScale = (Fix)1000L;
            __instance.MinScale = (Fix)0.00001f;
            return true;
        }

        public static bool Init_dphyrndrct_myplug(DPhysicsRoundedRect __instance)
        {
            __instance.MaxScale = (Fix)1000L;
            __instance.MinScale = (Fix)0.00001f;
            return true;
        }

        public static bool Init_dphycirl_myplug(DPhysicsCircle __instance)
        {
            __instance.MaxScale = (Fix)1000L;
            __instance.MinScale = (Fix)0.00001f;
            return true;
        }
    }


    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Scale", MethodType.Setter)]
    public class YourClass_Scale_SetterPatch
    {
        static bool Prefix(ref Fix value, ref Fix ___scale)
        {            
           ___scale = value;
            return false;
        }
    }
}
