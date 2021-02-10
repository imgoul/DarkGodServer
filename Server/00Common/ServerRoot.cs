/****************************************************
	文件：ServerRoot.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/30 22:17   	
	功能：服务器初始化
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }
    public void Init()
    {
        

        //服务层初始化
        NetSvc.Instance.Init();
        CacheSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();


        //数据层初始化
        DBMgr.Instance.Init();

        //业务系统层初始化
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        FubenSys.Instance.Init();




    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }


    private int SessionID=0;
    public int GetSessionID()
    {
        if(SessionID==int.MaxValue)
        {
            SessionID=0;
        }
        return SessionID+=1;
    }
}