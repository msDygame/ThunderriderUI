var uvAnimationTileX : float= 24;
var uvAnimationTileY : float = 1;
var framesPerSecond : float = 10.0;
var TileScaleX : int = 1;
var TileScaleY : int = 1;

public var nType : int = 0;

function Update () {

    var index : int = Time.time * framesPerSecond;
    
    index = index % (uvAnimationTileX * uvAnimationTileY);
    
    var size = Vector2 (1.0 / uvAnimationTileX, 1.0 / uvAnimationTileY);

    var uIndex = index % uvAnimationTileX;
    var vIndex = index / uvAnimationTileX;

 switch (nType)
 {
  case 0:
   renderer.material.SetTextureOffset ("_MainTex", Vector2(0, -uIndex * size.x));
   break;
   
  case 1:
   renderer.material.SetTextureOffset ("_MainTex", Vector2(-uIndex * size.x, 0));
   break;
   
  case 2:
   renderer.material.SetTextureOffset ("_MainTex", Vector2(0, uIndex * size.x));
   break;
   
  case 3:
   renderer.material.SetTextureOffset ("_MainTex", Vector2(uIndex * size.x, 0));
   break;
   
  default:
   renderer.material.SetTextureOffset ("_MainTex", Vector2(0, -uIndex * size.x));
   break;
 }
 
 renderer.material.SetTextureScale ("_MainTex",  Vector2(TileScaleX,TileScaleY));
}