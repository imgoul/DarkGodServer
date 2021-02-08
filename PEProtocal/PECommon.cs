/****************************************************
	文件：PECommon.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/31 11:14   	
	功能：客户端服务器端公共工具类
*****************************************************/

using PENet;
using PEProtocal;

public enum  LogType
{
    Log=0,Warn=1,Error=2,Info=3
}

public class PECommon
{
    public static void Log(string msg="",LogType tp=LogType.Log)
    {
        LogLevel lv=(LogLevel)tp;
        PETool.LogMsg(msg,lv);
    }

    /// <summary>
    /// 获取战斗力
    /// </summary>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv*100+pd.ad+pd.ap+pd.addef+pd.apdef;
    }

    /// <summary>
    /// 获取体力值
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetPowerLimit(int lv)
    {
        return (lv-1)/10*150+150;
    }

    
    /// <summary>
    /// 获取升级经验上限
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetExpUpValByLv(int lv)
    {
        return 100*lv*lv;
    }


    public const int PowerAddSpace = 5;//分钟
    public const int PowerAddCount = 2;//分钟
}

