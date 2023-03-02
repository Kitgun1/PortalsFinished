using UnityEngine;

[CreateAssetMenu(fileName = "GunObject", menuName = "Objects/GunObject")]
public class GunObject : ScriptableObject
{
    public Sprite sprite;
    public int GunIndex;
    public bool IsTaken;
    public int AdWatching;
    public int AdToTake;
}
