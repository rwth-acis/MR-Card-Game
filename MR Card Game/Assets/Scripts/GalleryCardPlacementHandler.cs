using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCardPlacementHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;
    private float start = Screen.width / 6.5f;
    private float range = Screen.width - Screen.width / 3.25f;
    [SerializeField] private float offset = 0;

    private Vector2 screen_res = new Vector2(Screen.width, Screen.height);

    // Start is called before the first frame update
    void Start()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        offset *= Screen.width / 20;
        start += offset;
        range -= 2 * offset;
        int numberOfCards = cards.Length;
        for (int i = 0; i < numberOfCards; i++)
        {
            RectTransform cardTransform = cards[i].GetComponent<RectTransform>();
            float xPosition = start + i * (range / (numberOfCards - 1));
            cards[i].GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(xPosition, Screen.height / 2.75f, cardTransform.position.z), cardTransform.rotation);
        }
    }
}
