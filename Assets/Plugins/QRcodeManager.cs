using UnityEngine;
using System.Collections;
using Antares.QRCode;

public class QRcodeManager : MonoBehaviour {
    public static string QREncodeText = "1;3;32500;0000;0;0;;0;1;aiwi-rd;aibelive12345;2;1;1;Connectify-Jason;11111111;2;0;0;;;0;1;10.15.20.57;1;0;10.15.20.11;0;;1;0;;;";
    private static Texture2D QREncodeTexture = null;
    public static string _textToEncode = "";

	// Use this for initialization
	//void Start () {
	
	//}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	
	public static Texture2D QREncode(string _textToEncode, int _width, int _height)
	{
		if(_textToEncode=="")
			_textToEncode = "QR text is empty.";
        if (_textToEncode == QREncodeText && QREncodeTexture!=null)
            return QREncodeTexture;
        QREncodeText = _textToEncode;
        QREncodeTexture = QRCodeProcessor.Encode(_textToEncode, _width, _height);
		//Debug.Log("QREncode imgae maker:: " + _textToEncode);
        return QREncodeTexture;
	}
	
	public static string QRDecode(Texture2D qrCodeImage)
	{
		string qrCodeString;
		if(qrCodeImage!=null)
			qrCodeString = QRCodeProcessor.Decode(qrCodeImage).Text;
		else
			qrCodeString = "";
		return qrCodeString;
	}
	
}
