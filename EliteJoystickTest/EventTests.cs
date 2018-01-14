using System;
using EddiJournalMonitor;
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
    }
}
