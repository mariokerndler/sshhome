using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabControl : MonoBehaviour
{
    EventSystem eSystem;

    // Start is called before the first frame update
    void Start()
    {
        eSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = eSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if(next != null)
            {
                InputField inputField = next.GetComponent<InputField>();
                if (inputField != null) inputField.OnPointerClick(new PointerEventData(eSystem));

                eSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eSystem));
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SSHUI.Instance.SendRequest();
        }
    }
}
