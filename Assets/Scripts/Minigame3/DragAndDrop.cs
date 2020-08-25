using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    public GameObject SelectedPiece;
    public GameObject EndMenu;
    int OrderInLayer = 1;
    public int PlacedPieces = 0;

    void Start()
    {
        GameController.instance.changeState("minigame3");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit != null && hit.transform != null && hit.transform.CompareTag("Puzzle"))
            {
                if (!hit.transform.GetComponent<pieces>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    SelectedPiece.GetComponent<pieces>().Selected = true;
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OrderInLayer;
                    OrderInLayer++;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<pieces>().Selected = false;
                SelectedPiece = null;
            }
        }

        if (SelectedPiece != null)
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectedPiece.transform.position = new Vector3(MousePoint.x, MousePoint.y);
        }
        if (PlacedPieces == 36)
        {
            EndMenu.SetActive(true);
            if (SoundConfig.instance != null)
                SoundConfig.instance.ActivateSoftMG();
        }

    }
    public void Continue()
    {
        GameController.instance.returnToExploration();
    }
}