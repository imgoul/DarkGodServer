/****************************************************
	文件：ChatSys.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月7日 12:20:46	
	功能：聊天业务系统
*****************************************************/

using System.Collections.Generic;
using PENet;
using PEProtocal;

public class ChatSys
{
    private static ChatSys instance = null;

    private CacheSvc cacheSvc = null;

    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
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
        PETool.LogMsg("ChatSys Init Done.");
    }


    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.Msg.sndChat;

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.Session);

        //任务进度数据更新
        TaskSys.Instance.CalcTaskPrgs(pd,6);
        
        GameMsg msg = new GameMsg()
        {
            cmd = (int) CMD.PshChat,
            pshChat = new PshChat()
            {
                name = pd.name,
                chat = data.chat
            }
        };

        //广播所有在线客户端
        List<ServerSession> list = cacheSvc.GetOnlineServerSessions();
        byte[] bytes = PETool.PackNetMsg(msg);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SendMsg(bytes);
        }
    }
}