using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.Core
{
    public class TestMessageData
    {
        public TestMessageData()
        {
        }

        public TestMessageData(int num1, int num2)
        {
            this.Number1 = num1;
            this.Number2 = num2;
        }

        public int Number1 { get; set; }
        public int Number2 { get; set; }
    }
}
