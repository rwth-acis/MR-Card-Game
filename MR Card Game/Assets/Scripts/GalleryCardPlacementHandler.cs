using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCardPlacementHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;
    private float start = Screen.width / 4;
    private float range = Screen.width - Screen.width / 3;
    [SerializeField] private float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        start += offset;
        range -= 2*offset;
        int numberOfCards = cards.Length;
        for(int i = 0; i< numberOfCards;i++)
        {
            RectTransform cardTransform = cards[i].GetComponent<RectTransform>();
            float xPosition = start + i * (range / (numberOfCards-1));
            cards[i].GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(xPosition, Screen.height/3, cardTransform.position.z), cardTransform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
