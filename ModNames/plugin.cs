using BepInEx;
using BepInEx.Logging;

using UnityEngine;
using TMPro;

using BepInEx.Bootstrap;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ModNames
{
    [BepInPlugin("com.WackyModer.ModNames", "ModNames", "1.1.0")]
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



        Canvas current_canvas;



        void spawnshit()
        {

            Canvas canvas = current_canvas;
            
            canvas = GameObject.Find("Canvas (1)").GetComponent<Canvas>();
            
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
                
                textComponent.raycastTarget = false;

                if (plugin.Key.Split('.').Length == 3)
                {
                    textComponent.text = "";
                    if (plugin.Key.Split('.')[2] == "ModNames")
                    {
                        textComponent.text = "(This mod) ";
                    }
                    textComponent.text += $"{plugin.Value} by \"{plugin.Key.Split('.')[1]}\"";
                }
                else if (plugin.Key.Split('.').Length == 2)
                {
                    textComponent.text = $"{plugin.Value} by \"{plugin.Key.Split('.')[0]}\"";
                }
                else
                {
                    textComponent.text = $"(Mod contains incorrect plugin info)";
                }

                textComponent.font = LocalizedText.localizationTable.GetFont(Settings.Get().Language, false);
                textComponent.color = Color.black;

                textComponent.raycastTarget = false;

                textComponent.fontSize = 0.27f;
                textComponent.alignment = TextAlignmentOptions.Right;


                RectTransform rectTransform = mod_text.GetComponent<RectTransform>();

                float canv_h = canvas.GetComponent<RectTransform>().rect.height;
                float canv_w = canvas.GetComponent<RectTransform>().rect.width;

                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.sizeDelta = new Vector2(7, 0.5f);

                rectTransform.anchoredPosition = new Vector2(-30, 100 + (80 * plugin_num));
                rectTransform.pivot = new Vector2(1, 0);
                plugin_num++;
            }

            mod_text.SetActive(true);
        }

        void Update()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene != "MainMenu")
            {
                try {
                    GameObject.Destroy(GameObject.Find("ModNamesCanvas"));
                } catch
                {

                }
            }
            else
            {
                bool do_i_have_it = false;
                for(int i = 0; i < GameObject.Find("Canvas (1)").transform.childCount; i++)
                {
                    GameObject obj = GameObject.Find("Canvas (1)").transform.GetChild(i).gameObject;

                    if(obj.name == "Mod_List_TextBox")
                    {
                        do_i_have_it = true;
                        break;
                    }
                }
                if (!do_i_have_it)
                {
                    spawnshit();
                }
            }
        }

        void Start()
        {
            GameObject canvasObject = new GameObject("ModNamesCanvas");

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            current_canvas = canvas;
            canvasObject.AddComponent<CanvasScaler>();


            float canv_h = canvas.GetComponent<RectTransform>().rect.height;

            Plugin.print(canv_h);



            canvas = FindObjectOfType<Canvas>();
            Plugin.print(canvas.name);
            canvas = GameObject.Find("Canvas (1)").GetComponent<Canvas>();
            Plugin.print(canvas.name);
            if (canvas == null)
            {
                Plugin.print("WTF?!??! There was no canvas object!");
                return;
            }

            GameObject ver_txt = GameObject.Find("Canvas (1)/TEXT");

            spawnshit();

        }
    }
}
