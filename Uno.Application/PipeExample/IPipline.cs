using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Application.PipeExample
{
    public interface IPipline
    {
        Action<object> Next { get; }
        void Visit(object data);
    }


    public class AddIssuePipe : IPipline
    {

        Action<object> Next { get; }
        public void Visit(object data)
        {
            //do something
            if (Next is not null)
                Next(data);
        }
    }


    public class AddAttachmentIssuePipe : IPipline
    {
        Action<object> Next { get; }
        public void Visit(object data)
        {
            //do something

            if (Next is not null)
                Next(data);
        }
    }


    public class PipelineBuilder
    {
        private List<IPipline> _pipes;

        public PipelineBuilder Add<T>() where T : IPipline
        {
            _pipes.Add(T);
            return this;
        }

        public Action<object> Build()
        {
            foreach (var item in _pipes)
            {
                //chain
            }

            return // avalin action ro Visit(object data);
        }

    }


    public class Program
    {
        public static void Main()
        {
            object data;
            var pieline = new PipelineBuilder()
                                .Add<AddIssuePipe>()
                                .Add<AddAttachmentIssuePipe>()
                                .Build();

            pieline(data);
        }
    }
}
