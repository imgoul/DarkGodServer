/****************************************************
	文件：TImerSvc.cs
	作者：疯人院の病友
	邮箱: 274216398@qq.com
	日期：2021年2月7日 20:57:59	
	功能：计时服务
*****************************************************/

using System;
using System.Collections.Generic;
using PEProtocal;
using PENet;

public class TimerSvc
{
    class TaskPack
    {
        public int tid;
        public Action<int> cb;

        public TaskPack(int tid, Action<int> cb)
        {
            this.tid = tid;
            this.cb = cb;
        }
    }

    private static TimerSvc instance = null;
    private PETimer pt = null;
    private Queue<TaskPack> tpQue = new Queue<TaskPack>();
    private static readonly string tpQueLock = "tpQueLock";

    public static TimerSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimerSvc();
            }

            return instance;
        }
    }

    public void Init()
    {
        tpQue.Clear();
        pt = new PETimer(100);

        //设置日志输出
        pt.SetLog((info => { PECommon.Log(info); }));

        pt.SetHandle(((cb, tid) =>
        {
            if (cb != null)
            {
                lock (tpQueLock)
                {
                    tpQue.Enqueue(new TaskPack(tid, cb));
                }
            }
        }));
        PECommon.Log("TimerSvc Init Done.");
    }


    public void AddTimeTask(Action<int> cb, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        pt.AddTimeTask(cb, delay, timeUnit, count);
    }

    public void Update()
    {
        if (tpQue.Count > 0)
        {
            TaskPack tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }

            if (tp != null)
            {
                tp.cb(tp.tid);
            }
        }
    }

    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}