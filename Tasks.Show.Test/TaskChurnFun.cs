using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelLab.Common;
using PixelLab.Contracts;
using Tasks.Show.Helpers;
using Tasks.Show.Models;

namespace Tasks.Show.Test
{
    [TestClass]
    public class TaskChurnFun
    {
        [TestMethod]
        public void RenameFolder()
        {
            var taskData = TestUtil.GetTaskData();
            var folderToRename = taskData.AllFolders.OfType<Folder>().ToArray().Random();
            var otherExisting = taskData.AllFolders.OfType<Folder>().Where(other => !other.Equals(folderToRename)).ToArray().Random();

            taskData.RenameFolder(folderToRename, folderToRename.Name);
            taskData.RenameFolder(folderToRename, folderToRename.Name.ToUpperInvariant());
            taskData.RenameFolder(folderToRename, folderToRename.Name.ToLowerInvariant());

            var novelName = Environment.TickCount.ToString();
            Contract.Requires(!taskData.AllFolders.Any(bf => bf.Name.EasyEquals(novelName)), "test a totally novel name");
            taskData.RenameFolder(folderToRename, novelName);

            taskData.RenameFolder(folderToRename, otherExisting.Name);
            Assert.IsFalse(taskData.AllFolders.Contains(folderToRename), "should no longer contain the folder -> it got merged");
        }

        [TestMethod]
        public void RemoveFolder()
        {
            var taskData = TestUtil.GetTaskData();
            var folderToRemove = taskData.AllFolders.OfType<Folder>().ToArray().Random();

            var associatedTasks = taskData.Tasks.Where(task => task.Folder == folderToRemove).ToArray();
            Contract.Requires(associatedTasks.Length > 0);

            taskData.RemoveFolder(folderToRemove);
            Assert.IsTrue(associatedTasks.All(task => task.Folder == null));
        }
    }
}
