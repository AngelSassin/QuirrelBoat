using Modding;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GlobalEnums;
using System.IO;
using System.Reflection;

namespace QuirrelBoat
{
    public class QuirrelBoat : Mod
    {
        public const string Version = "1.0.0.0";
        public override string GetVersion() => QuirrelBoat.Version;
        internal static QuirrelBoat Instance;
        internal GameObject quirrel_prefab = null;
        internal AssetBundle quirrelBundle = null;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;

            string bundleN = "quirrelbundle";
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string res in asm.GetManifestResourceNames())
            {
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                    if (s == null) continue;
                    string bundleName = Path.GetExtension(res).Substring(1);
                    if (bundleName != bundleN) continue;
                    quirrelBundle = AssetBundle.LoadFromStream(s);
                }
            }
            quirrel_prefab = GameObject.Instantiate(quirrelBundle.LoadAsset<GameObject>("DeadQuirrel"));
            GameObject.DontDestroyOnLoad(quirrel_prefab);
            quirrel_prefab.SetActive(false);

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChange;
            On.HeroController.TakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            if (HeroController.instance.gameObject.GetComponent<BoatRider>() && damageAmount > 0)
            {
                if (HeroController.instance.gameObject.GetComponent<BoatRider>().riding)
                    HeroController.instance.gameObject.GetComponent<BoatRider>().StopRiding();
            }

            orig(self, go, damageSide, damageAmount, hazardType);
        }

        private void OnActiveSceneChange(Scene arg0, Scene arg1)
        {
            if (quirrel_prefab == null)
                return;

            if (!arg1.name.Equals("Crossroads_50"))
            {
                quirrel_prefab.SetActive(false);
                if (quirrel_prefab.GetComponent<Boat>())
                    GameObject.Destroy(quirrel_prefab.GetComponent<Boat>());
                return;
            }

            if (!HeroController.instance.playerData.quirrelEpilogueCompleted)
            {
                quirrel_prefab.SetActive(false);
                return;
            }

            quirrel_prefab.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            quirrel_prefab.GetComponent<SpriteRenderer>().enabled = true;

            if (!quirrel_prefab.GetComponent<Boat>())
                quirrel_prefab.AddComponent<Boat>();
            quirrel_prefab.SetActive(true);
            quirrel_prefab.GetComponent<Boat>().enabled = true;
            HeroController.instance.gameObject.AddComponent<BoatRider>();
        }
    }
}
