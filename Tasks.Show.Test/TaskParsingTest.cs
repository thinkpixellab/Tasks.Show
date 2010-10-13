using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.Show.Models;

namespace Tasks.Show.Test
{
    [TestClass]
    public class TaskParsingTest
    {
        [TestMethod]
        public void ParsingTest()
        {
            var samples = new Dictionary<string, DraftTask>();

            samples.Add(
                "A crazy sample task name with simple text",
                new DraftTask() { Description = "A crazy sample task name with simple text" }
            );

            samples.Add(
                "A task due today on: today",
                new DraftTask() { Description = "A task due today", Due = DateTime.Now.Date }
                );

            samples.Add(
                "A task due today in personal on: today in: personal",
                new DraftTask()
                {
                    Description = "A task due today in personal",
                    Due = DateTime.Now.Date,
                    FolderName = "personal"
                });

            foreach (var pair in samples)
            {
                var task = TaskParser.Parse(pair.Key);
                Assert.IsTrue(
                    TestUtil.EditTaskEquals(pair.Value, task),
                    string.Format("Had issues with '{0}'", pair.Key));
            }
        }
    }
}
