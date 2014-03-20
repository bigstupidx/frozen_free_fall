package com.mfp.android.sns;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.util.Log;
import android.widget.Toast;

import com.qihoo.gamecenter.sdk.buildin.Matrix;
import com.qihoo.gamecenter.sdk.buildin.activity.ContainerActivity;
import com.qihoo.gamecenter.sdk.common.IDispatcherCallback;
import com.qihoo.gamecenter.sdk.protocols.ProtocolConfigs;
import com.qihoo.gamecenter.sdk.protocols.ProtocolKeys;
import com.qihoo.gamecenter.sdk.protocols.snapshot.ISnapshot;
import com.qihoo.gamecenter.sdk.protocols.snapshot.ISnapshotCallback;
import com.qihoo.gamecenter.sdk.protocols.snapshot.SnapshotMgr;
import com.unity3d.player.UnityPlayer;

public class QiHooSocialManager {

	
	private static Activity mActivity;
	private static final String TAG = "QiHooSocialManager";
	
	  private static String _onPayResult;
	  private static String _gameObject;
	  private static boolean mIsInOffline = false;
	/**
	 * 
	 * 玩家 360登录； 返回玩家id、名称、性别等信息
	 * @param _activity
	 * @param _mobileway
	 */
	public static void login(Activity _activity,String gameObject,String onResult)
	{
		mActivity = _activity;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	Log.d(TAG, "start 360 login  ,gameObject" + gameObject +",onresult:"+onResult );
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					doSdkLogin(false,false,null,true);
				}catch(Exception e){
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  null);
					Log.e(TAG, e.toString());
				}
			}
		});
    	Log.d(TAG, "end 360 login  ,gameObject" + gameObject +",onresult:"+onResult );		
	}
	
	/**
	 * 
	 * 得到游戏内好友信息；
	 * @param _activity
	 * @param _mobileway
	 */
	public static void getGameFriends(Activity _activity,String gameObject,String onResult)
	{
		mActivity = _activity;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	Log.d(TAG, "start 360 getGameFriends  ,gameObject" + gameObject +",onresult:"+onResult );
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					doSdkGetGameFriend();
				}catch(Exception e){
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  null);
					Log.e(TAG, e.toString());
				}
			}
		});
    	Log.d(TAG, "end 360 getGameFriends  ,gameObject" + gameObject +",onresult:"+onResult );		
	}
	/**
	 * 
	 * 检查获取访问通讯录好友；
	 * @param _activity
	 * @param _mobileway
	 */
	public static void sdkGetContactContent(Activity _activity,String gameObject,String onResult)
	{
		mActivity = _activity;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	Log.d(TAG, "start 360 sdkGetContactContent  ,gameObject" + gameObject +",onresult:"+onResult );
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					doSdkGetContactContent(false);
				}catch(Exception e){
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  null);
					Log.e(TAG, e.toString());
				}
			}
		});
    	Log.d(TAG, "end 360 sdkGetContactContent  ,gameObject" + gameObject +",onresult:"+onResult );		
	}
	
	/**
	 * 
	 * 上传游戏积分
	 * @param _activity
	 * @param _mobileway
	 */
	private static String mTopId;
	private static String mScore;
	public static void uploadScore(Activity _activity,String gameObject,String onResult,String topId,String score)
	{
		mActivity = _activity;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	mTopId = topId;
    	mScore = score;
    	Log.d(TAG, "start 360 uploadScore  ,gameObject" + gameObject +",onresult:"+onResult+",topId:"+topId+",score:"+score );
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					doSdkUploadScore(mTopId,mScore);
				}catch(Exception e){
					
					Log.e(TAG, e.toString());
				}
			}
		});
    	Log.d(TAG, "end 360 uploadScore  ,gameObject" + gameObject +",onresult:"+onResult );		
	}
	
	/**
	 * 
	 * 弹出悬浮框；
	 * @param _activity
	 * @param _mobileway
	 */
	public static void showFloatWnd(Activity _activity,String gameObject,String onResult)
	{
		mActivity = _activity;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	Log.d(TAG, "start 360 showFloatWnd  ,gameObject" + gameObject +",onresult:"+onResult );
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					doSdkShowFloatWnd(false,true);
				}catch(Exception e){
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  null);
					Log.e(TAG, e.toString());
				}
			}
		});
    	Log.d(TAG, "end 360 showFloatWnd  ,gameObject" + gameObject +",onresult:"+onResult );		
	}
	
	
	private static void doSdkShowFloatWnd( boolean bLandScape, boolean show) {
        
//        initSnapShotInterface();
        Intent intent = getShowFloatWndIntent(bLandScape, show);
        Matrix.execute(mActivity, intent, new IDispatcherCallback() {
            
            @Override
            public void onFinished(String data) {
            	UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  data);
            	System.out.println(data);
            }
        });
    }
    
    private static Intent getShowFloatWndIntent(boolean bLandScape, boolean show) {
        Intent intent = new Intent();
        // function code
        intent.putExtra(ProtocolKeys.FUNCTION_CODE, ProtocolConfigs.FUNC_CODE_SHOW_FLOW_WND);
        // 屏幕方向
        intent.putExtra(ProtocolKeys.IS_SCREEN_ORIENTATION_LANDSCAPE, bLandScape);
        // 应用名称
        intent.putExtra(ProtocolKeys.APP_NAME, "冰雪奇缘");
        // 显示还是关闭
        intent.putExtra(ProtocolKeys.SHOW, show);
        // 悬浮窗菜单选项
        intent.putStringArrayListExtra(ProtocolKeys.FLOAT_WND_MENU_ITEMS, getFloatWndMenuItemParam());
        
        return intent;
    }
    
    private static ArrayList<String> getFloatWndMenuItemParam() {
        ArrayList<String> ret = new ArrayList<String>();
        ret.add(ProtocolKeys.FLOAT_WND_MENU_ITEM_BBS);
        ret.add(ProtocolKeys.FLOAT_WND_MENU_ITEM_CUSTOMSERVICE);
        ret.add(ProtocolKeys.FLOAT_WND_MENU_ITEM_MESSAGE);
        ret.add(ProtocolKeys.FLOAT_WND_MENU_ITEM_SNAPSHOT);
        ret.add(ProtocolKeys.FLOAT_WND_MENU_ITEM_HIDEICON);
        return ret;
    }
	/**
	 *   // ----------------------结合本地通信录获取可邀请好友列表-----------------
	 */ 
	 
    private static  void doSdkGetContactContent( boolean isLandScape ){
        
        Intent intent = getGetContactContentIntent(isLandScape);
        Matrix.execute(mActivity, intent, new IDispatcherCallback() {
            @Override
            public void onFinished(String data) {
//                Toast.makeText(mActivity, data, Toast.LENGTH_SHORT).show();
            	UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  data);
                System.out.println(data);
            }
        });
    }
    
    private static Intent getGetContactContentIntent(boolean isLandScape){
        Intent intent = new Intent();
        /*
         * 必须参数：
         *  function_code : 必须参数，表示调用SDK接口执行的功能
         *  screen_orientation : 必须参数，屏幕方向
         *  start : 非必须参数，表示此次获取可邀请好友列表的开始位置，从0开始。
         *          无此参数时会获取所有的可邀请好友列表，不进行翻页。
         *  count : 非必须参数，表示此次获取可邀请好友的个数。
         *          无此参数时会获取所有的可邀请好友列表，不进行翻页。
        */
        intent.putExtra(ProtocolKeys.IS_SCREEN_ORIENTATION_LANDSCAPE, isLandScape);
        intent.putExtra(ProtocolKeys.FUNCTION_CODE, ProtocolConfigs.FUNC_CODE_GET_CONTACT_CONTENT);
        // 界面相关参数，360SDK登录界面背景是否透明。
        intent.putExtra(ProtocolKeys.IS_LOGIN_BG_TRANSPARENT, false);
        return intent;
    }
	
    // ------------------------上传积分接口--------------------
    // usrInfo : 必须参数，用户信息
    // tokeninfo : 必须参数，用户token信息
	private static String mUploadReturnData;
    protected static String doSdkUploadScore(String topId,String score){
 
        Intent intent = getUploadScoreIntent(topId,score);
        Matrix.execute(mActivity, intent, new IDispatcherCallback() {
            
            @Override
            public void onFinished(String data) {
            	mUploadReturnData = data;
            	UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  data);
                System.out.println(data);
            }
        });
        return mUploadReturnData;
    }
    
    // token : 必须参数，用户token信息
    private static Intent getUploadScoreIntent(String topId,String score){
        Intent intent = new Intent();
        
        // 从界面获取用户输入的数据
        /*
         * 必须参数：
         *  function_code : 必须参数，表示调用SDK接口执行的功能为上传积分
         *  score : 用户积分。
         *  topnid : 排行榜标识
        */
        intent.putExtra(ProtocolKeys.FUNCTION_CODE, ProtocolConfigs.FUNC_CODE_UPLOAD_SCORE);
        intent.putExtra(ProtocolKeys.SCORE, score/*"100"*/);
        intent.putExtra(ProtocolKeys.TOPNID, topId);
        return intent;
    }
    // ---------------------------------------------获取游戏好友接口-------------------------
    // usrInfo : 必须参数，用户信息
    // tokeninfo : 必须参数，用户token信息
    protected static void doSdkGetGameFriend(){
        // 检查用户是否登录
        Intent intent = getGetGameFriendIntent();
        Matrix.execute(mActivity, intent, new IDispatcherCallback() {
            
            @Override
            public void onFinished(String data) {
            	UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  data);
//                Toast.makeText(SdkUserBaseActivity.this, data, Toast.LENGTH_SHORT).show();
            }
        });
    }

    /**
     * 使用360SDK的登录接口
     * 
     * @param isLandScape 是否横屏显示登录界面
     * @param isBgTransparent 是否以透明背景显示登录界面
     * @param clientId 即AppKey
     * @param isShowCloseIcon 是否显示关闭按钮 默认true
     * @param isForce 是否强制登陆 默认false
     */
    protected static void doSdkLogin(boolean isLandScape, boolean isBgTransparent, String clientId,boolean isShowCloseIcon) {
    	   mIsInOffline = false;
    	   Log.d(TAG, "start doSdkLogin" );
        Intent intent = getLoginIntent(isLandScape, isBgTransparent, clientId,isShowCloseIcon, true, false);
        Matrix.invokeActivity(mActivity, intent, mLoginCallback);
    	Log.d(TAG, "end getLoginIntent" );
    }
    /***
     * 生成调用360SDK登录接口的Intent
     * 
     * @param isLandScape 是否横屏
     * @param isBgTransparent 是否背景透明
     * @param clientId 即AppKey
     * @param isShowCloseIcon 是否显示关闭按钮 默认true
     * @param isSupportOffline 是否支持离线模式
     * @param showSwitch 是否在自动登录过程中显示切换账号
     * @return Intent
     */
    private static Intent getLoginIntent(boolean isLandScape, 
                        boolean isBgTransparent, 
                        String clientId,
                        boolean isShowCloseIcon, 
                        boolean isSupportOffline , 
                        boolean showSwitch) {
    	Log.d(TAG, "start getLoginIntent" );
        Intent intent = new Intent(mActivity, ContainerActivity.class);

        // 界面相关参数，360SDK界面是否以横屏显示。
        intent.putExtra(ProtocolKeys.IS_SCREEN_ORIENTATION_LANDSCAPE, isLandScape);

        // 界面相关参数，360SDK登录界面背景是否透明。
        intent.putExtra(ProtocolKeys.IS_LOGIN_BG_TRANSPARENT, isBgTransparent);

        //是否显示关闭按钮
        intent.putExtra(ProtocolKeys.IS_LOGIN_SHOW_CLOSE_ICON, isShowCloseIcon);

        // 必需参数，使用360SDK的登录模块。
        intent.putExtra(ProtocolKeys.FUNCTION_CODE, ProtocolConfigs.FUNC_CODE_LOGIN);
        
        // 可选参数，是否支持离线模式，默认值为false
        intent.putExtra(ProtocolKeys.IS_SUPPORT_OFFLINE, isSupportOffline);
        
        // 可选参数，是否在自动登录的过程中显示切换账号按钮，默认为false
        intent.putExtra(ProtocolKeys.IS_SHOW_AUTOLOGIN_SWITCH, showSwitch);
        
        return intent;
    }
    // 登录、注册的回调
    private static IDispatcherCallback mLoginCallback = new IDispatcherCallback() {

        @Override
        public void onFinished(String data) {
            // press back
        	String retData = data;
        	
            Log.e(TAG, "mLoginCallback, data is " + data);
            UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, retData );
            // 解析token
//            onGotTokenInfo(ti);
            // 解析User info
//            QihooUserInfo info = parseUserInfoFromLoginResult(data);
            
            // 显示一下登录结果
        }
    };
    // token : 必须参数，用户token信息
    private static Intent getGetGameFriendIntent(){
        Intent intent = new Intent();
        
        String strStart ="0";
        String strCount ="100";
        /*
         * 必须参数：
         *  function_code : 必须参数，表示调用SDK接口执行的功能为获取游戏好友。
         *  start : 可选参数，表示从第几个好友开始获取。从0开始。
         *  coutn : 可选参数，表示要获取的好友数量。
         *          start和count参数如果不传的话，会返回最多1000个好友。
        */
        intent.putExtra(ProtocolKeys.FUNCTION_CODE, ProtocolConfigs.FUNC_CODE_GET_GAME_FRIENDS);
        intent.putExtra(ProtocolKeys.START, strStart);
        intent.putExtra(ProtocolKeys.COUNT, strCount);
        
        return intent;
    }
	
}
