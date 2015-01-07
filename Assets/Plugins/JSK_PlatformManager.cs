//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;

//public class JSK_PlatformManager : MonoBehaviour
//{
//    static public bool g_LaunchAiwi = true;
	
//    #if UNITY_ANDROID
//    [DllImport("dygamejavabridge")]
//    private static extern IntPtr getCacheDir();
	
//    [DllImport ("dygamejavabridge")]
//    private static extern int launchGameZone([MarshalAs(UnmanagedType.LPStr)]string sPackageName, [MarshalAs(UnmanagedType.LPStr)]string sGameName); 
	
//    [DllImport("dygamejavabridge")]
//    private static extern IntPtr getParameter([MarshalAs(UnmanagedType.LPStr)]string sPara);
	
//    private string sParaString = "Push to get parameter";
//    private int iLaunchRtn = -1;
//    #endif
	
//    public void LaunchAIWI()
//    {
//        #if UNITY_ANDROID
//        Debug.Log("before call getParameter");
//        IntPtr stringPtr = getParameter("FromGameZone");
//        //IntPtr stringPtr = getParameter("FromAIWI");
//        Debug.Log("stringPtr = " + stringPtr);
//        sParaString = Marshal.PtrToStringAnsi(stringPtr);
//        Debug.Log("getParameter returned:" + sParaString);
//        if( sParaString.Length > 0 )
//        {
//            //Launch by AIWI
//        }
//        else
//        {
//            //iLaunchRtn = launchAIWI("com.aibelive.aiwi", "com.dygame.waverider");
///* 20141013 Fix for Demo 
//            iLaunchRtn = launchGameZone("com.dygame.gamezone2","com.dygame.waverider");
//            Debug.Log("iLaunchRtn:" + iLaunchRtn);
//            if( iLaunchRtn == 0 )
//            {
//                //Threading.Thread.Sleep(1000);
//                //Application.Quit();
//            }
//*/			
//        }
//        #endif
//    }
//}
