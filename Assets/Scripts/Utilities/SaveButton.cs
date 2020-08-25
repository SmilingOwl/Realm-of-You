using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void savePlayerInfo()
    {
        PlayerSave.instance.SavePlayer();
    }
}
