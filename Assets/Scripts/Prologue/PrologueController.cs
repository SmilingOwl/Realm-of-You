using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueController : MonoBehaviour
{
    public void GoToExploration() {
        if (SoundConfig.instance != null) {
            SoundConfig.instance.IncrementRichness(2.0f);
        }
        SceneManager.LoadScene("SampleScene");
    }
}
