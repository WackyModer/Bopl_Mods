using BepInEx;
using BepInEx.Logging;

using BoplFixedMath;
using UnityEngine;
using TMPro;

using BepInEx.Bootstrap;

namespace ModNames
{
    [BepInPlugin("com.WackyModer.ModNames", "ModNames", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {

        public new ManualLogSource Log;

        private GameObject mod_text;

        private void Awake()
        {
            Log = this.Logger;

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
        }

        void Start()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Plugin.print("WTF?!??! There was no canvas object!");
                return;
            }


            int plugin_num = 0;
            foreach (var plugin in Chainloader.PluginInfos)
            {
                mod_text = new GameObject("Mod_List_TextBox", typeof(RectTransform), typeof(TextMeshProUGUI));
                mod_text.transform.SetParent(canvas.transform);

                TextMeshProUGUI textComponent = mod_text.GetComponent<TextMeshProUGUI>();

                if (plugin.Key.Split('.').Length == 3)
                {
                    textComponent.text = "";
                    if (plugin.Key.Split('.')[2] == "ModNames") {
                        textComponent.text = "(This mod) ";
                    }
                    textComponent.text += $"{plugin.Value} by \"{plugin.Key.Split('.')[1]}\"";
                } else if(plugin.Key.Split('.').Length == 2)
                {
                    textComponent.text = $"{plugin.Value} by \"{plugin.Key.Split('.')[0]}\"";
                }
                else
                {
                    textComponent.text = $"I\'m a stupidhead that CANT FORMAT THE BEPINEX PLUGIN INFO!!!! (Check #how-to-make-mods in the modding server...)";
                }

                textComponent.font = LocalizedText.localizationTable.GetFont(Settings.Get().Language, false);
                textComponent.color = Color.black;

                textComponent.raycastTarget = false;

                textComponent.fontSize = 20;
                textComponent.alignment = TextAlignmentOptions.Right;

                RectTransform rectTransform = mod_text.GetComponent<RectTransform>();

                // unused, may never be used
                float canv_h = canvas.GetComponent<RectTransform>().rect.height;
                float canv_w = canvas.GetComponent<RectTransform>().rect.width;

                rectTransform.anchoredPosition = new Vector2(0, 0 - 130 + 10 * plugin_num);
                rectTransform.sizeDelta = new Vector2(1250, 30);
                plugin_num++;
            }

            // Make the text box visible
            mod_text.SetActive(true);
        }
    }
}
