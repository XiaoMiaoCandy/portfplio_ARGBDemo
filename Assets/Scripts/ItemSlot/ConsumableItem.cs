using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 可以被消耗的道具，该脚本挂载到对应的预设体上
/// 表示地上的物品
/// </summary>
public class ConsumableItem : MonoBehaviour
{
    private SpriteRenderer consumableItemSprite;
    private Animator consumableItemAnim;
    //该道具的数量
    public int nums;
    public bool canPickUp1 =true;
    //关联的道具基类
    public ItemObject itemObject;
    //该道具特有的字段
    //合成该物品的道具 和 用到 的数量
    private bool isDestory = false;
    //是否被销毁
    public Dictionary<ItemObject, int> recipe;

    private void Awake()
    {
        consumableItemSprite = GetComponent<SpriteRenderer>();
        consumableItemAnim = GetComponent<Animator>();
        if(consumableItemSprite != null )
        {
            //进行初始化
            consumableItemSprite.sprite = itemObject.itemSprite;
            this.name = itemObject.name;
        }
       
    }
    private void OnValidate()
    {
        //检测是否关联了具体物品
        if(itemObject == null)
            return;
        //为预设体添加图像
        if (consumableItemAnim != null)
        {
            consumableItemSprite.sprite = itemObject.itemSprite;
            this.name = itemObject.name;
            print("物品更换成功");
        }

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&canPickUp1)
        {
            this.canPickUp1 = false;

            ConsumableItem saveItem = Instantiate(this);
            saveItem.transform.SetParent(InvertoryMgr.Instance.transform);
            saveItem.gameObject.SetActive(false); // 隐藏
            //====================
            // 这里我发现真正的道具实列，只会出现在背包里一小会，然后就变成了missing,这是因为该道具后面被销毁的原因
            // 解决方法为：在背包里存储一个克隆体
            // 但我想如果有多个同类的物体，应该会有很大的浪费
            //====================
            InvertoryMgr.Instance.PickUpItem(itemObject,saveItem, nums);
            print("捡到了：" + itemObject.name + "X 1");
            consumableItemAnim.Play("PickUpItem");
            Destroy(gameObject,1f);
            isDestory = true;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //====================
        //这里我发现我的玩家拾取时有概率重复进行，导致数量越来越多，所以我改为等道具彻底销毁后才可以进行出现的捡起
        //====================
        if (other.CompareTag("Player")&&!isDestory)
        {
            Invoke("RePickup", 1.5f);
        }
    }
    private void RePickup()
    {
        this.canPickUp1 = true;
    }

    #region 道具的表现效果
    //道具的效果，具体效果由道具的具体脚本实现
    [SerializeField]
    public UnityEvent itemEffect;
    #endregion
}
