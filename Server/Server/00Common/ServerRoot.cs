﻿/****************************************************
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
        //TODO数据层初始化

        //服务层初始化
        NetSvc.Instance.Init();

        //业务系统层初始化
        LoginSys.Instance.Init();
    }
}