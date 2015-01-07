package com.aibelive.CheckEnter;

import com.unity3d.player.UnityPlayerActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;

public class CheckEnterActivity extends UnityPlayerActivity {
	
	private static final int MAX_KEY_COUNT = 10;
	
	private static int[] mDeviceId = new int[MAX_KEY_COUNT];
	private static int[] mKeyCode = new int[MAX_KEY_COUNT];
	private static int[] mKeySatus = new int[MAX_KEY_COUNT];
	
	private static int mIndex = 0;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
    	super.onCreate(savedInstanceState);
        initParams();
    }
    
	private void initParams() {
		mIndex=0;
		for (int i = 0 ; i < MAX_KEY_COUNT ; ++i) 
		{
			mKeySatus[i]=0;
		}
	}
	
	public static String getKeyState() {
		String value = "";
		for (int i = 0 ; i < MAX_KEY_COUNT ; ++i) 
		{
			if (mKeySatus[i] != 0)
			{
				value+=mDeviceId[i]+":"+mKeyCode[i]+":"+mKeySatus[i]+",";
				mKeySatus[i]=0;
			}
		}
		mIndex=0;
		return value;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		//Log.d(DEBUG_TAG, String.format("[onKeyDown] deviceId:%d keyCode:%d", event.getDeviceId(), event.getKeyCode()));
		
		mDeviceId[mIndex] = event.getDeviceId();
		mKeyCode[mIndex] = event.getKeyCode();
		mKeySatus[mIndex] = 1;
		mIndex++;
		if (mIndex>=MAX_KEY_COUNT) mIndex=0;
		return super.onKeyDown(keyCode, event);
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent event) {
		mDeviceId[mIndex] = event.getDeviceId();
		mKeyCode[mIndex] = event.getKeyCode();
		mKeySatus[mIndex] = 2;
		mIndex++;
		if (mIndex>=MAX_KEY_COUNT) mIndex=0;
		return super.onKeyUp(keyCode, event);
	}

}