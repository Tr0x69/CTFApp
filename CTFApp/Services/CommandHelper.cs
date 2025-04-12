using System.Diagnostics;

namespace CTFApp.Services
{
    public class CommandManager
    {
        public string? output { get; set; }

        private string? _command;

        public string? command
        {
            get { return _command; }

            set
            {
                _command = value;

                Process p = new Process();
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "/usr/bin/env";
                p.StartInfo.Arguments = _command;
                p.Start();

                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Console.WriteLine(output);
            }
        }
    };
}
