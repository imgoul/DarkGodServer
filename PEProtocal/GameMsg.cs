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


        public int hp;
        public int ad;//物理攻击
        public int ap;//魔法攻击
        public int addef;//物理防御
        public int apdef;//魔法防御
        public int dodge; //闪避概率
        public int pierce; //穿透比率
        public int critical; //暴击概率
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

    public enum ErrorCode
    {
        None = 0,
        AcctIsOnLine, //账号已经上线
        WrongPass, //密码错误
        NameIsExist, //名字已存在
        UpdateDBError, //更新数据库失败
    }

    public enum CMD
    {
        None = 0,

        //登录相关
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,
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