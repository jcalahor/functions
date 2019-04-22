using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;

namespace FunctionsCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter lambda to use: ");
            var lambdaString = Console.ReadLine();

            var calc = new Calc(lambdaString, new List<int> { 10, 11, 13 });
            calc.Execute();
        }
    }

    class Calc
    {
        private int holderV;

        private readonly Func<int, int> fun;
        private readonly List<int> nums;

        private List<int> results = new List<int>();
        
        public Calc(string lambdaString, List<int> ns)
        {
            fun = CreateFun<int, int>(lambdaString);
            nums = ns;
        }

        public void Execute()
        {
            nums.ForEach(n => Context(n));
            results.ForEach(r => Console.WriteLine(r));
        }

        private void m1() { holderV += fun(1);  }
        private void m2() { holderV *= fun(10);  }
        private void m3() { holderV += fun(100); }

        private void Context(int n)
        {
            holderV = n;

            var actions = new List<Action> { m1, m2, m3 };
            actions.ForEach(m => ShowStatus(m));
            results.Add(holderV);
        }

        private void ShowStatus(Action m)
        {
            Console.WriteLine($"Executing {m.Method.Name}, current value is {holderV}");
            m();
        }

        private Func<S, T> CreateFun<S, T>(string lambda)
        {
            string inputType = typeof(S).ToString();
            string outputType = typeof(T).ToString();
            string lambdaDef = $"System.Func<{inputType},{outputType}> fun = {lambda};";
            var script = CSharpScript.Create(lambdaDef).ContinueWith<Func<S, T>>($"fun");
            return script.RunAsync().Result.ReturnValue;
        }
    }
}
