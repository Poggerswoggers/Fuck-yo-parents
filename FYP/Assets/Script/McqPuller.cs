using UnityEngine;

[CreateAssetMenu(menuName = "MCQ categories")]
public class McqCategories : ScriptableObject
{
    public string pathName;
    public MCQ[] CategoryList
    {
        get
        {
            return Resources.LoadAll<MCQ>(pathName);
        }
    } 
}
