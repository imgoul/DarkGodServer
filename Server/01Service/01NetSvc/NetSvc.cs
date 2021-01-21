/****************************************************
	文件：NetSvc.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/30 22:23   	
	功能：网络服务(NetMsg是一个异步多线程类)
*****************************************************/

using System.Collections.Generic;
using PENet;
using PEProtocal;

public class MsgPack
{
    public ServerSession Session;
    public GameMsg Msg;

    public MsgPack(ServerSession session, GameMsg msg)
    {
        Session = session;
        Msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc instance = null;

    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }

            return instance;
        }
    }

    //消息队列
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    //线程锁
    public static readonly string obj = "lock";

    /// <summary>
    /// 网络初始化
    /// </summary>
    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.SrvIp, SrvCfg.SrvPort);

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(MsgPack pack)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(pack);
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            PECommon.Log("PackCount:" + msgPackQue.Count);
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD) pack.Msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
        }
    }
}