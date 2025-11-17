// SimpleTestButton.cs
using UnityEngine;
using UnityEngine.UI;

public class SimpleTestButton : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => Debug.Log("TEST BUTTON CLICKED!"));
    }
}