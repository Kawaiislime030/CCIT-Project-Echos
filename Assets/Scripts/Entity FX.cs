using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = (Random.Range(-1, 1));
        float randomY = (Random.Range(1.5f, 3));

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        
        newText.GetComponent<TextMeshPro>().text = _text;

    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(.2f);

        sr.material = originalMat;

     
    }
    
    private void RedColorBlink()
    {
        if(sr.color != Color.white) 
            sr.color = Color.white;
        else
            sr.color = Color.red; 
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

}
