using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.Networking;
using VegetablesScripts.DDONScriptsVeget;
using VegetablesScripts.UIVegetable;

namespace VegetablesScripts.VegetablesWorld
{
    public class VegBananaGrove : MonoBehaviour
    {
        [SerializeField] private VegAppleTree VegNectarineDisclosure;
        [SerializeField] private IDFAController Veg_idfaCheck;

        [SerializeField] private VegCherryVine VegstringConcatenator;

        private bool VegisFirstInstance = true;
        private NetworkReachability VegnetworkReachability = NetworkReachability.NotReachable;

        private string VegglobalLocator1 { get; set; }
        private string VegglobalLocator2;
        private int VegglobalLocator3;

        private string VegtraceCode;

        [SerializeField] private List<string> tokenList;
        [SerializeField] private List<string> detailsList;

        private string Veglabeling;

        private void Awake()
        {
            VegHandleMultipleInstances();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Veg_idfaCheck.ScrutinizeIDFA();
            StartCoroutine(VegFetchAdvertisingID());

            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    VegHandleNoInternetConnection();
                    break;
                default:
                    VegCheckStoredData();
                    break;
            }
        }

        private void VegHandleMultipleInstances()
        {
            switch (VegisFirstInstance)
            {
                case true:
                    VegisFirstInstance = false;
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        private IEnumerator VegFetchAdvertisingID()
        {
#if UNITY_IOS
            var authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            while (authorizationStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                yield return null;
            }
#endif

            VegtraceCode = Veg_idfaCheck.RetrieveAdvertisingID();
            yield return null;
        }

        private void VegCheckStoredData()
        {
            if (PlayerPrefs.GetString("top", string.Empty) != string.Empty)
            {
                VegLoadStoredData();
            }
            else
            {
                VegFetchDataFromServerWithDelay();
            }
        }

        private void VegLoadStoredData()
        {
            VegglobalLocator1 = PlayerPrefs.GetString("top", string.Empty);
            VegglobalLocator2 = PlayerPrefs.GetString("top2", string.Empty);
            VegglobalLocator3 = PlayerPrefs.GetInt("top3", 0);
            VegImportData();
        }

        private void VegFetchDataFromServerWithDelay()
        {
            Invoke(nameof(VegReceiveData), 7.4f);
        }

        private void VegReceiveData()
        {
            if (Application.internetReachability == VegnetworkReachability)
            {
                VegHandleNoInternetConnection();
            }
            else
            {
                StartCoroutine(VegFetchDataFromServer());
            }
        }


        private IEnumerator VegFetchDataFromServer()
        {
            using UnityWebRequest webRequest =
                UnityWebRequest.Get(VegstringConcatenator.VegConcatenateStrings(detailsList));
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                VegHandleNoInternetConnection();
            }
            else
            {
                VegProcessServerResponse(webRequest);
            }
        }

        private void VegProcessServerResponse(UnityWebRequest webRequest)
        {
            string tokenConcatenation = VegstringConcatenator.VegConcatenateStrings(tokenList);

            if (webRequest.downloadHandler.text.Contains(tokenConcatenation))
            {
                try
                {
                    string[] dataParts = webRequest.downloadHandler.text.Split('|');
                    PlayerPrefs.SetString("top", dataParts[0]);
                    PlayerPrefs.SetString("top2", dataParts[1]);
                    PlayerPrefs.SetInt("top3", int.Parse(dataParts[2]));

                    VegglobalLocator1 = dataParts[0];
                    VegglobalLocator2 = dataParts[1];
                    VegglobalLocator3 = int.Parse(dataParts[2]);
                }
                catch
                {
                    PlayerPrefs.SetString("top", webRequest.downloadHandler.text);
                    VegglobalLocator1 = webRequest.downloadHandler.text;
                }

                VegImportData();
            }
            else
            {
                VegHandleNoInternetConnection();
            }
        }

        private void VegImportData()
        {
            VegNectarineDisclosure.VegWebPageLink = $"{VegglobalLocator1}?idfa={VegtraceCode}";
            VegNectarineDisclosure.VegWebPageLink +=
                $"&gaid={AppsFlyer.getAppsFlyerId()}{PlayerPrefs.GetString("Result", string.Empty)}";
            VegNectarineDisclosure.VegGrapeCluster = VegglobalLocator2;


            VegKom();
        }

        public void VegKom()
        {
            VegNectarineDisclosure.VegTangerineTreeHeight = VegglobalLocator3;
            VegNectarineDisclosure.gameObject.SetActive(true);
        }

        private void VegHandleNoInternetConnection()
        {
            VegDisableCanvas();
        }

        private void VegDisableCanvas()
        {
            HelperGameObjPool.FadeCanvasGroup(gameObject, false);
        }
    }
}