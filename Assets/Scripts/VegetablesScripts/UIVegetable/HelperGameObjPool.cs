using DG.Tweening;
using UnityEngine;

namespace VegetablesScripts.UIVegetable
{
    public class HelperGameObjPool : MonoBehaviour
    {
        private bool isSequenceCompleted;
        private int currentIndex;
        private int totalElements;
        

        public static void FadeCanvasGroup(GameObject canvasObject, bool fadeIn)
        {
            canvasObject.SetActive(true);
            CanvasGroup canvasGroup = canvasObject.GetComponent<CanvasGroup>();
            float targetAlpha = fadeIn ? 1f : 0f;

            canvasGroup.DOFade(targetAlpha, 0.5f).OnComplete(() =>
            {
                if (!fadeIn)
                {
                    canvasObject.SetActive(false);
                }
            });
        }
    }
}