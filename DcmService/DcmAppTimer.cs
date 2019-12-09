using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DcmService
{
    public delegate void DcmAppTimerEventHandler();

    public class DcmAppTimer
    {
        private readonly Timer timer = new Timer();
        private readonly int elapsedPeriod = 10; //1ms

        public int Period { get; set; }

        private bool started = false;
        public bool Started
        {
            get { return started; }
            set
            {
                // 重新启动
                if (started == false && value == true)
                {
                    //tick = 0;
                }
                started = value;
            }
        }

        public event DcmAppTimerEventHandler DcmAppTimerEvent;

        //private int tick = 0;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!started) return;

            //++tick;
            //if (tick >= (Period / elapsedPeriod))
            //{
            //    tick = 0;
            //    DcmAppTimerEvent?.Invoke();
            //}

            DcmAppTimerEvent?.Invoke();
        }

        #region Singleton implement
        private static DcmAppTimer instance;
        private static object syncRoot = new object();

        private DcmAppTimer()
        {
            InitTimer();
        }

        private void InitTimer()
        {
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = elapsedPeriod;
            timer.Enabled = true;
        }

        public static DcmAppTimer Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmAppTimer();
                    }
                }
            }
            return instance;
        }
        #endregion
    }
}
