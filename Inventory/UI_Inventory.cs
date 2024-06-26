/*
 参考资料：https://github.com/ivomarel/InfinityScroll/blob/master/Scripts/InfiniteScroll.cs#L168
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShootGame
{
    public class UI_Inventory : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
    {        
        private Inventory inventory;
        private GameObject itemGameObject;

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
            Init();
        }

        private void Update()
        {
            HandleScroll();
        }

        private void Init()
        {            
            //根据container的height计算出需要生成多少个slot
            int slotCount = (int)(container.rect.height / itemGameObject.GetComponent<RectTransform>().rect.height);
            rightIndex = slotCount - 1;
            //生成slot
            for (int i = 0; i < slotCount; i++)
            {
                var item = inventory.itemList[i];
                RectTransform itemSlotRectTransform = Instantiate(itemGameObject, transform).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.Find("Name").GetComponent<Text>().text = item.name;
            }
            
            itemGameObject.SetActive(false); //最后把一开始的预制体禁用
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
    }
}