/*
 * AutoHideUIScroller base code is made by Essential on https://forum.unity.com/threads/hide-scrollbar.285929/
 * Code edited By Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */


using UnityEngine;
using UnityEngine.UI;

namespace Tablet
{
    [RequireComponent(typeof(ScrollRect))]
    public class AutoHideUIScrollbar : MonoBehaviour
    {



        public bool alsoDisableScrolling;

        private float disableRange = 0.99f;
        private ScrollRect scrollRect;
        private ScrollbarClass scrollbarVertical = null;
        private ScrollbarClass scrollbarHorizontal = null;

        private bool hasScroll;

        private class ScrollbarClass
        {
            public Scrollbar bar;
            public bool active;
        }


        void Start()
        {
            /*
            scrollRect = gameObject.GetComponent<ScrollRect>();
            if (scrollRect.verticalScrollbar != null)
                scrollbarVertical = new ScrollbarClass() { bar = scrollRect.verticalScrollbar, active = true };
            if (scrollRect.horizontalScrollbar != null)
                scrollbarHorizontal = new ScrollbarClass() { bar = scrollRect.horizontalScrollbar, active = true };

            if (scrollbarVertical == null && scrollbarHorizontal == null)
                Debug.LogWarning("Must have a horizontal or vertical scrollbar attached to the Scroll Rect for AutoHideUIScrollbar to work");
            */
        }
        /*
        void Update()
        {
            if (scrollbarVertical != null)
                SetScrollBar(scrollbarVertical, true);
            if (scrollbarHorizontal != null)
                SetScrollBar(scrollbarHorizontal, false);

        }
        */

        void StretchUIItems(ScrollbarClass scrollbar)
        {

            if (!scrollbar.active)
            {
                RectTransform ContentView = FindDeepChild(gameObject, "Content").GetComponent<RectTransform>();
                ContentView.anchoredPosition = new Vector2(0, ContentView.anchoredPosition.y);
                ContentView.sizeDelta = new Vector2(100, ContentView.sizeDelta.y);
            }
        }
        

        void ResizeUIItemsToOrigin(ScrollbarClass scrollbar)
        {

            if (!scrollbar.active)
            {
                RectTransform ContentView = FindDeepChild(gameObject, "Content").GetComponent<RectTransform>();
                ContentView.anchoredPosition = new Vector2(-3.65f, ContentView.anchoredPosition.y);
                ContentView.sizeDelta = new Vector2(90, ContentView.sizeDelta.y);
            }
        }


        void SetScrollBar(ScrollbarClass scrollbar, bool vertical)
        {
            if (scrollbar.active && scrollbar.bar.size > disableRange)
                SetBar(scrollbar, false, vertical);
            else if (!scrollbar.active && scrollbar.bar.size < disableRange)
                SetBar(scrollbar, true, vertical);


            if (scrollbar.active)
                ResizeUIItemsToOrigin(scrollbar);
            else
                StretchUIItems(scrollbarVertical);
        }

        void SetBar(ScrollbarClass scrollbar, bool active, bool vertical)
        {
            hasScroll = active;

            scrollbar.bar.gameObject.SetActive(active);
            scrollbar.active = active;

            if (alsoDisableScrolling)
            {
                if (vertical)
                    scrollRect.vertical = active;
                else
                    scrollRect.horizontal = active;
            }
        }

        public GameObject FindDeepChild(GameObject parent, string childName)
        {
            foreach (Transform myChild in parent.GetComponentsInChildren<Transform>())
            {
                if (myChild.name == childName)
                    return myChild.gameObject;
            }
            return null;
        }



    }
}
