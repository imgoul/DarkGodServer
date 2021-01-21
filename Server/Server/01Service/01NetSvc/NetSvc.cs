/****************************************************
	文件：NetSvc.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/30 22:23   	
	功能：网络服务
*****************************************************/

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

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {

    }
}

