using UnityEngine;
public class Effect_Mushroom : MonoBehaviour
{
    public void AddHealthEffect()
    {
        int healValue = 30;
        int currentHp = Player.Instance.currentHp;
        int maxHp = Player.Instance.maxHp;
        currentHp = Mathf.Min(currentHp + healValue, maxHp);
        Player.Instance.currentHp = currentHp;
        GamePanel_UI.Instance.UpdataStatic_HpUI();
        Debug.Log($"使用了道具，恢复了 {healValue} 点生命值，当前血量是{currentHp}");
    }
}