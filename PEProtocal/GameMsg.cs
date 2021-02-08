/****************************************************
	文件：GameMsg.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/31 10:19   	
	功能：网络通信协议(客户端服务器端公用)
*****************************************************/

using System;
using PENet;

namespace PEProtocal
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        
    }

    #region 登录相关

    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;
        


        public int hp;
        public int ad; //物理攻击
        public int ap; //魔法攻击
        public int addef; //物理防御
        public int apdef; //魔法防御
        public int dodge; //闪避概率
        public int pierce; //穿透比率
        public int critical; //暴击概率
        public int guideid; //任务引导id
        public int[] strongArr;//下标：装备位置,0：头盔     内容：星级
        
        public long time;//时间：用来计算离线时体力恢复
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }

    #endregion


    #region 引导相关

    [Serializable]
    public class ReqGuide
    {
        public int guideid;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }

    
    
    
    #endregion

    #region 强化相关

    [Serializable]
    public class ReqStrong
    {
        public int pos;
    }

    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;

    }
    #endregion



    #region 聊天相关

    [Serializable]
    public class SndChat
    {
        public string chat;
    }

    [Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }

    [Serializable]
    public class PshPower
    {
        public int power;
    }

    #endregion




    #region 资源交易相关

    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
        
    }

    #endregion
    public enum ErrorCode
    {
        None = 0,
        AcctIsOnLine, //账号已经上线
        WrongPass, //密码错误
        NameIsExist, //名字已存在
        UpdateDBError, //更新数据库失败
        
        ServerDataError,//服务器数据异常
        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
        
    }

    public enum CMD
    {
        None = 0,

        //登录相关
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,


        //主城相关
        ReqGuide = 200,
        RspGuide = 201,
        
        ReqStrong=203,
        RspStrong=204,
        
        
        SndChat=205,
        PshChat=206,
        
        
        ReqBuy=207,
        RspBug=208,
        
        PshPower=209,
        
        
        
    }

    public class SrvCfg
    {
        public const string SrvIp = "127.0.0.1";
        public const int SrvPort = 17666;
    }

    public class ClientCfg
    {
        public const string ClientConnectIp = "127.0.0.1";
        public const int ClientConnectPort = 17666;
    }
}