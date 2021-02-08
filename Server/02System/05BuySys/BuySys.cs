/****************************************************
	文件：BuySys.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月7日 18:16:26	
	功能：购买系统
*****************************************************/
using PENet;
using PEProtocal;
public class BuySys
{
	private static BuySys instance = null;

	private CacheSvc cacheSvc = null;

	public static BuySys Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BuySys();
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
		PETool.LogMsg("BuySys Init Done.");
	}


	public void ReqBuy(MsgPack pack)
	{
		ReqBuy data = pack.Msg.reqBuy;
		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspBug
		};
		
		
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.Session);

		if (pd.diamond<data.cost)
		{
			msg.err = (int) ErrorCode.LackDiamond;
		}
		else
		{
			pd.diamond -= data.cost;
			switch (data.type)
			{
				case 0:
					pd.power += 100;
					break;
				case 1:
					pd.coin += 1000;
					break;
					
			}

			if (!cacheSvc.UpdatePlayerData(pd.id,pd))
			{
				msg.err = (int) ErrorCode.UpdateDBError;
			}
			else
			{
				RspBuy rspBuy = new RspBuy()
				{
					type = data.type,
					diamond = pd.diamond,
					coin = pd.coin,
					power = pd.power
				};
				msg.rspBuy = rspBuy;
			}
			
		}

		pack.Session.SendMsg(msg);

	}
}