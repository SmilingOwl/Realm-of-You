using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ChangeImage : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (GameObject.Find("ContinueButton") != null) {
                GameObject.Find("DialogueRunner").GetComponent<DialogueUI>().MarkLineComplete();
            }
        }
    }

    [YarnCommand("set_image")]
    public void SetImage(string image) {
        this.gameObject.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(image);
    }
}
