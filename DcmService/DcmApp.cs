//#define DEBUG_DISPATCH_PERIOD

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    enum DcmTpHandleResult
    {
        OK = 0,
        OKWithSupporessResp,
        Fail,
        FailWithSend,
        FailWithReceive,
        FailWithReceiveTimeout
    };

    internal class DcmAppRxIndicationArgs
    {
        public DcmTpHandleResult Result { get; set; }
        public List<byte> RequestData { get; set; }
        public List<byte> ResponseData { get; set; }
        public int RequestCanId { get; set; }
        public int ResponseCanId { get; set; }
    }

    delegate void DcmAppRxIndicationCallback(DcmAppRxIndicationArgs args);

    /// <summary>
    /// 主要负责如下职能
    /// 1. 维护诊断应用数据队列
    /// 2. 使用TP层发送应用数据
    /// 3. 诊断应用数据发送完毕后, 解析数据，然后通知诊断服务
    /// </summary>
    class DcmApp
    {
        private DcmTp dcmTp;
        private DcmAppQueue queue;
        private DcmAppDataParser parser;
        private DcmAppTimer timer;

        private readonly List<KeyValuePair<string, string>> FailedHandleResult;
        private const int DispatchPeriod = 10;

        public void SendRequest(int canId, List<byte> data)
        {
            queue.Enqueue(canId, data);
        }

        public bool CanTickEnable { get; set; }
        public uint CanTickPeriod { get; set; }
        private Stopwatch canTickStopWatch = new Stopwatch();

        private DcmAppQueue.Entity currentEntity;

        private DcmAppQueue.Entity beatWithoutRespEntity;
        public DcmAppQueue.Entity BeatWithoutRespEntity
        {
            get
            {
                //if (beatWithoutRespEntity == null)
                {
                    beatWithoutRespEntity = new DcmAppQueue.Entity();
                    beatWithoutRespEntity.CanId = DcmService.FunctionRequestId;
                    beatWithoutRespEntity.Data = new List<byte>();
                    beatWithoutRespEntity.Data.Add(0x3e);
                    beatWithoutRespEntity.Data.Add(0x80);
                }
                return beatWithoutRespEntity;
            }
        }

        private DcmAppQueue.Entity beatWithRespEntity;
        public DcmAppQueue.Entity BeatWithRespEntity
        {
            get
            {
                //if (beatWithRespEntity == null)
                {
                    beatWithRespEntity = new DcmAppQueue.Entity();
                    beatWithRespEntity.CanId = DcmService.PhysicalRequestId;
                    beatWithRespEntity.Data = new List<byte>();
                    beatWithRespEntity.Data.Add(0x3e);
                    beatWithRespEntity.Data.Add(0x00);
                }
                return beatWithRespEntity;
            }
        }

        public List<KeyValuePair<string, string>> FailedWithReceiveHandleResult { get; private set; }
        public List<KeyValuePair<string, string>> FailWithReceiveTimeoutHanldeResult { get; private set; }
        public List<KeyValuePair<string, string>> FailWithSendHandleResult { get; private set; }
        public bool SuppressResponse { get; internal set; }

        private object syncDispatchRoot = new object();


#if DEBUG_DISPATCH_PERIOD
        private Stopwatch stopwatch = new Stopwatch();
#endif

        // 此方法需要以指定周期调用
        private void Dispatch()
        {
#if DEBUG_DISPATCH_PERIOD
            Console.WriteLine("Elapsed: {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
#endif
            lock (syncDispatchRoot)
            {
                if (dcmTp.IsIdle)
                {
                    currentEntity = queue.Dequeue();
                }

                if (currentEntity != null)
                {
                    canTickStopWatch.Stop();
                    if (dcmTp.Execute(currentEntity.CanId, DcmService.ResponseId,
                        currentEntity.Data))
                    {
                        currentEntity = null;
                        canTickStopWatch.Restart();
                    }
                }
                else
                {
                    if (CanTickEnable && DcmService.CanIsOpened() && 
                        canTickStopWatch.ElapsedMilliseconds >= CanTickPeriod)
                    {
                        canTickStopWatch.Restart();

                        if (SuppressResponse)
                        {
                            queue.Enqueue(BeatWithoutRespEntity);
                        }
                        else
                        {
                            queue.Enqueue(BeatWithRespEntity);
                        }
                    }
                }
            }

        }

        private void DcmAppRxIndication(DcmAppRxIndicationArgs args)
        {
            List<KeyValuePair<string, string>> parsedList = null;
            bool postiveRes = false;

            switch (args.Result)
            {
                case DcmTpHandleResult.OK:
                    parsedList = parser.Parse(args.ResponseData, out postiveRes);
                    break;
                case DcmTpHandleResult.OKWithSupporessResp:
                    return;
                case DcmTpHandleResult.Fail:
                    parsedList = FailedHandleResult;
                    break;
                case DcmTpHandleResult.FailWithReceive:
                    parsedList = FailedWithReceiveHandleResult;
                    break;
                case DcmTpHandleResult.FailWithReceiveTimeout:
                    parsedList = FailWithReceiveTimeoutHanldeResult;
                    break;
                case DcmTpHandleResult.FailWithSend:
                    parsedList = FailWithSendHandleResult;
                    break;
                default:
                    throw new InvalidOperationException("DcmApp: Unknown handle result->"
                        + args.Result.ToString());
            }

            NotifyParsingDataIncommingEvent(parsedList, args, postiveRes);
        }

        private void NotifyParsingDataIncommingEvent(
            List<KeyValuePair<string, string>> parsedList, 
            DcmAppRxIndicationArgs args, bool postiveRes)
        {
            ParsingDataIncommingEventArgs parsingArgs = new ParsingDataIncommingEventArgs();
            parsingArgs.EntryList = parsedList;
            parsingArgs.ResponseData = args.ResponseData;
            parsingArgs.RequestData = args.RequestData;
            parsingArgs.PostiveResponse = postiveRes;
            parsingArgs.RequestCanId = args.RequestCanId;
            parsingArgs.ResponseCanId = args.ResponseCanId;
            DcmService.FireParsingDataIncommingEvent(parsingArgs);
        }

        #region Singleton implement
        private static DcmApp instance;
        private static object syncRoot = new object();

        private DcmApp()
        {
            dcmTp = DcmTp.Instance(DcmAppRxIndication);
            queue = new DcmAppQueue();

            parser = new DcmAppDataParserGroup();
            parser.AddDcmAppDataParser(new DcmAppDataParserISO14229());

            //失败处理结果
            FailedHandleResult = new List<KeyValuePair<string, string>>();
            FailedHandleResult.Add(
                new KeyValuePair<string, string>("HandleResult", "Failed with tp protocol"));

            FailedWithReceiveHandleResult = new List<KeyValuePair<string, string>>();
            FailedWithReceiveHandleResult.Add(
                new KeyValuePair<string, string>("HandleResult", "Recieve error"));

            FailWithReceiveTimeoutHanldeResult = new List<KeyValuePair<string, string>>();
            FailWithReceiveTimeoutHanldeResult.Add(
                new KeyValuePair<string, string>("HandleResult", "Receive Timeout"));

            FailWithSendHandleResult = new List<KeyValuePair<string, string>>();
            FailWithSendHandleResult.Add(
                new KeyValuePair<string, string>("HandleResult", "Send error"));

            timer = DcmAppTimer.Instance();
            timer.DcmAppTimerEvent += Dispatch;
            timer.Period = DispatchPeriod;
            timer.Started = true;


            //TODO: CanTp is unset
        }

        public static DcmApp Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmApp();
                    }
                }
            }
            return instance;
        }
        #endregion
    }
}
