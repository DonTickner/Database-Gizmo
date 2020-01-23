using System;
using System.Collections.Generic;
using System.Text;

namespace Database_Gizmo.Structure
{
    /// <summary>
    /// Represents a result from testing an <see cref="SqlConnection"/>.
    /// </summary>
    public class SQLConnectionTestResult
    {
        /// <summary>
        /// Represents if the test was successful.
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The message returned for the test result.
        /// </summary>
        public string ResultMessage { get; set; }
    }
}
