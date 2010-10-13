using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.Show.Helpers;
using Tasks.Show.Models;

namespace Tasks.Show.Test
{
    [TestClass]
    public class OpenAndSaveTest
    {
        [TestMethod]
        public void TestFolderXmlRoundTrip()
        {
            var folder = new Folder("test", Colors.Red);
            Guid key;
            var xml = folder.ToXml(out key);

            Guid newKey;
            var newFolder = XmlHelper.GetFolder(xml, out newKey);

            Assert.AreEqual(key, newKey);
            Assert.AreEqual(folder, newFolder);
        }

        [TestMethod]
        public void TestTaskXmlRoundTrip()
        {
            foreach (var task in TestUtil.GetTasks())
            {
                IDictionary<Folder, Guid> folderMap;
                var folders = new List<Folder>();
                if (task.Folder != null)
                {
                    folders.Add(task.Folder);
                }
                folders.ToXml(out folderMap);

                var xml = task.ToXml(f => folderMap[f]);
                Debug.WriteLine(xml);

                var newTask = XmlHelper.GetTask(xml, g => folderMap.First(kvp => kvp.Value == g).Key);
                Assert.IsTrue(TestUtil.ArraysEqual(TestUtil.GetArray(task), TestUtil.GetArray(newTask)));
            }
        }

        [TestMethod]
        public void TestTaskDataRoundTrip()
        {
            var taskData = TestUtil.GetTaskData();
            foreach (var currentFolder in taskData.AllFolders)
            {
                taskData.CurrentFolder = currentFolder;
                AssertGoodRoundTrip(taskData);
            }
        }

        private static void AssertGoodRoundTrip(TaskData taskData)
        {
            var xml = taskData.ToXml();

            var newTaskData = XmlHelper.GetTaskData(xml);

            TestUtil.AssertTaskDataEqual(taskData, newTaskData);
        }

    }
}
