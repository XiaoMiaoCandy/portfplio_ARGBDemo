using TMPro;
using UnityEngine;

public class QuestTargetSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text trackingText;

    public void RefreshTarget(string description,string progress,bool isComplete)
    {
        targetText.text = description;
        trackingText.text = progress;
        Color c = isComplete ? Color.gray : Color.white;
        targetText.color = c;
        trackingText.color = c;
    }
}
