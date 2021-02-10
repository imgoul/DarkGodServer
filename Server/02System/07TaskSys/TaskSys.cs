﻿/****************************************************
	文件：TaskSys.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月8日 17:06:13	
	功能：任务奖励系统
*****************************************************/

using PEProtocal;
using PENet;

public class TaskSys
{
	private static TaskSys instance = null;

	private CacheSvc cacheSvc = null;
	private CfgSvc cfgSvc = null;

	public static TaskSys Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new TaskSys();
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
		cfgSvc=CfgSvc.Instance;
		
		PETool.LogMsg("TaskSys Init Done.");
	}

	public void ReqTakeTaskReward( MsgPack pack)
	{
		ReqTakeTaskReward data = pack.Msg.reqTakeTaskReward;
		
		GameMsg msg = new GameMsg()
		{
			cmd = (int) CMD.RspTakeTaskReward
		};
		
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.Session);

		TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(data.tid);
		TaskRewardData trd = CalcTaskRewardData(pd, data.tid);


		if (trd.prgs==trc.count&&!trd.taked)
		{
			pd.coin += trc.coin;
			PECommon.CalcExp(pd,trc.exp);
			trd.taked = true;
			//更新任务进度数据
			CalcTaskArr(pd,trd);

			if (!cacheSvc.UpdatePlayerData(pd.id,pd))
			{
				msg.err = (int) ErrorCode.UpdateDBError;
			}
			else
			{
				RspTakeTaskReward rspTakeTaskReward = new RspTakeTaskReward()
				{
					coin = pd.coin,
					lv = pd.lv,
					exp = pd.exp,
					taskArr = pd.taskArr
				};
				msg.rspTakeTaskReward = rspTakeTaskReward;
			}
		}
		else
		{
			msg.err = (int) ErrorCode.ClientDataError;
		}

		pack.Session.SendMsg(msg);



	}


	public TaskRewardData CalcTaskRewardData(PlayerData pd, int rid)
	{
		TaskRewardData trd = null;
		for (int i = 0; i < pd.taskArr.Length; i++)
		{
			string[] taskInfo = pd.taskArr[i].Split('|');
			//1|0|0
			if (int.Parse(taskInfo[0])==rid)
			{
				trd = new TaskRewardData()
				{
					ID = int.Parse(taskInfo[0]),
					prgs = int.Parse(taskInfo[1]),
					taked = taskInfo[2].Equals("1")
				};
				break;
			}
		}

		return trd;
	}


	public void CalcTaskArr(PlayerData pd, TaskRewardData trd)
	{
		string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
		int index = -1;
		for (int i = 0; i < pd.taskArr.Length; i++)
		{
			string[] taskInfo = pd.taskArr[i].Split('|');
			if (int.Parse(taskInfo[0])==trd.ID)
			{
				index = i;
				break;
			}
		}

		pd.taskArr[index] = result;
	}


	public void CalcTaskPrgs(PlayerData pd,int tid)
	{
		TaskRewardData trd = CalcTaskRewardData(pd, tid);
		TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(tid);


		if (trd.prgs<trc.count)
		{
			trd.prgs += 1;

			//更新任务进度
			CalcTaskArr(pd, trd);

			ServerSession session = cacheSvc.GetOnlineServerSession(pd.id);
			if (session!=null)
			{
				session.SendMsg(new GameMsg()
				{
					cmd = (int)CMD.PshTaskPrgs,
					pshTaskPrgs = new PshTaskPrgs()
					{
						taskArr = pd.taskArr
					}
				});
			}
		}
	}
	
	
	
	public PshTaskPrgs GetTaskPrgs(PlayerData pd,int tid)
	{
		TaskRewardData trd = CalcTaskRewardData(pd, tid);
		TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(tid);


		if (trd.prgs<trc.count)
		{
			trd.prgs += 1;

			//更新任务进度
			CalcTaskArr(pd, trd);

			return new PshTaskPrgs()
			{
				taskArr = pd.taskArr
			};
		}
		else
		{
			return null;
		}
	}
}