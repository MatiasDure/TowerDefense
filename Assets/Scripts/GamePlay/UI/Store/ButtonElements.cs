using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A struct that helps group relevant information about each store button
/// </summary>
[System.Serializable]
public struct ButtonElements
{
    [SerializeField] Image _image;
    [SerializeField] string _name;
    [SerializeField] TextMeshProUGUI _textObj;
    [SerializeField] int _cost;

    /// <summary>
    /// Gets the image component of the button.
    /// </summary>
    public Image Img => _image;

    /// <summary>
    /// Gets the name of the button element.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Gets the text object of the button element.
    /// </summary>
    public TextMeshProUGUI TextObj => _textObj;

    /// <summary>
    /// Gets the cost of the store button element.
    /// </summary>
    public int Cost => _cost;
}
