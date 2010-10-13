using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelLab.Common;
using Tasks.Show.Models;

namespace Tasks.Show.Test
{
    public static class TestUtil
    {
        public static void AssertTaskDataEqual(TaskData a, TaskData b)
        {
            Assert.AreEqual(a.CurrentFolder, b.CurrentFolder);
            Assert.AreEqual(a.AllFolders.Count, b.AllFolders.Count);

            System.Threading.Tasks.Parallel.For(0, a.AllFolders.Count, i =>
            {
                Assert.AreEqual(a.AllFolders[i], b.AllFolders[i]);
            });

            Assert.AreEqual(a.Tasks.Count, b.Tasks.Count);

            System.Threading.Tasks.Parallel.For(0, a.Tasks.Count, i =>
            {
                Assert.IsTrue(ArraysEqual(GetArray(a.Tasks[i]), GetArray(b.Tasks[i])));
            });
        }

        public static bool EditTaskEquals(DraftTask a, DraftTask b)
        {
            return ArraysEqual(GetArray(a), GetArray(b));
        }

        private static object[] GetArray(BaseTask a)
        {
            return new object[] { a.Completed, a.Description, a.Due, a.Estimate, a.IsImportant };
        }

        public static object[] GetArray(DraftTask a)
        {
            return GetArray((BaseTask)a).Concat(new string[] { a.FolderName }).ToArray();
        }

        public static object[] GetArray(Task a)
        {
            return GetArray((BaseTask)a).Concat(new Folder[] { a.Folder }).ToArray();
        }

        public static bool ArraysEqual(object[] a, object[] b)
        {
            if (a != null && b != null & a.Length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (!object.Equals(a[i], b[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IEnumerable<Task> GetTasks(Folder[] folders = null)
        {
            var stringOptions = new string[] { "", null, "   ", "kevin", DateTime.Now.ToLongDateString(), "you rock the house with cheese" };
            folders = folders ?? new Folder[] { null, new Folder("foo", Colors.Red) };
            List<Folder> folderOptions = folders.ToList();
            if (!folderOptions.Contains(null))
            {
                folderOptions.Add(null);
            }

            var dateOptions = new DateTime?[] { null, DateTime.Now, DateTime.Now.AddDays((Util.Rnd.NextDouble() - .5) * 100) };

            foreach (TimeSpan? estimate in new TimeSpan?[] { null, TimeSpan.FromDays(Util.Rnd.NextDouble() * 10), TimeSpan.FromDays(Util.Rnd.NextDouble() * 10) })
            {
                foreach (bool? important in new bool?[] { true, false, null })
                {
                    foreach (var description in stringOptions)
                    {
                        foreach (var completeData in dateOptions)
                        {
                            foreach (var folder in folderOptions)
                            {
                                foreach (var dueDate in dateOptions)
                                {
                                    yield return new Task() { Description = description, Due = dueDate, Folder = folder, Completed = completeData, Estimate = estimate, IsImportant = important, };
                                }
                            }
                        }
                    }
                }
            }

        }

        public static TaskData GetTaskData()
        {
            var colors = new Color[] { Colors.Red, Colors.Transparent, Colors.Blue };
            var folders = (new string[] { "kevin", "brian", "Test", "ZOO" })
                .Select(str => new Folder(str, colors.Random()))
                .ToArray();
            var tasks = TestUtil.GetTasks(folders);

            var currentFolderOptions = folders
                .Cast<BaseFolder>()
                .Concat(new SpecialFolder[] { SpecialFolder.AllFolder, SpecialFolder.InboxFolder })
                .ToArray();

            return new TaskData(tasks, folders, currentFolderOptions.Random(), "foo");

        }

    }
}
