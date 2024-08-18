using GoOS.GUI;
using System;
using System.Collections.Generic;

namespace GoOS
{
    internal class ProcessManager
    {
        public List<Process> Processes;

        public ProcessManager()
        {
            Processes = new List<Process>();
        }

        public void addProcess(Process p)
        {
            for (int i = 0; i < Processes.Count; i++) { if (Processes[i].PID == p.PID) return; }

            Processes.Add(p);
        }

        public void Execute()
        {

        }
    }
}
