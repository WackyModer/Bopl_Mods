using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;
using System.Threading;
using UnityEngine.UIElements;

namespace dummies
{
    [BepInPlugin("com.WackyModer.iHaveNoFriends", "Dummies", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded! (I will krill myself, its a shrimple fact)");

            Harmony harmony = new Harmony("com.WackyModer.iHaveNoFriends");


            MethodInfo original;
            MethodInfo patch;

            original = AccessTools.Method(typeof(PlayerHandler), "HasAliveTeammate");
            patch = AccessTools.Method(typeof(myPatches), "HasAliveTeammate");
            harmony.Patch(original, new HarmonyMethod(patch));

        }

        public delegate void KeyPressedCallback(KeyCode keyCode);
        public event KeyPressedCallback OnKeyPressed;

        private void Update()
        {
            if (Keyboard.current[Key.E].wasPressedThisFrame)
            {

                Vector2 mouse_pos = Mouse.current.position.ReadValue();



                if (mouse_pos.x < Screen.width && mouse_pos.x > 0 &&
                    mouse_pos.y < Screen.height && mouse_pos.y > 0)
                {
                    Vector2 mpos2 = new Vector2((mouse_pos.x / Screen.width)-0.5f, (mouse_pos.y / Screen.height)-0.5f);
                    dummy_thing.spawn_dummy(mouse_pos);
                    
                }
                else
                {
                }
            }
        }
    }

    public class dummy_thing
    {
        public static int dummynum = 0;
        public static void spawn_dummy(Vector2 mpos)
        {
            dummynum++;
            try
            {
                Vector3 mousePosition = Camera.current.ScreenToWorldPoint(mpos);
                mousePosition.z = 0; 
                Vector3 localPosition = GameObject.Find($"PlayerList").transform.InverseTransformPoint(mousePosition);


                GameObject pl_thing = GameObject.Instantiate(GameObject.Find($"PlayerList/Player(Clone)"),GameObject.Find($"PlayerList").transform);


                pl_thing.name = "dummy_"+dummynum;

                pl_thing.GetComponent<SlimeController>().SetPlayerId(dummynum+2);
                pl_thing.GetComponent<SlimeController>().enabled = false;


                pl_thing.GetComponent<SlimeTrailHandler>().enabled = false;
                pl_thing.GetComponent<VarySlimeTrailOnLand>().enabled = false;


                pl_thing.GetComponent<FixTransform>().position = new Vec2((Fix)localPosition.x, (Fix)localPosition.y);

            }
            catch
            {
        
            }
        }
    }

    public class myPatches
    {
        public static bool HasAliveTeammate(int playerId, ref bool __result)
        {
            __result = true;
            return false;
        }

    }
}
