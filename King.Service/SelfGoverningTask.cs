namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A task that determines its frequency through actions taken
    /// </summary>
    /// <remarks>
    /// If there is work is to be done then the task gets more aggressive
    /// If there is no work to be done then the task backs off
    /// - The result should be that the task is working at a needed capacity.
    /// </remarks>
    public class SelfGoverningTask
    {
    }
}