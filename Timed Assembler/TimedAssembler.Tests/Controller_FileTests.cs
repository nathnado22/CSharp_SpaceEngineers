﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nado.TimedBlocks;
using Sandbox.ModAPI;
using TimedAssembler.IO;
using TimedAssembler.Tests.Utils;

namespace TimedAssembler.Tests
{
    namespace ControllerTests
    {
        [TestClass]
        public class Controller_FileTests
        {
            [TestMethod]
            public void TestSaveLoad()
            {
                int START_FIRST = 0900, END_FIRST = 1100;
                int START_SECOND = 2100, END_SECOND = 2300;

                var controller = TestUtils.GenerateEmptyController();

                controller.AddTime(START_FIRST, END_FIRST);
                controller.AddTime(START_SECOND, END_SECOND);
                controller.SaveChanges();

                string cfgFilename = string.Format(FileController.CfgFormat, controller.FILENAME_CFG);

                //..Make sure the file was created
                Assert.IsTrue(File.Exists(cfgFilename),"The file wasn't saved");

                //..Now check to make sure it can be loaded
                string oldPath = cfgFilename;
                controller = TestUtils.GenerateEmptyController();

                TimedBlockConfig cfg = FileController.LoadFile<TimedBlockConfig>(oldPath);
                controller.LoadConfig(cfg);

                Assert.IsNotNull(controller.GetTimes().FirstOrDefault(t => t.StartHour == START_FIRST && t.FinishHour == END_FIRST),"Time values were not correct");
                Assert.IsNotNull(controller.GetTimes().FirstOrDefault(t => t.StartHour == START_SECOND && t.FinishHour == END_SECOND), "Time values were not correct");
            }
        }
    }
}
