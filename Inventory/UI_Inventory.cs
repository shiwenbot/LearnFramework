/*
 参考资料：https://github.com/ivomarel/InfinityScroll/blob/master/Scripts/InfiniteScroll.cs#L168
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
        private Vector3 lastMousePos; //鼠标位置
        private float offset = 0; //偏移量
        private int leftIndex = 0, rightIndex = 0; //list的index要比container的child小1，因为child包含了一个预制体（在初始化完成后会被设置成inactive）        

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
            //根据container的height计算出需要生成多少个slot, vertical layout group
            int slotCount = (int)(container.rect.height / itemGameObject.GetComponent<RectTransform>().rect.height);
            /*int horizontalCount = (int)(container.rect.width / container.GetComponent<GridLayoutGroup>().cellSize.x);
            int verticalCount = (int)((container.rect.height / container.GetComponent<GridLayoutGroup>().cellSize.y));
            int slotCount = horizontalCount * verticalCount;*/
            rightIndex = slotCount - 1;
            //生成slot
            for (int i = 0; i < slotCount; i++)
            {
                InitItem(i);
            }            
            itemGameObject.SetActive(false); //最后把一开始的预制体禁用
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
            //销毁对象，把item从list中移除
            int index = itemIndexMap[newItem];
            Debug.Log("Before remove, size of list: " + inventory.itemList.Count);
            inventory.itemList.RemoveAt(index);
            Debug.Log("After remove, size of list: " + inventory.itemList.Count);
            //更新left，right，并在末尾新建一个对象

            Destroy(newItem);
            InitItem(rightIndex);
            //Debug.Log("Item name: " + inventory.itemList[rightIndex + 1].name);
        }

        //TODO:用对象池管理资源
        private void HandleScroll()
        {
            //向上滑动列表
            if(offset > 0 && rightIndex < inventory.itemList.Count) 
            {
                //判断是否超出container的最上边界
                //list的index要比container的child小1，因为child包含了一个预制体（在初始化完成后会被设置成inactive）
                RectTransform topChild = container.GetChild(leftIndex + 1).GetComponent<RectTransform>();

                //当slot的底部超出container的上边界时，才会判定为超出上边界
                if (topChild.anchoredPosition.y > container.anchoredPosition.y)
                {
                    //Debug.Log("topChild: " + topChild.anchoredPosition.y + ", container: " + container.anchoredPosition.y);
                    Debug.Log("超出上边界，移除对象");
                    Destroy(topChild.gameObject);
                    RectTransform itemSlotRectTransform = Instantiate(itemGameObject, transform).GetComponent<RectTransform>();
                    itemSlotRectTransform.gameObject.SetActive(true);
                    itemSlotRectTransform.Find("Name").GetComponent<Text>().text = inventory.itemList[rightIndex + 1].name;
                    Debug.Log("在结尾创建新的对象");

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
            //点击左键是使用物品
            if (Input.GetMouseButtonDown(0))
            {
                //点击后
                GameObject obj = GetFirstPickGameObject(Input.mousePosition);                
            }
        }

        /// <summary>
        /// 点击屏幕坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GameObject GetFirstPickGameObject(Vector2 position)
        {
            EventSystem eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = position;
            //射线检测ui
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

            //获取所有子物体
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