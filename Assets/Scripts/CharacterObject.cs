using UnityEngine;

[CreateAssetMenu(fileName = "CharObject", menuName = "Objects/CharObject")]
public class CharacterObject : ScriptableObject
{
    public Sprite sprite;
    public int CharIndex;
    public bool IsTaken;
    public int CakesToTake;
}
