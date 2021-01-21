/****************************************************
	文件：ServerRoot.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/30 22:12   	
	功能：服务器端入口
*****************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();
        while (true)
        {
            ServerRoot.Instance.Update();
        }
    }
}