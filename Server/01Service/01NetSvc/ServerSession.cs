/****************************************************
	文件：ServerSession.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/31 10:38   	
	功能：网络会话连接
*****************************************************/


using PENet;
using PEProtocal;

public class ServerSession : PESession<GameMsg>
{
    public int SessionID;
    protected override void OnConnected()
    {
        SessionID=ServerRoot.Instance.GetSessionID();
        PECommon.Log("SessionID:"+SessionID+" Client Connect");
        
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("SessionID:"+SessionID+" Client DisConnect.");
        
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID:"+SessionID+" RcvPack CMD:" +((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(new MsgPack(this,msg));
       
    }
}

