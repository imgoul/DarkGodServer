
using PEProtocal;
using System.Collections.Generic;
using PENet;

public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    //存储当前上线的用户
    private Dictionary<string,ServerSession> onLineAcctDic=new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession,PlayerData> onLineSessionDic=new Dictionary<ServerSession, PlayerData>();
    private DBMgr dbMgr;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        dbMgr=DBMgr.Instance;
        PETool.LogMsg("CacheSvc Init Done.");
    }

    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回对应的账号数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns>密码错误返回null,账号不存在则默认创建新账号</returns>
    public PlayerData GetPlayerData(string acct,string pass)
    {
        return dbMgr.QueryPlayerData(acct,pass);
    }

    /// <summary>
    /// 账号上线,缓存数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="session"></param>
    /// <param name="playerData"></param>
    public void AcctOnline(string acct,ServerSession session,PlayerData playerData)
    {
        onLineAcctDic.Add(acct,session);
        onLineSessionDic.Add(session,playerData);
    }

    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }

    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if(onLineSessionDic.TryGetValue(session,out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 更新数据库玩家信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool UpdatePlayerData(int id,PlayerData playerData)
    {
        
       return dbMgr.UpdatePlayerData(id,playerData);
    }

    
    /// <summary>
    /// 清除缓存中离线玩家数据
    /// </summary>
    /// <param name="session"></param>
    public void AcctOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if(item.Value==session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ=onLineSessionDic.Remove(session);
        PECommon.Log("OffLine Result: SessionId:"+session.SessionID+succ);
    }





    public List<ServerSession> GetOnlineServerSessions()
    {
        List<ServerSession> list = new List<ServerSession>();
        foreach (var item in onLineSessionDic)
        {
            list.Add(item.Key);
        }

        return list;
    }


    public Dictionary<ServerSession, PlayerData> GetOnlineCache()
    {
        return onLineSessionDic;
    }
}

