using System;
using EddiDataDefinitions;
using EddiDataProviderService;
using EddiJournalMonitor;
using Faz.SideWinderSC.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EliteJoystickTest
{
    [TestClass]
    public class EventTests
    {

        [TestMethod]
        public void TestJumped()
        {
            var joystickResponder = new EddiJoystickResponder.JoystickResponder();

            joystickResponder.Start();

            using (var file = new System.IO.StreamReader("Journal.171230210516.01.log"))
            {
                var journalEntry = "";
                while ((journalEntry = file.ReadLine()) != null)
                {
                    var events = JournalMonitor.ParseJournalEntry(journalEntry);

                    foreach (var journalEvent in events)
                        joystickResponder.Handle(journalEvent);
                }
            }
        }

        [TestMethod]
        public void TestOutputSystem()
        {
            StarSystem starSystem = StarSystemSqLiteRepository.Instance.GetOrCreateStarSystem("Maia", true);
            EddiJoystickResponder.Exploration.Actions.OutputValuableSystems(null, starSystem); 
        }

        [TestMethod]
        public void TestJoystickStartup()
        {
            var test = Faz.SideWinderSC.Logic.Swff2Controller.RetrieveAll();
        }
    }
}
