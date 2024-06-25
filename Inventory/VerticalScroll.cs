using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShootGame
{
    public class VerticalScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform container;
        public List<Item> itemList = new List<Item>();
        private Vector3 lastMousePos; //Êó±êÎ»ÖÃ
        private float offset = 0; //Æ«ÒÆÁ¿

        public void OnBeginDrag(PointerEventData eventData)
        {
            lastMousePos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            offset = eventData.position.y - lastMousePos.y;
            lastMousePos = eventData.position;

            for(int i = 0; i < itemList.Count; i++)
            {
                
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
    }
}