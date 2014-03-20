/*------------------------------------------------------------------------------

前后台接口管理类：
saveData:保存 用户玩的关卡信息
getFriend:得到所有好友的信息
// </auto-generated>
//------------------------------------------------------------------------------*/
using System;
namespace AssemblyCSharp
{
		public class NetWorkManager
		{


		private static NetWorkManager _instance;
		public static NetWorkManager Instance
		{
			get
			{
				if(_instance==null){
					_instance = new NetWorkManager();
				}
				return _instance;
			}
		}


	/**
	 * 
	 * cmd:SaveData    玩家关卡数据同步 ；  按Home 键退出时 调用； 前台同步数据到后台
	 * 参数：
	 * deviceId
	 * platformId
	 * level
	 * levelScore    关卡分数的最大值
     以逗号 空格分隔  1 200,2 300,3 400,

	*/

		public void SaveData(String deviceId,String platformId,String level,String score)
		{
				
		}

		/***
		 * cmd:getFriend  用户进入地图时调用
		参数：
		platformIds
		以逗号分隔 1,2,3,
		 * 
		 * 
		 */
		public Array GetFriend(String platformIds)
		{
			return null;
			
		}

		}
}

