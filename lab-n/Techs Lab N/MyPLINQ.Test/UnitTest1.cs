using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPLINQ.Test;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestToList()
    {
        var list = new List<int>()
        {
            1, 2, 3, 4
        };

        var paraList = list
            .AsPara()
            .ToList();

        WriteCollection(paraList);

        foreach (var t in list)
            Assert.IsTrue(paraList.Contains(t));
    }
    
    [TestMethod]
    public void TestToSelect()
    {
        var list = new List<int>
        {
            1000, 300, 300, 300, 300, 300, 300, 300,
            300, 300, 300, 300, 300, 300, 300, 300,
        };
        // list.AsParallel ()
        //     .Select(n => n).ToList();
        // list.AsQueryable()

        var count = list
            .Select(num =>
            {
                Console.WriteLine(num);
                return num;
            })
            .Select(num => num * 2)
            .Where(num => num < 1000)
            .Select(num =>
            {
                Console.WriteLine(num);
                return num;
            })
            .Count();

        var paraList = list
            .AsPara()
            .Select(n =>
            {
                // var sb = new StringBuilder(n + "\n");
                // foreach (var stackFrame in new StackTrace().GetFrames())
                // {
                //     sb.Append(stackFrame);
                //     //Console.WriteLine(stackFrame);
                // }
                // Console.WriteLine(sb.ToString());
                Thread.Sleep(n);
                return n;
            })
            .ToList();

        WriteCollection(paraList);
    }
    
    [TestMethod]
    public void TestParallelCollection()
    {
        var list = new List<int>
        {
            1000, 300, 300, 300, 300, 300, 300, 300,
            300, 300, 300, 300, 300, 300, 300, 300,
        };

        var res = list
            .AsParallelCollection()
            .Select(ms =>
            {
                Thread.Sleep(ms);
                return ms * 2;
            })
            .Where(ms => ms > 1000)
            .ToList();

        WriteCollection(res);
    }

    [TestMethod]
    public void TestStdParallel()
    {
        var list = new List<int>
        {
            1000, 300, 300, 300, 300, 300, 300, 300,
            300, 300, 300, 300, 300, 300, 300, 300,
        };

        var res = list
            .AsParallel()
            .Select(ms =>
            {
                Thread.Sleep(ms);
                return ms * 2;
            })
            .Where(ms => ms > 1000)
            .ToList();

        WriteCollection(res);
    }
    
    
    
    private void WriteCollection<T>(List<T> collection)
    {
        foreach (var i in collection)
            Console.Write(i + " ");
        Console.WriteLine();
    }
}