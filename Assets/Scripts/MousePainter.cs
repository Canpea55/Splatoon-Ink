using System;
using UnityEngine;

public class MousePainter : MonoBehaviour{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    void Update(){

        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (click){
            Vector3 position = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            switch(Input.GetKey(KeyCode.LeftAlt))
            {
                case true:
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Vector2 UVCoord = hit.textureCoord;

                    Paintable p = hit.collider.GetComponent<Paintable>();
                    RenderTexture mask = p.getMask();

                    Texture2D mask2D = new Texture2D(mask.width, mask.height, TextureFormat.RGBA32, false);
                    RenderTexture renderTexture = new RenderTexture(mask.width, mask.height, 32);
                    
                    Graphics.Blit(mask, renderTexture);

                    RenderTexture.active = renderTexture;
                    mask2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                    mask2D.Apply();

                    UVCoord.x *= mask2D.width;
                    UVCoord.y *= mask2D.height;

                    Color a = mask2D.GetPixel(Mathf.RoundToInt(UVCoord.x), Mathf.RoundToInt(UVCoord.y));
                    print(UVCoord + " : " + a.a);
                }
                break;
                
                case false:
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                    // transform.position = hit.point;
                    Paintable p = hit.collider.GetComponent<Paintable>();
                    if(p != null)
                    {
                        PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                    }
                }
                break;
            }
        }

    }

}
