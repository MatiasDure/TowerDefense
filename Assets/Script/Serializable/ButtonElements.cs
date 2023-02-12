using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ButtonElements
{
    [SerializeField] Image _image;
    [SerializeField] string _name;
    [SerializeField] TextMeshProUGUI _textObj;
    [SerializeField] int _cost;

    public Image Img => _image;
    public string Name => _name;
    public TextMeshProUGUI TextObj => _textObj;
    public int Cost => _cost;
}
