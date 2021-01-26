using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Items
{
    public class InventorySlot : MonoBehaviour, 
        IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [SerializeField] Image iconImage;
        [SerializeField] Item _item;
        [SerializeField] int count = 1;
        [SerializeField] TextMeshProUGUI textMesh;
        [SerializeField] bool isEquipSlot;

        bool hasTextMesh;
        InventoryManager manager;
        GameObject owner;

        private void Awake()
        {
            hasTextMesh = textMesh != null;
            UpdateIcon();
        }

        public void Construct(InventoryManager manager, GameObject owner)
        {
            this.manager = manager;
            this.owner = owner;
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

        public bool IsEquipSlot { get => isEquipSlot; }

        public Item Item
        {
            get => _item;
            set => _item = value;
        }

        public bool IsEmpty { get; private set; }

        public int Count { get => count; set => count = value; }

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
                    var equipItemCheck = !(!(Item is EquipableItem) && slot.IsEquipSlot);
                    if (equipItemCheck)
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
                        else if (slot.AddItemIfEmpty(Item, count))
                        {
                            RemoveItem();
                        }
                        UpdateIcon();
                        slot.UpdateIcon();
                    }
                    else
                    {
                        // Trying to put non equipable item in the equip slot
                        Debug.Log("Can't put it here");
                    }
                }
            } 
            else
            {
                // Pulled slot out of the inventory, probably we would delete it
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty) return;
            Item.Use(manager, owner);
        }
    }
}
