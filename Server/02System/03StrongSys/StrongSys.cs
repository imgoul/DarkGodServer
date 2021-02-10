/****************************************************
	文件：StrongSys.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月6日 16:42:46	
	功能：强化升级系统
*****************************************************/

using PEProtocal;

public class StrongSys
{
    private static StrongSys instance = null;

    private CacheSvc cacheSvc = null;

    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
            }

            return instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("StrongSys Init Done");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.Msg.reqStrong;


        GameMsg msg = new GameMsg()
        {
            cmd = (int) CMD.RspStrong
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.Session);
        int currentStarLv = pd.strongArr[data.pos];
        StrongCfg nextStrongCfg = CfgSvc.Instance.GetStrongCfg(data.pos, currentStarLv + 1);
        //条件判断
        if (pd.lv < nextStrongCfg.minLv)
        {
            msg.err = (int) ErrorCode.LackLevel;
        }
        else if (pd.coin < nextStrongCfg.coin)
        {
            msg.err = (int) ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextStrongCfg.crystal)
        {
            msg.err = (int) ErrorCode.LackCrystal;
        }
        else
        {
            //任务进度数据更新
            TaskSys.Instance.CalcTaskPrgs(pd,3);
            
            //资源扣除
            pd.coin -= nextStrongCfg.coin;
            pd.crystal -= nextStrongCfg.crystal;

            //星级+1
            pd.strongArr[data.pos] += 1;

            //增加属性
            pd.hp += nextStrongCfg.addhp;
            pd.ad += nextStrongCfg.addhurt;
            pd.ap += nextStrongCfg.addhurt;
            pd.addef += nextStrongCfg.adddef;
            pd.apdef += nextStrongCfg.adddef;
        }

        //更新数据库
        if (!cacheSvc.UpdatePlayerData(pd.id, pd))
        {
            msg.err = (int) ErrorCode.UpdateDBError;
        }
        else
        {
            msg.rspStrong = new RspStrong()
            {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strongArr = pd.strongArr
            };
        }

        pack.Session.SendMsg(msg);
    }
}