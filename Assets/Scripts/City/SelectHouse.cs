using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class SelectHouse : MonoBehaviour, IPointerEnterHandler {

    public enum HouseType { PoorHouse, BasicHouse, RichHouse}
    public HouseType houseType = HouseType.PoorHouse;

    public Button houseButton;

    private bool SelectionPlayed = false;

    private void Start()
    {
        houseButton = GetComponent<Button>();
        houseButton.onClick.AddListener(OnSelected);

        AudioClip sfx;
        // ADRIEN
        sfx = Resources.Load("Sounds/Ambiance") as AudioClip;
        SoundManager.PlaySFX(sfx);
        SelectionPlayed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ADRIEN
        AudioClip sfx;
        sfx = Resources.Load("Sounds/Bell") as AudioClip;
        SoundManager.PlaySFXRandomized(sfx);
    }

    void OnSelected()
    {
        House house = Resources.Load("Houses/" + houseType.ToString()) as House;

        if (!house.isLocked)
        {
            if (!SelectionPlayed)
            {
                SelectionPlayed = true;
                // ADRIEN
                AudioClip sfx;
                sfx = Resources.Load("Sounds/DoorOpened") as AudioClip;
                SoundManager.PlaySFXRandomized(sfx);
            }

            PlayerManager.Instance.houseSelected = house;
            SceneManager.LoadSceneAsync(2);
        }
            
    }
}
