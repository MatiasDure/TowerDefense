using System;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] private SO_Materials _materials;
    [SerializeField] private string _matId;
    [SerializeField] private Renderer _renderer;

    /// <summary>
    /// The ID of the current material.
    /// </summary>
    public string MatID => _matId;

    private void Awake()
    {
        if (!_renderer) throw new NullReferenceException("The MaterialManager needs a rendered!");
    }

    /// <summary>
    /// Finds a material by id
    /// </summary>
    /// <param name="id"> The id of the material to search for </param>
    /// <returns> the material with id passed, otherwise throws an exception </returns>
    /// <exception cref="ArgumentException"> There was no material found with the id passed </exception>
    private Mats FindMaterial(string id)
    {
        foreach (var material in this._materials.materials)
        {
            if (!id.Equals(material._id)) continue;

            return material;
        }

        throw new ArgumentException(String.Format("No material found in {0} with id: {1}. Check the methods referencing the SetMat method located in TowerManagement.",
                                            _materials.name, id));
    }

    /// <summary>
    /// Sets the the renderer material to a material
    /// </summary>
    /// <param name="id"> The id of the material to set </param>
    public void SetMat(string id)
    {
        Mats material = FindMaterial(id);
        
        _renderer.material = material._material;
        _matId = id;
    }
}
