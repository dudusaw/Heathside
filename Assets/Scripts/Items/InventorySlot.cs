using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Items
{
    public class InventorySlot : MonoBehaviour, 
        IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] Image iconImage;
        [SerializeField] Item _item;
        [SerializeField] int count = 1;
        [SerializeField] TextMeshProUGUI textMesh;

        bool hasTextMesh;
        public Item Item 
        {
            get => _item;
            private set
            {
                _item = value;
            }
        }

        private void Awake()
        {
            hasTextMesh = textMesh != null;
            UpdateIcon();
        }

        public void UpdateIcon()
        {
            if (Item == null)
            {
                iconImage.enabled = false;
                iconImage.sprite = null;
                if (hasTextMesh)
                {
                    textMesh.enabled = false;
                }
                IsEmpty = true;
                count = 0;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = _item.icon;
                if (hasTextMesh)
                {
                    textMesh.enabled = true;
                    textMesh.text = count.ToString();
                }
                IsEmpty = false;
            }
        }

        public bool IsEmpty { get; private set; }

        public int Count { get => count; }

        public bool AddItemToStack(Item newItem, in int newItemCount, out int itemsCountLeft)
        {
            itemsCountLeft = 0;
            if (newItem == Item)
            {
                int sum = Count + newItemCount;
                count = Mathf.Clamp(sum, 0, Item.maxStack);
                itemsCountLeft = sum - Count;
                return true;
            }
            return false;
        }

        public bool AddItemIfEmpty(Item newItem, in int newCount)
        {
            if (IsEmpty)
            {
                count = newCount;
                Item = newItem;
                return true;
            }
            return false;
        }

        public void RemoveItem()
        {
            Item = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsEmpty) return;
            iconImage.rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsEmpty) return;
            var go = eventData.pointerCurrentRaycast.gameObject;
            if (go != null)
            {
                var slot = go.GetComponent<InventorySlot>();
                if (slot != null && slot != this)
                {
                    if (slot.AddItemToStack(Item, count, out int countLeft))
                    {
                        if (countLeft == 0)
                        {
                            RemoveItem();
                        }
                        else
                        {
                            count = countLeft;
                        }
                    } 
                    else
                    {
                        slot.AddItemIfEmpty(Item, count);
                        RemoveItem();
                    }
                    UpdateIcon();
                    slot.UpdateIcon();
                }
            } 
            else
            {
                // Pulled slot out of inventory
            }
            

            iconImage.transform.SetParent(transform, true);
            iconImage.transform.SetAsFirstSibling();
            iconImage.transform.position = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty) return;
            iconImage.transform.SetParent(transform.parent.parent, true);
            iconImage.transform.SetAsLastSibling();
        }
    }
}
