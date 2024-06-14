using MelonLoader;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace SpeedDisplay
{
    public class SpeedDisplayScript : MelonMod
    {
        static public SpeedDisplayScript Instance;

        public Player player;
        public PlayerMovement playerMovement;

        public TextMeshProUGUI readOut;


        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();
            Instance = this;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (playerMovement != null)
            {
                readOut.text = $"{playerMovement.GetVelocity().magnitude.ToString("F3")}M/s";
            }
        }

        public void setPlayerScripts()
        {
            setupUI();
            playerMovement = player.GetMovementScript();
        }

        public void setupUI()
        {
            PlayerHUD playerHUD = player.GetHUD();
            GameObject retAnchor = playerHUD.GetReticle().transform.Find("Anchor").gameObject;
            GameObject g = GameObject.Instantiate(new GameObject(), retAnchor.transform);
            g.name = "Speed Display";
            g.transform.localPosition = new Vector3 (125, -25, 0);
            readOut = g.AddComponent<TextMeshProUGUI>();
            readOut.enableAutoSizing = false;
            readOut.fontSize = 24;
        }

        [HarmonyPatch(typeof(Player), "Initialize")]
        public static class playerInit
        {
            private static void Postfix(ref Player __instance)
            {
                Instance.player = __instance;
                Instance.setPlayerScripts();
                MelonLogger.Msg("Player Found");
            }
        }
    }
}
