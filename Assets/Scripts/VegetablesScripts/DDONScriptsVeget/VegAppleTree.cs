using UnityEngine;

namespace VegetablesScripts.DDONScriptsVeget
{
   public class VegAppleTree : MonoBehaviour
    {
        public VegAppleSeed VegAppleSeed;

        private void OnEnable()
        {
            VegAppleSeed.Initialize();
        }

        public string VegGrapeCluster;

        public string VegWebPageLink
        {
            get => VegWatermelonVine;
            set => VegWatermelonVine = value;
        }

        public int VegTangerineTreeHeight = 70;

        private string VegWatermelonVine;
        private UniWebView VegBananaBrowser;
        private GameObject VegloadingIndicator;

        private void Start()
        {
            VegSetupUI();
            VegLoadWebPage(VegWatermelonVine);
            VegHideLoadingIndicator();
        }

        private void VegSetupUI()
        {
            VegInitializeWebView();

            switch (VegWebPageLink)
            {
                case "0":
                    VegBananaBrowser.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    VegBananaBrowser.SetShowToolbar(false);
                    break;
            }

            VegBananaBrowser.Frame = new Rect(0, VegTangerineTreeHeight, Screen.width, Screen.height - VegTangerineTreeHeight);

            // Other setup logic...

            VegBananaBrowser.OnPageFinished += (_, _, url) =>
            {
                if (PlayerPrefs.GetString("LastLoadedPage", string.Empty) == string.Empty)
                {
                    PlayerPrefs.SetString("LastLoadedPage", url);
                }
            };
        }

        private void VegInitializeWebView()
        {
            VegBananaBrowser = GetComponent<UniWebView>();
            if (VegBananaBrowser == null)
            {
                VegBananaBrowser = gameObject.AddComponent<UniWebView>();
            }

            VegBananaBrowser.OnShouldClose += _ => false;

            // Other initialization logic...
        }

        private void VegLoadWebPage(string url)
        {
            print((url));
            if (!string.IsNullOrEmpty(url))
            {
                VegBananaBrowser.Load(url);
            }
        }

        private void VegHideLoadingIndicator()
        {
            if (VegloadingIndicator != null)
            {
                VegloadingIndicator.SetActive(false);
            }
        }
    }
}