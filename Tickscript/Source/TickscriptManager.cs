﻿namespace Tickscript
{
    public class TickscriptManager
    {
        public float RestingTime { get; internal set; }
        public float StartRestingBeat { get; internal set; }
        public float CommandBeat { get; internal set; }

        #region Looping

        public int LoopTimes { get; set; }
        public int LoopStartIndex { get; set; }
        public int LoopEndIndex { get; set; }

        #endregion
        public int SkipCommands { get; set; }
        public bool GoingToBeat { get; set; } = false;

        public int TokenIndex { get; internal set; }

        internal bool InParams { get; set; }

        public bool Started { get; set; }
        public bool Ended { get; internal set; }

        #region Custom

        public bool IsResting { get; set; }

        public void IncreaseTokenIndex()
        {
            TokenIndex++;
        }

        public void SetTokenIndex(int index)
        {
            TokenIndex = index;
        }

        #endregion
    }
}