using System.Diagnostics;

namespace Helverify.ConsensusNode.Domain.Model
{
    internal class CliRunner : ICliRunner
    {
        /// <summary>
        /// Based on https://stackoverflow.com/questions/63769059/exectute-a-linux-shell-command-from-asp-net-core-3-app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        public void Execute(string command, string arguments)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments
                }
            };
            
            process.Start();
        }
    }
}
