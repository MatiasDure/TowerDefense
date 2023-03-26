using System;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] SO_Materials materials;
    [SerializeField] string _matId;
    [SerializeField] Renderer rend;

    public string MatID => _matId;

    private void Awake()
    {
        if (!rend) throw new NullReferenceException("The MaterialManager needs a rendered!");
    }

    public void SetMat(string id)
    {
        foreach (var material in this.materials.materials)
        {
            if (id.Equals(material._id))
            {
                rend.material = material._material;
                return;
            }
        }
        throw new ArgumentException(String.Format("No material found in {0} with id: {1}. Check the methods referencing the SetMat method located in TowerManagement.",
                                            materials.name, id));
    }
}
