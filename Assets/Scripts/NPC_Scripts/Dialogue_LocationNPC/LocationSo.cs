using UnityEngine;

[CreateAssetMenu(menuName = "newLocation")]
public class LocationSo : ScriptableObject
{
    public string locationID;   //该区域的归属区块号
    public string locationName; //该区域的名字
}
