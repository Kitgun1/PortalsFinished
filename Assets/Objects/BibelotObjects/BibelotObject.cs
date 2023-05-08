using UnityEngine;

[CreateAssetMenu(fileName = "BibelotObject", menuName = "Objects/BibelotObject")]
public class BibelotObject : ScriptableObject
{
    public Sprite Sprite;
    public int BibelotIndex;
    public bool IsTaken;
    public int Janov;
}