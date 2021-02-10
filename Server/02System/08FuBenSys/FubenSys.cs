/****************************************************
	文件：FubenSys.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月9日 14:47:51	
	功能：副本系统
*****************************************************/

using System.Security.Policy;
using PEProtocal;
using PENet;

public class FubenSys
{
    private static FubenSys instance = null;

    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public static FubenSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FubenSys();
            }

            return instance;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;

        PETool.LogMsg("FubenSys Init Done.");
    }


    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.Msg.reqFBFight;

        GameMsg msg = new GameMsg()
        {
            cmd = (int) CMD.RspFBFight
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.Session);
        int power = cfgSvc.GetMapCfg(data.fbid).power;

        if (pd.fuben < data.fbid)
        {
            msg.err = (int) ErrorCode.ClientDataError;
        }
        else if (pd.power < power)
        {
            msg.err = (int) ErrorCode.LackPower;
        }
        else
        {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                RspFBFight rspFbFight = new RspFBFight()
                {
                    fbid = data.fbid,
                    power = pd.power
                };
                msg.rspFBFight = rspFbFight;
            }
            else
            {
                msg.err = (int) ErrorCode.UpdateDBError;
            }
        }

        pack.Session.SendMsg(msg);
    }
}