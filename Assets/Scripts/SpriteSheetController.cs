using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteSheetController : MonoBehaviour
{
    public SpriteAtlas atlas;

    public int spriteSheetSize;

    public string sheetName;

    int currentSprite = 0;

    float timer = 0;


    void Start()
    {
        SetSprite(currentSprite);
    }

    void SetSprite(int i)
    {
        Material mat = this.gameObject.GetComponent<MeshRenderer>().material;

        var sprite = atlas.GetSprite(sheetName + "_" + i.ToString());
        var croppedTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
        croppedTexture.filterMode = FilterMode.Point;
        var pixels = sprite.texture.GetPixels(
                    (int)sprite.textureRect.x,
                    (int)sprite.textureRect.y,
                    (int)sprite.textureRect.width,
                    (int)sprite.textureRect.height
        );
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        this.gameObject.GetComponent<Transform>().localScale = new Vector3(croppedTexture.width/20f, croppedTexture.height/20f, 1f);

        mat.SetTexture("_MainTex", croppedTexture);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 0.08f)
        {
            return;
        }

        timer = 0;
        currentSprite++;
        SetSprite(currentSprite%spriteSheetSize);
    }
}
