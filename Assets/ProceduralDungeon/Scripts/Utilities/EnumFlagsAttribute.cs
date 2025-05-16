using UnityEngine;

public class EnumFlagsAttribute : PropertyAttribute
{
    [SerializeField] string enumName;

    public EnumFlagsAttribute() { }

    public EnumFlagsAttribute(string name)
    {
        enumName = name;
    }
}
