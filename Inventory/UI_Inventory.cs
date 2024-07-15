/*
 �ο����ϣ�https://github.com/ivomarel/InfinityScroll/blob/master/Scripts/InfiniteScroll.cs#L168
 */
using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShootGame
{
    public class UI_Inventory : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler, IController
    {        
        private Inventory inventory;
        private GameObject itemGameObject;
        private Dictionary<GameObject, int> itemIndexMap = new Dictionary<GameObject, int>();

        public RectTransform container;       
        private Vector3 lastMousePos; //���λ��
        private float offset = 0; //ƫ����
        private int leftIndex = 0, rightIndex = 0; //list��indexҪ��container��childС1����Ϊchild������һ��Ԥ���壨�ڳ�ʼ����ɺ�ᱻ���ó�inactive��        

        private void Awake()
        {
            inventory = new CurrentInventoryQuery().Do();
            itemGameObject = GameObject.Find("Item");
        }

        private void Start()
        {            
            InitInventory();
        }

        private void Update()
        {
            HandleScroll();
            HandleMouseClick();
        }

        private void InitInventory()
        {
            //����container��height�������Ҫ���ɶ��ٸ�slot, vertical layout group
            int slotCount = (int)(container.rect.height / itemGameObject.GetComponent<RectTransform>().rect.height);
            /*int horizontalCount = (int)(container.rect.width / container.GetComponent<GridLayoutGroup>().cellSize.x);
            int verticalCount = (int)((container.rect.height / container.GetComponent<GridLayoutGroup>().cellSize.y));
            int slotCount = horizontalCount * verticalCount;*/
            rightIndex = slotCount - 1;
            //����slot
            for (int i = 0; i < slotCount; i++)
            {
                InitItem(i);
            }            
            itemGameObject.SetActive(false); //����һ��ʼ��Ԥ�������
        }

        private void InitItem(int index)
        {
            var item = inventory.itemList[index];
            GameObject newItem = Instantiate(itemGameObject, transform);
            RectTransform itemSlotRectTransform = newItem.GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.Find("Name").GetComponent<Text>().text = item.name;
            itemIndexMap[newItem] = index;

            Transform btn = newItem.transform.Find("Button");
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                DestroyItem(newItem);
            });           
        }

        private void DestroyItem(GameObject newItem)
        {
            //���ٶ��󣬰�item��list���Ƴ�
            int index = itemIndexMap[newItem];
            Debug.Log("Before remove, size of list: " + inventory.itemList.Count);
            inventory.itemList.RemoveAt(index);
            Debug.Log("After remove, size of list: " + inventory.itemList.Count);
            //����left��right������ĩβ�½�һ������

            Destroy(newItem);
            InitItem(rightIndex);
            //Debug.Log("Item name: " + inventory.itemList[rightIndex + 1].name);
        }

        //TODO:�ö���ع�����Դ
        private void HandleScroll()
        {
            //���ϻ����б�
            if(offset > 0 && rightIndex < inventory.itemList.Count) 
            {
                //�ж��Ƿ񳬳�container�����ϱ߽�
                //list��indexҪ��container��childС1����Ϊchild������һ��Ԥ���壨�ڳ�ʼ����ɺ�ᱻ���ó�inactive��
                RectTransform topChild = container.GetChild(leftIndex + 1).GetComponent<RectTransform>();

                //��slot�ĵײ�����container���ϱ߽�ʱ���Ż��ж�Ϊ�����ϱ߽�
                if (topChild.anchoredPosition.y > container.anchoredPosition.y)
                {
                    //Debug.Log("topChild: " + topChild.anchoredPosition.y + ", container: " + container.anchoredPosition.y);
                    Debug.Log("�����ϱ߽磬�Ƴ�����");
                    Destroy(topChild.gameObject);
                    RectTransform itemSlotRectTransform = Instantiate(itemGameObject, transform).GetComponent<RectTransform>();
                    itemSlotRectTransform.gameObject.SetActive(true);
                    itemSlotRectTransform.Find("Name").GetComponent<Text>().text = inventory.itemList[rightIndex + 1].name;
                    Debug.Log("�ڽ�β�����µĶ���");

                    leftIndex++;
                    rightIndex++;
                }
            }
            else
            {

            }
        }

        private void HandleMouseClick()
        {
            //��������ʹ����Ʒ
            if (Input.GetMouseButtonDown(0))
            {
                //�����
                GameObject obj = GetFirstPickGameObject(Input.mousePosition);                
            }
        }

        /// <summary>
        /// �����Ļ����
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GameObject GetFirstPickGameObject(Vector2 position)
        {
            EventSystem eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = position;
            //���߼��ui
            List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
            if (uiRaycastResultCache.Count > 0)
                return uiRaycastResultCache[0].gameObject;
            return null;
        }

        #region MouseDrag
        public void OnBeginDrag(PointerEventData eventData)
        {
            lastMousePos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            offset = eventData.position.y - lastMousePos.y;
            lastMousePos = eventData.position;

            //��ȡ����������
            for (int i = 0; i < container.childCount; i++)
            {
                RectTransform child = container.GetChild(i).GetComponent<RectTransform>();
                child.anchoredPosition += new Vector2(0, offset);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }
    }
}