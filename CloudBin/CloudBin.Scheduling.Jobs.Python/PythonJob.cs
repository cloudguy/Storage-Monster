using CloudBin.Scheduling.Core;
using Microsoft.Scripting;

namespace CloudBin.Scheduling.Jobs.Python
{
    public sealed class PythonJob : IJob
    {
        public string JobType
        {
            get { return "Python script"; }
        }

        public string JobName { get; set; }

        public void Execute(IJobParameters parameters)
        {
            var engine = IronPython.Hosting.Python.CreateEngine();
            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromString(parameters.Parameters["script"], SourceCodeKind.Statements);
            var compiled = source.Compile();
            compiled.Execute(scope);  
        }
    }
}
