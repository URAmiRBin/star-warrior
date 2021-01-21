using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBG : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] Texture myTex = null;
    private float offset = 0;

    Renderer rend = null;
    private void Start() {
        rend = GetComponent<Renderer>();
        if (myTex != null){
            rend.material.mainTexture = myTex;
        }
    }
    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }


}
