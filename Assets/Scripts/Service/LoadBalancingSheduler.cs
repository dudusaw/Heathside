using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Heathside.AI
{
    public interface RunBehavior
    {
        /// <summary>
        /// Function get called each frame with time constraint. It should return if the limit has reached.
        /// </summary>
        /// <param name="ticksToRun">Available time for this behavior to process in the particular frame.
        /// Counting in ticks, 1 tick is 100 nanoseconds (10mil ticks is 1 second)</param>
        void Run(long ticksToRun);
    }

    public class LoadBalancingSheduler : RunBehavior
    {
        private readonly struct BehaviorRecord
        {
            public readonly RunBehavior func;
            public readonly int frequency;
            public readonly int phase;

            public BehaviorRecord(RunBehavior func, int frequency, int phase)
            {
                this.func = func;
                this.frequency = frequency;
                this.phase = phase;
            }
        }

        private const int framesToCheck = 15;

        private List<BehaviorRecord> records = new List<BehaviorRecord>();
        private Stopwatch stopwatch = new Stopwatch();
        private int frame;

        public void AddBehavior(RunBehavior func, int frequency, int phase)
        {
            if (func == null)
            {
                throw new NullReferenceException("func is null");
            }
            if (frequency < 1)
            {
                throw new ArgumentOutOfRangeException("frequency should >= 1");
            }
            BehaviorRecord rec = new BehaviorRecord(func, frequency, phase);
            records.Add(rec);
        }

        /// <summary>
        /// Auto phase calculation is performed first
        /// </summary>
        public void AddBehavior(RunBehavior func, int frequency)
        {
            int phase = NextPhase(frequency);
            AddBehavior(func, frequency, phase);
        }

        private int NextPhase(int frequency)
        {
            int frameNum = framesToCheck + frequency;
            int min = int.MaxValue;
            int resultPhase = 0;
            int stackCounter = 0;
            for (int i = 1; i < frameNum - frequency; i++)
            {
                for (int j = 0; j < records.Count; j++)
                {
                    BehaviorRecord record = records[j];
                    int iPlusFreq = i + frequency;
                    if (record.frequency % (i + record.phase) == 0)
                    {
                        stackCounter++;
                    }
                    if (record.frequency % (iPlusFreq + record.phase) == 0)
                    {
                        stackCounter++;
                    }
                }
                if (stackCounter < min)
                {
                    min = stackCounter;
                    resultPhase = i;
                }
                if (stackCounter == 0)
                {
                    break;
                }
                stackCounter = 0;
            }
            return resultPhase;
        }

        public void Run(long ticksToRun)
        {
            frame++;
            BehaviorRecord[] runThese = new BehaviorRecord[records.Count];
            int runCount = 0;

            for (int i = 0; i < runThese.Length; i++)
            {
                BehaviorRecord record = records[i];
                if (record.frequency % (frame + record.phase) == 0)
                {
                    runThese[runCount] = record;
                    runCount++;
                }
            }

            stopwatch.Restart();
            long timeUsed = 0;
            long availableTime = 0;

            for (int i = 0; i < runCount; i++)
            {
                stopwatch.Stop();
                timeUsed = stopwatch.ElapsedTicks;
                stopwatch.Restart();

                ticksToRun -= timeUsed;
                availableTime = ticksToRun / (runCount - i);

                runThese[i].func.Run(availableTime);
            }
            stopwatch.Stop();
        }
    }
}