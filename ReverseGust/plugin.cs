using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

using static UnityEngine.UI.Image;
using System.Runtime.InteropServices;
using BepInEx.Logging;

namespace ReverseGust
{
    [BepInPlugin("com.WackyModer.ReverseGust", "ReverseGust", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony harmony = new Harmony("com.WackyModer.ReverseGust");

            MethodInfo original = AccessTools.Method(typeof(Shockwave), "UpdateHitObjects");
            MethodInfo patch = AccessTools.Method(typeof(myPatches), "UpdateHitObjects_gust_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));
        }
    }

    public class myPatches
    {
        public static bool UpdateHitObjects_gust_myplug(Fix simDeltaTime, Shockwave __instance, ref PhysicsParent[] ___hits,
    ref FixTransform ___fixTrans, ref Fix ___timeAlive, ref int ___hitCount, ref Fix ___scale, ref HashSet<int> ___extraObjectsToIgnore,
    ref HashSet<int> ___gameObjectIdsHit)
        {
            Fix s = -__instance.forceCurve01.Evaluate(___timeAlive);
            ___timeAlive += simDeltaTime;
            if (___timeAlive > __instance.forceCurve01.Duration())
            {
                Updater.DestroyFix(__instance.gameObject);
                return false;
            }
            for (int i = 0; i < ___hitCount; i++)
            {
                if (!(___hits[i].fixTrans == null) && !___hits[i].fixTrans.IsDestroyed && ___hits[i].monobehaviourCollider != null && ___gameObjectIdsHit.Contains(___hits[i].instanceId) && !___extraObjectsToIgnore.Contains(___hits[i].instanceId))
                {
                    StickyRoundedRectangle component = ___hits[i].fixTrans.GetComponent<StickyRoundedRectangle>();
                    Vec2 vec;
                    Vec2 v;
                    Fix fix;
                    if (component != null)
                    {
                        vec = component.GetClosestPoint(___fixTrans.position);
                        v = -component.currentNormal(vec);
                        Fix y = Fix.Max(Vec2.SqrMagnitude(___fixTrans.position - vec) / (Fix)10L, (Fix)1.5);
                        if (component.CompareTag("boulder"))
                        {
                            fix = __instance.defaultForce / y;
                        }
                        else
                        {
                            fix = __instance.platformForce / y;
                        }
                    }
                    else
                    {
                        vec = ___hits[i].fixTrans.position;
                        v = vec - ___fixTrans.position;
                        Fix y = Fix.Max(Vec2.SqrMagnitude(v) / (Fix)10L, (Fix)1.5);
                        fix = __instance.defaultForce / y;
                    }
                    fix *= ___scale;
                    Debug.DrawRay((Vector3)vec, Vector3.left, Color.red);
                    Debug.DrawRay((Vector3)vec, Vector3.up, Color.red);
                    Debug.DrawRay((Vector3)vec, Vector3.right, Color.red);
                    Debug.DrawRay((Vector3)vec, Vector3.down, Color.red);
                    if (___hits[i].fixTrans.gameObject.layer == LayerMask.NameToLayer("RigidBodyAffector"))
                    {
                        BlackHole component2 = ___hits[i].fixTrans.GetComponent<BlackHole>();
                        if (component2 != null)
                        {
                            component2.AddForce(Vec2.NormalizedSafe(v) * fix * s);
                        }
                    }
                    else
                    {
                        ___hits[i].monobehaviourCollider.AddForceAtPosition(Vec2.NormalizedSafe(v) * fix * s, vec, ForceMode2D.Impulse);
                    }
                }
            }


            return false;
        }


    }
}
