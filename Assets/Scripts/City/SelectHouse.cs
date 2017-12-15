using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SelectHouse : MonoBehaviour {

    public enum HouseType { PoorHouse, BasicHouse, RichHouse}
    public HouseType houseType = HouseType.PoorHouse;

    public Button houseButton;

    private void Start()
    {
        houseButton = GetComponent<Button>();
        houseButton.onClick.AddListener(OnSelected);
    }

    void OnSelected()
    {
        House house = Resources.Load("Houses/" + houseType.ToString()) as House;
        
        if(!house.isLocked)
        {
            PlayerManager.Instance.houseSelected = house;
            SceneManager.LoadSceneAsync(2);
        }
            
    }
}
