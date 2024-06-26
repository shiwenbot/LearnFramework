/*
 �ο����ϣ�https://github.com/ivomarel/InfinityScroll/blob/master/Scripts/InfiniteScroll.cs#L168
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
            Init();
        }

        private void Update()
        {
            HandleScroll();
        }

        private void Init()
        {            
            //����container��height�������Ҫ���ɶ��ٸ�slot
            int slotCount = (int)(container.rect.height / itemGameObject.GetComponent<RectTransform>().rect.height);
            rightIndex = slotCount - 1;
            //����slot
            for (int i = 0; i < slotCount; i++)
            {
                var item = inventory.itemList[i];
                RectTransform itemSlotRectTransform = Instantiate(itemGameObject, transform).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.Find("Name").GetComponent<Text>().text = item.name;
            }
            
            itemGameObject.SetActive(false); //����һ��ʼ��Ԥ�������
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
    }
}