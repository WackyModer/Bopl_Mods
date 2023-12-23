using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace arrowWall
{
    [BepInPlugin("com.WackyModer.arrowWall", "Arrow Wall", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony harmony = new Harmony("com.WackyModer.arrowWall");


            MethodInfo original = AccessTools.Method(typeof(BowTransform), "Shoot");
            MethodInfo patch = AccessTools.Method(typeof(myPatches), "Shoot_bowtrans_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));

            original = AccessTools.Method(typeof(BowTransform), "Awake");
            patch = AccessTools.Method(typeof(myPatches), "Awake_bowtrans_myplug");
            harmony.Patch(original, new HarmonyMethod(patch));
        }
    }

    public class myPatches {

        public static bool Shoot_bowtrans_myplug(Vec2 dir, BowTransform __instance, ref Fix ___ArrowSpeed,
    ref PlayerBody ___body, ref RingBuffer<BoplBody> ___Arrows, ref BoplBody ___Arrow, ref bool ___hasFired,
    ref Vec2 ___FirepointOffset, ref Fix ___TimeBeforeArrowsHurtOwner, ref int ___loadingFrame,
    ref PlayerInfo ___playerInfo, ref int ___maxNumberOfArrows)
        {
            for (int i = 0; i <= 39; i++)
            {
                int diff = 20 - i;
                Vec2 pos = ___body.position + (___FirepointOffset.x + (Fix)diff) * ___body.right + ___FirepointOffset.y * ___body.up;
                BoplBody boplBody = FixTransform.InstantiateFixed<BoplBody>(___Arrow, pos, ___body.rotation);

                boplBody.gravityScale = (Fix)0;
                boplBody.Scale = ___body.fixtrans.Scale;
                Fix fix = Fix.One + (___body.fixtrans.Scale - Fix.One) / (Fix)2L;
                ___Arrows.Add(boplBody);
                boplBody.GetComponent<IPlayerIdHolder>().SetPlayerId(___playerInfo.playerId);
                boplBody.GetComponent<SpriteRenderer>().material = ___playerInfo.playerMaterial;
                boplBody.StartVelocity = dir * ((Fix)4 + Fix.One) * ___ArrowSpeed * fix + ___body.selfImposedVelocity;

                boplBody.GetComponent<Projectile>().DelayedEnableHurtOwner(___TimeBeforeArrowsHurtOwner * fix / Vec2.Magnitude(boplBody.StartVelocity));
                boplBody.rotation = ___body.rotation;
                ___hasFired = true;

            }
            return false;
        }


        public static bool Awake_bowtrans_myplug(BowTransform __instance, ref int ___maxNumberOfArrows)
        {
            ___maxNumberOfArrows = 5000;

            return true;
        }
    }
}
