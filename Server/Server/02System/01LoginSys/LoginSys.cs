/****************************************************
	文件：LoginSys.cs
	作者：unravel
	邮箱: 274216398@qq.com
	日期：2019/05/31 10:04   	
	功能：登录业务系统
*****************************************************/
public class LoginSys
{
    private static LoginSys instance = null;
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

    }
}