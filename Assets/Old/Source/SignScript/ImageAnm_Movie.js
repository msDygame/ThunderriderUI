
var  tex1:Texture2D;
var  tex2:Texture2D;
private var  index:int=1;
private var NextTime = 0.0;
var T = 1.0;

function Update () 
{
	if ( Time.time>NextTime )
	{
		if(index==1)  
			gameObject.renderer.material.mainTexture = tex1; 
		else 
			gameObject.renderer.material.mainTexture = tex2; 
			
		index++;
		if ( index>2) index=1;
		NextTime=Time.time+T*0.3+Random.value*T*0.7;
	}
}