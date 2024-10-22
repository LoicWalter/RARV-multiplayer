using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
  public float scrollX;
  public float scrollY;
  private new Renderer renderer;

  private void Start()
  {
    renderer = GetComponent<Renderer>();
  }
  private void Update()
  {
    float OffsetX = scrollX * Time.time;
    float OffsetY = scrollY * Time.time;

    renderer.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
  }

}
