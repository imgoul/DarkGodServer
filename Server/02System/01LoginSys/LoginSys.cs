/****************************************************
	文件：LoginSys.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/31 10:04   	
	功能：登录业务系统
*****************************************************/

using PENet;
using PEProtocal;

public class LoginSys
{
    private static LoginSys instance = null;

    private CacheSvc cacheSvc=null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        cacheSvc=CacheSvc.Instance;
        PETool.LogMsg("LoginSys Init Done.");
    }

    /// <summary>
    /// 响应登录请求
    /// </summary>
    /// <param name="msg"></param>
    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data=pack.Msg.reqLogin;
        
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
            
        };
        //当前账号是否上线
        if(cacheSvc.IsAcctOnLine(data.acct))
        {
            //已上线:返回错误信息
            msg.err=(int)ErrorCode.AcctIsOnLine;
        }
        else
        {
            //未上线:检测账号是否存在
            PlayerData _palyerData=cacheSvc.GetPlayerData(data.acct,data.pass);
            if(_palyerData==null)//账号存在,密码错误
            {
                
                msg.err=(int)ErrorCode.WrongPass;
            }
            else//账号密码正确
            {
                msg.rspLogin=new RspLogin
                {
                    playerData = _palyerData
                };
            }
            
            //缓存账号数据
            cacheSvc.AcctOnline(data.acct,pack.Session,_palyerData);
        }

        //回应客户端
        pack.Session.SendMsg(msg);
    }



    public void ReqRename(MsgPack pack)
    {
        ReqRename data=pack.Msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd=(int)CMD.RspRename
        };

        if(cacheSvc.IsNameExist(data.name))//名字是否存在
        {
            
            //存在:返回错误码
            msg.err=(int)ErrorCode.NameIsExist;

        }
        else
        {   //不存在:更新缓存和数据库,再返回给客户端
            
            //更新缓存
            PlayerData playerData=cacheSvc.GetPlayerDataBySession(pack.Session);

            playerData.name=data.name;

            //更新数据库
            if(!cacheSvc.UpdatePlayerData(playerData.id,playerData))
            {
                msg.err=(int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
                
            }
        }
        pack.Session.SendMsg(msg);
    }

    
    /// <summary>
    /// 清除下线玩家数据
    /// </summary>
    /// <param name="session"></param>
    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AcctOffLine(session);
    }
}