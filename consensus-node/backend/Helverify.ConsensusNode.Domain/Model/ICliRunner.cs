using System.Diagnostics;

namespace Helverify.ConsensusNode.Domain.Model;

/// <summary>
/// Allows to run commands on the system command line.
/// </summary>
public interface ICliRunner
{
    /// <summary>
    /// Based on https://stackoverflow.com/questions/63769059/exectute-a-linux-shell-command-from-asp-net-core-3-app
    /// </summary>
    /// <param name="command"></param>
    /// <param name="arguments"></param>
    Process Execute(string command, string arguments);
}