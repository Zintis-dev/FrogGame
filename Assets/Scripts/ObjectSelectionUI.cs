using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectSelectionUI : MonoBehaviour
{
    [SerializeField] private ObjectDatabaseSO objectDatabase;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;

    private List<Button> buttons = new List<Button>();
    private int selectedIndex = 0;

    public int SelectedIndex => selectedIndex;
    public ObjectData SelectedObject => objectDatabase.objectData[selectedIndex];

    private void Start()
    {
        CreateButtons();
        UpdateSelectionUI();
    }

    private void CreateButtons()
    {
        Debug.Log("Creating buttons");
        for (int i = 0; i < objectDatabase.objectData.Count; i++)
        {
            int index = i;
            GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonGO.GetComponent<Button>();
            buttons.Add(button);

            Image icon = buttonGO.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = objectDatabase.objectData[i].Icon;

            button.onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        selectedIndex = index;
        UpdateSelectionUI();
    }

    private void UpdateSelectionUI()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Transform selectionFrame = buttons[i].transform.Find("SelectionFrame");
            selectionFrame.gameObject.SetActive(i == selectedIndex);
        }
    }
}
