using System;

namespace Homework1
{
    public class Program
    {
        public static void Main(string[] args)  // 该程序用于Debug
        {
            IProgress progress = new Progress();
            for (int i = 1; i <= 20; i++)
            {
                try
                {
                    progress.Start(i * ((i % 2 == 0) ? 1 : -1));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (i % 3 != 0)
                {
                    progress.Add(i - 1);
                }
                else progress.Sub(i - 1);

                if (i == 6 || i == 7)
                {
                    progress.Double();
                }
                Console.WriteLine(progress.GetProgress().ToString());
            }
        }
    }

    public interface IProgress
    {
        public int Num { get; } // Progress的序号，表明是第几个实例化的Progress
        public int RequiredProgress { get; } // Progress加载完成所需进度
        public int FinishedProgress { get; } // FinishedProgress指其中已完成的进度, FinishedProgress应当在[0,RequiredProgress]中

        /// <summary>
        /// 尝试加载下一次进度条，requiredProgress指再次加载进度条所需进度
        /// 如果之前进度条已经加载完成，则将进度清零开始下一次加载，返回true，但如果requiredProgress<0，应当报错
        /// 如果之前进度条尚未加载完成，返回false
        /// </summary>
        public bool Start(int requiredProgress);

        public void Add(int addProgress); //增加addProgress的进度
        public void Sub(int subProgress); //减少subProgress的进度
        public void Double(); //进度翻倍

        /// <summary>
        ///  FinishedProgress指其中已完成的进度，RequiredProgress指当前Progress完成所需进度
        /// </summary>
        public (int FinishedProgress, int RequiredProgress) GetProgress();
    }

    public class Progress : IProgress
    {
        private static int count = 0;
        private int num;
        private int requiredProgress;
        private int finishedProgress;

        public Progress()
        {
            num = ++count;
            requiredProgress = 0;
            finishedProgress = 0;
        }

        public int Num => num;
        public int RequiredProgress => requiredProgress;
        public int FinishedProgress => finishedProgress;

        public bool Start(int requiredProgress)
        {
            if (requiredProgress < 0)
                throw new ArgumentException("RequiredProgress must be positive.", "Homework1.Progress");

            if (finishedProgress < this.requiredProgress)
                return false;

            this.requiredProgress = requiredProgress;
            this.finishedProgress = 0;
            return true;
        }

        public void Add(int addProgress)
        {
            finishedProgress = Math.Min(finishedProgress + addProgress, requiredProgress);
        }

        public void Sub(int subProgress)
        {
            finishedProgress = Math.Max(finishedProgress - subProgress, 0);
        }

        public void Double()
        {
            finishedProgress = Math.Min(finishedProgress * 2, requiredProgress);
        }

        public (int FinishedProgress, int RequiredProgress) GetProgress()
        {
            return (finishedProgress, requiredProgress);
        }
    }
}