using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomContentFiller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextPrefab;
    [SerializeField]
    private float SpawnSpeed = 10;
    [SerializeField]
    private Transform Parent;

    private Coroutine SpawningCoroutine;
    private List<TextMeshProUGUI> ActiveTexts = new List<TextMeshProUGUI>();

    public void StartFillingContent()
    {
        SpawningCoroutine = StartCoroutine(SpawnText());
    }

    public void StopFillingContent()
    {
        StopCoroutine(SpawningCoroutine);

        foreach(TextMeshProUGUI text in ActiveTexts)
        {
            Destroy(text.gameObject);
        }
    }

    private IEnumerator SpawnText()
    {
        WaitForSeconds Wait = new WaitForSeconds(1 / SpawnSpeed);
        while(true)
        {
            TextMeshProUGUI text = Instantiate(TextPrefab);
            text.SetText(Random.value > 0.5f ? "1" : "0");
            text.rectTransform.SetParent(Parent, false);
            text.rectTransform.anchoredPosition = new Vector2(Random.Range(25, Screen.width - 25), 25);
            text.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 100, ForceMode2D.Impulse);
            ActiveTexts.Add(text);
            yield return Wait;
        }
    }
}
