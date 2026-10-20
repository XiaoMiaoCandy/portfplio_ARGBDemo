using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;//相机跟随移动的对象
    public Vector3 offsetPosition = new Vector3(0,0,-5);

    public float moveSpeed = 3;
    // Update is called once per frame
    private void Start()
    {
        target = Player.Instance.transform;
    }
    void LateUpdate()
    {
        if(target == null)
            return;
        //（1）利用插值运算让摄像机一直位于目标对象的指定位置
        //this.transform.position = Vector3.Lerp(this.transform.position, target.position+offsetPosition, Time.deltaTime);
        //让相机看向目标
        //this.transform.LookAt(target.position);
        //这里虽然会让摄像机看向玩家，但会导致摄像机旋转，会有一种类似 眩晕的效果

        //（2）修改后（取消盯着玩家看），我发现在玩家移动快时，会导致抖动

        this.transform.position = target.position + offsetPosition;
        //（直接将位置移过去）这样可以让摄像机一直跟随玩家，会让玩家在视觉上位置恒定

        //（匀速过去）还是那样还会有抖动，
        // this.transform.position = Vector3.MoveTowards(this.transform.position, target.position + offsetPosition, Time.deltaTime * moveSpeed);
    }
}
