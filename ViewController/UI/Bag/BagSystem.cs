using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShootGame
{
    public sealed class BagSystem : FrameworkComponent, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject m_BagUI;
        bool isOpen = false;
        public Inventory m_BagData;
        public GameObject slotPrefab; 
        int curCount = 0;

        public event Action<string, string> ShowItemInfo;

        protected override void Awake()
        {
            base.Awake();

        }
        protected override void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                isOpen = !isOpen;
                m_BagUI.SetActive(isOpen);
                CoroutineHelper.Instance.EnqueueWork(LoadBag());
            }
        }

        IEnumerator LoadBag()
        {
            GameObject slot = Instantiate(slotPrefab);
            

            slot.transform.SetParent(m_BagUI.transform.Find("Grid"));

            RectTransform rectTransform = slot.GetComponent<RectTransform>();
            Vector3 position = rectTransform.localPosition;
            position.z = 0;
            rectTransform.localPosition = position;
            rectTransform.localScale = Vector3.one;
            curCount++;
            if(curCount < 4) CoroutineHelper.Instance.EnqueueWork(LoadBag());
            yield return null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Not Enter");
            if (eventData.pointerEnter.gameObject.transform.parent.name == "Grid")
            {
                Debug.Log("Enter");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }
    }
}
